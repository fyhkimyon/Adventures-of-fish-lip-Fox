using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleHook : MonoBehaviour
{

    Player player;
    public float depth = 1;

    LineRenderer line;
    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] float fixedTime = 0f;
    [SerializeField] public float coolingTime = 0f;
    [SerializeField] public bool isCooling = false;

    bool isGrappling = false;
    [HideInInspector] public bool retracting = false;
    public AudioSource shengzi;
    Vector2 target;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {

    }

    public float getCoolingTime()
    {
        return coolingTime;
    }

    public bool getIsCooling()
    {
        return isCooling;
    }

    private void FixedUpdate()
    {
        if (player.isDead) return;
        Vector2 nowP = transform.position;
        // 发出钩子的触发条件为鼠标左键并且人物的y轴距离小于一定值
        if (Input.GetMouseButtonDown(0) && !isGrappling && nowP.y < 30 && (!isCooling || player.isCrazy))
        {
            shengzi.Play();
            fixedTime = 0;
            if (!player.isCrazy)
            {
                coolingTime = 0f;
                isCooling = true;
            }
            StartGrapple();
        }
        // 如果当前钩子钩中地形则按右键可以断开钩子
        else if (Input.GetMouseButtonDown(1) && isGrappling)
        {
            retracting = false;
            isGrappling = false;
            line.enabled = false;
        }

        if (isCooling)
        {
            coolingTime += Time.fixedDeltaTime;
            if (coolingTime >= 3.0f) isCooling = false;
        }

        if (retracting)
        {
            fixedTime += Time.fixedDeltaTime;
            Vector2 grapplePos = Vector2.Lerp(transform.position, target, grappleSpeed * Time.fixedDeltaTime);

            transform.position = grapplePos;

            line.SetPosition(0, transform.position);

            // 设置钩中位置向后移动
            float realVelocity = player.velocity.x / depth;
            target.x -= realVelocity * Time.fixedDeltaTime;
            if (target.x <= -25)
                target.x = 80;
            line.SetPosition(1, target);

            if (Vector2.Distance(transform.position, target) < 1f || fixedTime >= 0.12f)
            {
                retracting = false;
                isGrappling = false;
                line.enabled = false;
            }
        }
    }

    private void StartGrapple()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // 使用raycasting射线检测钩子是否碰撞到目标
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableMask);

        if (hit.collider != null)
        {
            isGrappling = true;
            target = hit.point;
            line.enabled = true;
            line.positionCount = 2;
            StartCoroutine(Grapple());
        }
    }

    IEnumerator Grapple()
    {
        float t = 0;
        float time = 10;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target, t / time);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, target);
        retracting = true;
    }
}
