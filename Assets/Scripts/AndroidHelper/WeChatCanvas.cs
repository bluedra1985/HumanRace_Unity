using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeChatCanvas : MonoBehaviour
{
    [SerializeField] Sprite image1;
    [SerializeField] Sprite image2;
    [SerializeField] AndroidConnecter androidConnecter;
    [SerializeField] TextMeshProUGUI text_Mac;
    [SerializeField] TextMeshProUGUI text_Log;
    [SerializeField] TextMeshProUGUI text_Name;

    [SerializeField] Button button_GetMac;
    [SerializeField] Button button_Login;
    [SerializeField] Button button_ShareText;
    [SerializeField] Button button_ShareImage;
    [SerializeField] Button button_ShareWeb;
    [SerializeField] Image sprite_HeadIcon;


    void Awake()
    {
        androidConnecter = GameObject.Find("AndroidConnecter").GetComponent<AndroidConnecter>();
    }
    void Start()
    {
        button_GetMac.onClick.AddListener(GetMac);
        button_Login.onClick.AddListener(LoginWechat);
        button_ShareText.onClick.AddListener(ShareText);
        button_ShareImage.onClick.AddListener(ShareImage);
        button_ShareWeb.onClick.AddListener(ShareWeb);
    }

    public void GetMac()
    {
        text_Mac.text = AndroidConnecter.ANOROID_MAC;
    }
    public void LoginWechat()
    {
        Log("Unity-LoginWechat");
        androidConnecter.LoginWechat();
    }
    public void ShareText()
    {
        androidConnecter.ShareText("ssssss", 1);
    }
    public void ShareImage()
    {
        Log("ShareImage 1");
        Texture2D temp1 = image1.texture;
        Texture2D temp2 = image1.texture;
        byte[] imageData = temp1.EncodeToPNG();
        byte[] thumbImageData = temp2.EncodeToPNG();
        androidConnecter.ShareImage(imageData, thumbImageData, 1);
        Log("ShareImage 2");
    }
    public void ShareWeb()
    {
        string url = "www.baidu.com";
        string title = "AAA";
        string description = "BBBB";
        Texture2D temp1 = image1.texture;
        byte[] thumbImageData = temp1.EncodeToPNG();

        androidConnecter.ShareWeb(url, title, description, thumbImageData, 1);
    }
    public void ShowName(string str)
    {
        text_Name.text = str;
    }
    public void ShowHeadIcon(Sprite sprite)
    {
        sprite_HeadIcon.sprite = sprite;
    }
    public void Log(string str){
        text_Log.text+="\r\n"+str;
    }


}
