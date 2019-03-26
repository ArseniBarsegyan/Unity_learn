using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    [SerializeField] private Vector3 doorPosition;

    private bool _isDoorOpen;

    public void Operate()
    {
        if (_isDoorOpen)
        {
            Vector3 position = transform.position - doorPosition;
            transform.position = position;
        }
        else
        {
            Vector3 position = transform.position + doorPosition;
            transform.position = position;
        }
        _isDoorOpen = !_isDoorOpen;
    }

    public void Activate()
    {
        if (!_isDoorOpen)
        {
            Vector3 pos = transform.position + doorPosition;
            transform.position = pos;
            _isDoorOpen = true;
        }
    }

    public void Deactivate()
    {
        if (_isDoorOpen)
        {
            Vector3 pos = transform.position - doorPosition;
            transform.position = pos;
            _isDoorOpen = false;
        }
    }
}
