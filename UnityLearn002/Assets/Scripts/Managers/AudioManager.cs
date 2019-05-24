using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    [SerializeField] private AudioSource soundSource;

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
        Debug.Log("Audio manager starting...");
        _network = service;

        SoundVolume = 1.0f;

        Status = ManagerStatus.Started;
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
