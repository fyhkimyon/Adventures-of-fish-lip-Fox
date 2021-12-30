using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    // 背景向后移动以体现人物向前奔跑
    // depth为调整背景向后移动速度快慢的参数
    public float depth = 1;

    Player player;

    private void Awake()
    {
        // 获取player对象以得到其横向速度
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 背景向后移动的实际速度应该与人物向前移动的速度相一致
        float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= -15)
            pos.x = 80;

        transform.position = pos;
    }
}
