using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool _isTriggered;
    public string identifier;

    void OnTriggerEnter(Collider collider)
    {
        if (_isTriggered)
        {
            return;
        }

        Managers.Weather.LogWeather(identifier);
        _isTriggered = true;
    }
}
