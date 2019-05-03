using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService : MonoBehaviour
{
    private const string XmlApi = "https://samples.openweathermap.org/data/2.5/forecast?id=524901&appid=b6907d289e10d714a6e88b30761fae22";

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

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallAPI(XmlApi, callback);
    }
}
