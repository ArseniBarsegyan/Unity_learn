using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;

    void Start()
    {
        speedSlider.value = PlayerPrefs.GetFloat("speed", 1);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnSubmitName(string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {
        Messenger<float>.Broadcast(GameEvent.SpeedChanged, speed);
        Debug.Log("Speed: " + speed);
        PlayerPrefs.SetFloat("speed", speed);
    }

    public void OnSoundToggle()
    {
        Managers.Audio.MuteSound = !Managers.Audio.MuteSound;
    }

    public void OnSoundValue(float volume)
    {
        Managers.Audio.SoundVolume = volume;
    }
}
