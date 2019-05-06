using System;
using UnityEngine;

public class ImagesManager : MonoBehaviour, IGameManager
{
    private NetworkService _networkService;
    private Texture2D _webImage;

    public ManagerStatus Status { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Images manager starting...");
        _networkService = service;
        Status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback)
    {
        if (_webImage == null)
        {
            StartCoroutine(_networkService.DownloadImage(d =>
            {
                _webImage = d;
                callback(_webImage);
            }));
        }
        else
        {
            callback(_webImage);
        }
    }
}
