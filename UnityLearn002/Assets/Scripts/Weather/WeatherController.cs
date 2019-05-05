using Assets.Scripts.Broadcast;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float _fullIntensity;
    private float _cloudValue = 0f;

    void Awake()
    {
        Messenger.AddListener(GameEvent.WeatherUpdated, OnWeatherUpdated);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WeatherUpdated, OnWeatherUpdated);
    }

    void Start()
    {
        _fullIntensity = sun.intensity;
    }

    private void OnWeatherUpdated()
    {
        SetOvercast(Managers.Weather.CloudValue);
    }

    private void SetOvercast(float value)
    {
        sky.SetFloat("_Blend", value);
        sun.intensity = _fullIntensity - (_fullIntensity * value);
    }
}
