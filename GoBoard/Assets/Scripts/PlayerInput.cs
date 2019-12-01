using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float m_h;
    private float m_v;

    private bool m_inputEnabled = false;

    public float H { get => m_h; }
    public float V { get => m_v; }

    public bool InputEnabled
    {
        get => m_inputEnabled;
        set => m_inputEnabled = value;
    }

    public void GetKeyInput()
    {
        if (InputEnabled)
        {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
    }
}
