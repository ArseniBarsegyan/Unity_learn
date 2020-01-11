using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndScreen : MonoBehaviour
{
    public PostProcessProfile blurProfile;
    public PostProcessProfile normalProfile;
    public PostProcessVolume postProcessVolume;

    public void EnableCameraBlur(bool state)
    {
        if (blurProfile != null && normalProfile != null && postProcessVolume != null)
        {
            postProcessVolume.profile = (state) ? blurProfile : normalProfile;
        }
    }
}
