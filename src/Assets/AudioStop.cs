using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStop : MonoBehaviour
{
    Player player;
    public AudioSource BGM;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            BGM.mute = true;
        }
    }
}
