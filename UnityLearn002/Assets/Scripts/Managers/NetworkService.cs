using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService : MonoBehaviour
{
    private const string XmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&APPID=e41900cf4f3ced633b4a5e9257cfb312";

    private bool IsResponseValid(UnityWebRequest webRequest)
    {
        if (webRequest.error != null)
        {
            Debug.Log("bad connection");
            return false;
        }

        if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
        {
            Debug.Log("bad data");
            return false;
        }
        return true;
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (var webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (!IsResponseValid(webRequest))
            {
                yield break;
            }
            callback(webRequest.downloadHandler.text);
        }
    }

    public IEnumerator GetWeather(Action<string> callback)
    {
        return CallAPI(XmlApi, callback);
    }
}
