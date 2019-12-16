using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotateSpeed = 20f;

    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "y", 360,
            "looptype", iTween.LoopType.loop,
            "speed", rotateSpeed,
            "easetype", iTween.EaseType.linear
            ));
    }
}
