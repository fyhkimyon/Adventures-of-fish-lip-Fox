using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;  
    private Canvas BGcanvas;
    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public Obstacle2 boxTemplate2;
    public Obstacle3 boxTemplate3;
    public Obstacle4 boxTemplate4;
    public Obstacle5 boxTemplate5;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();

        // 设置groundHeight的y轴位置应该为物体的中心位置加其大小的一半
        groundHeight = transform.position.y + (collider.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }


    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;


        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2; 
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        int ex=Random.Range(0,2);
        int obstacleNum = Random.Range(0, 2);
        if(ex==0){
            for (int i=0; i<obstacleNum; i++)
            {
                GameObject box = Instantiate(boxTemplate.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }
        }

        int ex2=Random.Range(0,2);
        int obstacleNum2 = Random.Range(0, 2);
        if(ex2==0)
        for (int i=0; i<obstacleNum2; i++)
        {
            GameObject box = Instantiate(boxTemplate2.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }


        int ex3=Random.Range(0,2);
        int obstacleNum3 = Random.Range(0, 2);
        if(ex3==0)
        for (int i=0; i<obstacleNum3; i++)
        {
            GameObject box = Instantiate(boxTemplate3.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }

        int ex4=Random.Range(0,2);
        int obstacleNum4 = Random.Range(0, 2);
        if(ex4==0)
        for (int i=0; i<obstacleNum4; i++)
        {
            GameObject box = Instantiate(boxTemplate4.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }


        int ex5=Random.Range(0,2);
        int obstacleNum5 = Random.Range(0, 2);
        if(ex5==0)
        for (int i=0; i<obstacleNum5; i++)
        {
            GameObject box = Instantiate(boxTemplate5.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }
    }

}
