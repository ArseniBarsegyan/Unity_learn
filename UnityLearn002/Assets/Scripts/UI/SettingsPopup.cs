using Assets.Scripts.Broadcast;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private AudioClip sound;

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
        // Play sound when click on button
        Managers.Audio.PlaySound(sound);
    }

    public void OnSoundValue(float volume)
    {
        Managers.Audio.SoundVolume = volume;
    }

    public void OnPlayMusic(int selector)
    {
        Managers.Audio.PlaySound(sound);
        switch (selector)
        {
            case 1:
                Managers.Audio.PlayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic();
                break;
            default:
                Managers.Audio.StopMusic();
                break;
        }
    }

    public void OnMusicToggle()
    {
        Managers.Audio.MusicMute = !Managers.Audio.MusicMute;
        Managers.Audio.PlaySound(sound);
    }

    public void OnMusicValue(float volume)
    {
        Managers.Audio.MusicVolume = volume;
    }
}
