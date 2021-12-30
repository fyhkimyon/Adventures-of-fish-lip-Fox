using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetXML : MonoBehaviour
{
    Text RankList;
    // Start is called before the first frame update
    void Start()
    {
        RankList = GameObject.Find("Text").GetComponent<Text>();
        RankList.text="排行榜\n昵称+分数\n";
        XML xml=new XML();
        Dictionary<string, int> res=xml.ReadXML();
        Dictionary<string, int> res_SortedByKey = res.OrderByDescending(p=>p.Value).ToDictionary(p => p.Key, o => o.Value);
        int cnt = 0;
        foreach(KeyValuePair<string, int> keyvalue in res_SortedByKey){
            ++cnt;
            if (cnt <= 6) {
                String name = keyvalue.Key;
                if (name == "") name = "nobody";
                RankList.text += name + " "+keyvalue.Value+"\n";
            } else {
                xml.DeleteXml(keyvalue.Key);
            }
            // print(keyvalue.Key+" "+keyvalue.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //finalDistanceText.text = distance + " m";
        
    }
}
