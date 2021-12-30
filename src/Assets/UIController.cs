using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    Text LifeNumber;
    Text CrazyTime;

    GameObject results;
    Text finalDistanceText;

    private void Awake()
    {
        // 获取player对象
        player = GameObject.Find("Player").GetComponent<Player>();
        // 获取显示距离的text对象
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        LifeNumber = GameObject.Find("LifeNumber").GetComponent<Text>();
        CrazyTime = GameObject.Find("CrazyTime").GetComponent<Text>();
        results.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + " m";
        }

        LifeNumber.text = "生命:" + player.lifeNum;
        if (player.isCrazy)
        {
            float x = 3.0f - player.crazyTime;
            CrazyTime.text = "" + Mathf.CeilToInt(x);
        }
        else
        {
            CrazyTime.text = "";
        }

    }


    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    
}
