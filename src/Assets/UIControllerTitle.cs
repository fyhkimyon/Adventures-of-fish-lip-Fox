using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void RankingList()
    {
        SceneManager.LoadScene("RankingList");
    }

    public void back()
    {
        SceneManager.LoadScene("Menu");
    }
}