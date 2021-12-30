using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator animator; // 动作
    public float gravity; // 重力参数
    public Vector2 velocity; // 速度
    public float maxXVelocity = 100; // 横向速度上限
    public float maxAcceleration = 10; // 横向加速度上限
    public float acceleration = 10; // 横向加速度
    public float distance = 0;
    public float jumpVelocity = 20; // 跳跃速度
    public float groundHeight = 10; // 地面高度
    public bool isGrounded = false; // 是否在地面上

    public bool isHoldingJump = false;// 是否按住空格键
    public float maxHoldJumpTime = 0.4f;// 最长持续跳跃键的时间
    public float maxMaxHoldJumpTime = 0.4f; // 最长持续跳跃时间上限
    public float holdJumpTimer = 0.0f; // 按住跳跃键的计时器

    public float jumpGroundThreshold = 1; //跳跃容错距离

    public bool isDead = false; //是否死亡

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    Rigidbody2D rb;
    GrappleHook gh;
    private BoxCollider2D box;
    [SerializeField] float speed = 5f;

    public int lifeNum = 1;

    public bool isCrazy = false;
    public float crazyTime = 0f;

    float mx;
    float my;

    public AudioSource jump;
    public AudioSource shitou;
    public AudioSource mogu;
    public AudioSource gg;
    public AudioSource yingtao;
    public AudioSource baoshi;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gh = GetComponent<GrappleHook>();
        animator.SetBool("isHurt",false);
    }

    void Update()
    {
        if (isDead)//判断是否死亡
        {
            return;
        }

        mx = Input.GetAxisRaw("Horizontal");
        my = 0;

        // 获取人物的初始位置
        Vector2 pos = transform.position;
        // 获取人物与地面的距离
        float groundDistance = Mathf.Abs(pos.y - groundHeight);
        
        // 允许跳跃的条件：在地面上或者在允许的容错范围内
        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump.Play();
                animator.SetBool("isJumping",true);
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }

    }

    private void FixedUpdate()
    {
        if (isDead)//判断是否死亡
        {
            return;
        }

        if (!gh.retracting)
        {
            rb.velocity = new Vector2(mx, my).normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        Vector2 pos = transform.position;

        if (isCrazy)
        {
            crazyTime += Time.fixedDeltaTime;
            if (crazyTime >= 3.0f)
            {
                isCrazy = false;
                crazyTime = 0;
            }
        }

        if (pos.y < -100)
        {
            isDead = true;
        }

        animator.SetBool("isCheerUp",false);
        animator.SetBool("isHurt",false);
        animator.SetBool("isEat",false);
        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            // 采用raycasting从上往下投影的方式降落
            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y); // x方向添加一个缓冲(容错)
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance,groundLayerMask);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null) // 此时判断主角落在地上
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight; // 更新地面的高度为当前所落在的物体上
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                        animator.SetBool("isJumping",false);
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, velocity.x * Time.fixedDeltaTime,groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        //velocity.x = 0;
                    }
                }
            }
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            // 设置加速度随速度的递增而递减
            acceleration = maxAcceleration * (1 - velocityRatio);
            // 设置最长持续跳跃时间随速度的递增而递减
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            //重新生成地面
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance,groundLayerMask);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }

        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }

        Vector2 obstOrigin2 = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX2 = Physics2D.Raycast(obstOrigin2, Vector2.right, velocity.x * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitX2.collider != null)
        {
            Obstacle2 obstacle2 = obstHitX2.collider.GetComponent<Obstacle2>();
            if (obstacle2 != null)
            {
                hitObstacle2(obstacle2);
            }
        }

        RaycastHit2D obstHitY2 = Physics2D.Raycast(obstOrigin2, Vector2.up, velocity.y * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitY2.collider != null)
        {
            Obstacle2 obstacle2 = obstHitY2.collider.GetComponent<Obstacle2>();
            if (obstacle2 != null)
            {
                hitObstacle2(obstacle2);
            }
        }

        Vector2 obstOrigin3 = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX3 = Physics2D.Raycast(obstOrigin3, Vector2.right, velocity.x * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitX3.collider != null)
        {
            Obstacle3 obstacle3 = obstHitX3.collider.GetComponent<Obstacle3>();
            if (obstacle3 != null)
            {
                hitObstacle3(obstacle3);
            }
        }

        RaycastHit2D obstHitY3 = Physics2D.Raycast(obstOrigin3, Vector2.up, velocity.y * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitY3.collider != null)
        {
            Obstacle3 obstacle3 = obstHitY3.collider.GetComponent<Obstacle3>();
            if (obstacle3 != null)
            {
                hitObstacle3(obstacle3);
            }
        }

        Vector2 obstOrigin4 = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX4 = Physics2D.Raycast(obstOrigin4, Vector2.right, velocity.x * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitX4.collider != null)
        {
            Obstacle4 obstacle4 = obstHitX4.collider.GetComponent<Obstacle4>();
            if (obstacle4 != null)
            {
                hitObstacle4(obstacle4);
            }
        }

        RaycastHit2D obstHitY4 = Physics2D.Raycast(obstOrigin4, Vector2.up, velocity.y * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitY4.collider != null)
        {
            Obstacle4 obstacle4 = obstHitY4.collider.GetComponent<Obstacle4>();
            if (obstacle4 != null)
            {
                hitObstacle4(obstacle4);
            }
        }

        Vector2 obstOrigin5 = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX5 = Physics2D.Raycast(obstOrigin5, Vector2.right, velocity.x * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitX5.collider != null)
        {
            Obstacle5 obstacle5 = obstHitX5.collider.GetComponent<Obstacle5>();
            if (obstacle5 != null)
            {
                hitObstacle5(obstacle5);
            }
        }

        RaycastHit2D obstHitY5 = Physics2D.Raycast(obstOrigin5, Vector2.up, velocity.y * Time.fixedDeltaTime,obstacleLayerMask);
        if (obstHitY5.collider != null)
        {
            Obstacle5 obstacle5 = obstHitY5.collider.GetComponent<Obstacle5>();
            if (obstacle5 != null)
            {
                hitObstacle5(obstacle5);
            }
        }

        transform.position = pos;
    }


    void hitObstacle(Obstacle obstacle)
    {
        animator.SetBool("isHurt",true);
        Destroy(obstacle.gameObject);
        velocity.x *= 0.7f;
        shitou.Play();
    }
    void hitObstacle2(Obstacle2 obstacle)
    {
        animator.SetBool("isCheerUp",true);
        Destroy(obstacle.gameObject);
        velocity.x *= 1.1f;
        if (velocity.x >= maxXVelocity)
        {
            velocity.x = maxXVelocity;
        }
        mogu.Play();
    }

    void hitObstacle3(Obstacle3 obstacle)
    {
        animator.SetBool("isHurt",true);
        Destroy(obstacle.gameObject);
        lifeNum -= 1;
        if (lifeNum == 0)
        {
            isDead=true;
            isGrounded=false;
        }
        gg.Play();
    }
    void hitObstacle4(Obstacle4 obstacle)
    {
        Destroy(obstacle.gameObject);
        if (lifeNum < 3) lifeNum += 1;
        animator.SetBool("isEat",true);
        yingtao.Play();
    }

    void hitObstacle5(Obstacle5 obstacle)
    {
        Destroy(obstacle.gameObject);
        isCrazy = true;
        crazyTime = 0;
        animator.SetBool("isEat",true);
        baoshi.Play();
    }
}