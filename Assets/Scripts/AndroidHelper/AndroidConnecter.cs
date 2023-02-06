using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AndroidConnecter : Singleton<AndroidConnecter>
{
    [SerializeField] WeChatCanvas weChatCanvas;
    string APP_ID = "wx38a27ff6b2edd7e5";
    string SECRET = "bde4d6cf0c7bf83f4e55d4900ed733e5";
    AndroidJavaObject ajObject;
    public static string ANOROID_MAC;

    protected override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID && !UNITY_EDITOR
        ajObject = new AndroidJavaObject("com.HanCrafter.HumanRace.UnityAndroidConnecter");
#endif
    }
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        weChatCanvas.Log("Android Connecter Start 1");
        //ANOROID_MAC = ajObject.Call<string>("getLocalMacAddress");
        ajObject.Call("regToWx");
        weChatCanvas.Log("Android Connecter Start 2");
#endif
    }



    //登录微信
    public void LoginWechat()
    {

        ajObject.Call("login");
    }

    //分享文字
    public void ShareText(string text, int scene)
    {
        ajObject.Call("shareText", text, scene);
    }

    //分享图片
    public void ShareImage(byte[] imageData, byte[] thumbImageData, int scene)
    {
        ajObject.Call("shareImage", imageData, thumbImageData, scene);
    }

    //分享链接
    public void ShareWeb(string url,
                         string title,
                         string description,
                         byte[] thumbImageData,
                         int scene)
    {
        ajObject.Call("shareWeb", url, title, description, thumbImageData, scene);
    }


    //安卓端微信的回调函数
    public void LoginCallback(string str)
    {
        switch (str)
        {
            case "UserCancel":
                break;
            case "UserReject":
                break;
            case "OtherError":
                break;
            default:
                weChatCanvas.Log("登录成功");
                StartCoroutine(IEN_GetUserData(str));
                break;

        }

    }

    //获取用户数据
    IEnumerator IEN_GetUserData(string code)
    {
        weChatCanvas.Log("IEN_GetUserData -1");
        string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APP_ID + "&secret=" + SECRET + "&code=" + code + "&grant_type=authorization_code";
        UnityWebRequest request = new UnityWebRequest(url);
        request.timeout = 5;
        request.downloadHandler=new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        weChatCanvas.Log("IEN_GetUserData -2");
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            weChatCanvas.Log("ConnectionError A");
        }
        else
        {
            weChatCanvas.Log("request.downloadHandler.text");
            WechatData wechatData = JsonUtility.FromJson<WechatData>(request.downloadHandler.text);
            if (wechatData != null)
            {
                string url_GetUser = "https://api.weixin.qq.com/sns/userinfo?access_token=" + wechatData.access_token + "&openid=" + wechatData.openid;
                UnityWebRequest request_GetUser = new UnityWebRequest(url_GetUser);
                request_GetUser.downloadHandler=new DownloadHandlerBuffer();
                request_GetUser.timeout = 5;
                yield return request_GetUser.SendWebRequest();

                if (request_GetUser.result == UnityWebRequest.Result.ConnectionError)
                {
                    weChatCanvas.Log("ConnectionError B");
                }
                else
                {
                    weChatCanvas.Log("request_GetUser");
                    WechatUserInfo userInfo = JsonUtility.FromJson<WechatUserInfo>(request_GetUser.downloadHandler.text);
                    if (userInfo != null)
                    {
                        weChatCanvas.ShowName(userInfo.nickname);

                        UnityWebRequest request_HeadIcon = new UnityWebRequest(userInfo.headimgurl);
                        request_HeadIcon.timeout = 5;
                        DownloadHandlerTexture texDl=new DownloadHandlerTexture();
                        request_HeadIcon.downloadHandler=texDl;
                        yield return request_HeadIcon.SendWebRequest();
                        if (request_HeadIcon.result == UnityWebRequest.Result.ConnectionError)
                        {
                            //weChatCanvas.Log("ConnectionError C");
                        }
                        else {
                            //weChatCanvas.Log("request_HeadIcon");
                            //Texture2D texture = (request_HeadIcon.downloadHandler as DownloadHandlerTexture).texture;
                            Texture2D texture =new Texture2D(200,200);
                            texture= texDl.texture;
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                            weChatCanvas.ShowHeadIcon(sprite);
                        }

                    }
                }

            }


        }

    }



    public void GetAndroidLog(string str)
    {
        weChatCanvas.Log(str);
    }
}
[Serializable]
public class WechatData
{
    public string access_token;
    public string expires_in;
    public string refresh_token;
    public string openid;
    public string scope;

}

[Serializable]
public class WechatUserInfo
{
    public string openid;
    public string nickname;
    public int sex;
    public string province;
    public string city;
    public string country;
    public string headimgurl;
    public string[] privilege;
    public string unionid;
}