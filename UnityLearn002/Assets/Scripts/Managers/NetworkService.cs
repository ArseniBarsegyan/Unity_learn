using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService : MonoBehaviour
{
    private const string XmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&APPID=e41900cf4f3ced633b4a5e9257cfb312";
    private const string WebImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";
    private const string LocalApiUrl = "http://localhost:8080/api/weather";

    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(WebImageUrl);
        yield return webRequest.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(webRequest));
    }

    public IEnumerator GetWeather(Action<string> callback)
    {
        return CallAPI(XmlApi, callback);
    }

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

    private IEnumerator Post(string url, Hashtable args, Action<string> callback)
    {
        UnityWebRequest webRequest;
        if (args == null)
        {
            webRequest = new UnityWebRequest(url);
        }
        else
        {
            var form = new WWWForm();
            foreach (DictionaryEntry arg in args)
            {
                form.AddField(arg.Key.ToString(), arg.Value.ToString());
            }
            webRequest = UnityWebRequest.Post(url, form);
        }
        yield return webRequest;
    }

    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        Hashtable args = new Hashtable();
        args.Add("message", name);
        args.Add("cloud_value", cloudValue);
        args.Add("timestamp", DateTime.UtcNow.Ticks);

        return Post(LocalApiUrl, args, callback);
    }
}
