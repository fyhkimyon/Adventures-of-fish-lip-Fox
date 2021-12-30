using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System;

public class XML : MonoBehaviour
{
    private string name="";
    private Text text;
    private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        inputField.onEndEdit.AddListener (End_Value); 
        text = GameObject.Find("FinalDistanceText").GetComponent<Text>();
    }
    public void End_Value(string inp){
        name=inp;
        print(name);
    }
    public void Add(){
        AddXmlData();
    }
    public void AddXmlData(){
        string localPath=Application.dataPath+"/XMLData.xml";
        if (!File.Exists(localPath) ) {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", "UTF-8", "");//设置xml文件编码格式为UTF-8
            XmlElement root = xml.CreateElement("Data");//创建根节点
            XmlElement info = xml.CreateElement("Info");//创建子节点
            info.SetAttribute("Name","admin");//创建子节点属性名和属性值
            info.SetAttribute("score","0 m");
            root.AppendChild(info);//将子节点按照创建顺序，添加到xml
            xml.AppendChild(root);
            xml.Save(localPath);//保存xml到路径位置
            Debug.Log("创建XML成功！");
        }else if ( File.Exists(localPath) )
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(localPath);//加载xml文件
            XmlNode root = xml.SelectSingleNode("Data");//获取根节点
            XmlElement info = xml.CreateElement("Info");//创建新的子节点
            info.SetAttribute("Name", name);//创建新子节点属性名和属性值
            info.SetAttribute("score", text.text);
            root.AppendChild(info);//将子节点按照创建顺序，添加到xml
            xml.AppendChild(root);
            xml.Save(localPath);//保存xml到路径位置
            Debug.Log("添加XML成功！");
        }
    }


    public void DeleteXml(String name)
    {
        string localPath=Application.dataPath+"/XMLData.xml";
        if ( File.Exists(localPath) )
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(localPath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach ( XmlElement xe in nodeList )
            {
                if ( xe.GetAttribute("Name") == name )
                {
                    xe.RemoveAll();
                }
            }
            xmlDoc.Save(localPath);
            Debug.Log("删除XML成功！");
        }
    }
    public Dictionary<string, int> ReadXML() {
        string localPath=Application.dataPath+"/XMLData.xml";
        Dictionary<string, int> myDictionary = new Dictionary<string, int>();
        if ( File.Exists(localPath) )
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(localPath);//加载xml文件
            XmlNodeList nodeList = xml.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodeList) {//遍历所以子节点
                if (xe.Name== "Info" ) {
                    string name=xe.GetAttribute("Name");
                    string str =xe.GetAttribute("score");
                    int score=0;
                    foreach (char c in str)
                    {
                        if (Convert.ToInt32(c) >= 48 && Convert.ToInt32(c) <= 57)
                        {
                            score=score*10+c-'0';
                        }
                    }
                    if(!myDictionary.ContainsKey(name))
                    myDictionary.Add(name,score);
                    else {
                        int maxx=Math.Max(score,myDictionary[name]);
                        myDictionary[name]=maxx;
                    }
                }
            }
            Debug.Log("读取XML成功！"+xml.OuterXml);
        }
        return myDictionary;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
