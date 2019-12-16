using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float m_h;
    private float m_v;

    public float H => m_h;
    public float V => m_v;

    public bool InputEnabled { get; set; } = false;

    public void GetKeyInput()
    {
        if (InputEnabled)
        {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
        else
        {
            m_h = 0;
            m_v = 0;
        }
    }
}
