using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCool : MonoBehaviour
{

    [SerializeField] public Image image;
    GrappleHook grappleHook;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        grappleHook = GameObject.Find("Player").GetComponent<GrappleHook>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        if (grappleHook.getIsCooling())
        {
            if(image.fillAmount == 1)
            {
                image.fillAmount = 0;
            }
            image.fillAmount += Time.fixedDeltaTime / 3;
        }
    }
}
