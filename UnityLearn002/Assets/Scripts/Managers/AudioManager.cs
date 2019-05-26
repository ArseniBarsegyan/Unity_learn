using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    private float _musicVolume;

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.volume = _musicVolume;
            }
        }
    }

    public bool MusicMute
    {
        get
        {
            if (backgroundMusicSource != null)
            {
                return backgroundMusicSource.mute;
            }
            return false;
        }
        set
        {
            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.mute = value;
            }
        }
    }

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }
    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }
    private void PlayMusic(AudioClip clip)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.Play();
    }
    public void StopMusic()
    {
        backgroundMusicSource.Stop();
    }

    public ManagerStatus Status { get; private set; }

    public float SoundVolume
    {
        get => AudioListener.volume;
        set => AudioListener.volume = value;
    }

    public bool MuteSound
    {
        get => AudioListener.pause;
        set => AudioListener.pause = value;
    }

    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager is starting...");
        _network = service;

        backgroundMusicSource.ignoreListenerVolume = true;
        backgroundMusicSource.ignoreListenerPause = true;

        SoundVolume = 1.0f;
        MusicVolume = 1.0f;

        Status = ManagerStatus.Started;
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
