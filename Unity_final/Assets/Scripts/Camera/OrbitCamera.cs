using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    // Object that will be tracked by camera
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;

    private float _rotY;
    private Vector3 _offset;

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position; // saving delta between camera and target
    }

    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0) // slow camera rotation with arrow keys
        {
            _rotY += horInput * rotSpeed;
        }
        else
        {
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3; // fast rotation with mouse
        }

        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset); // const delta that change its position with camera rotation
        transform.LookAt(target); // camera always look at it's target
    }
}
