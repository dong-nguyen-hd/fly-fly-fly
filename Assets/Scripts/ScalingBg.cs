using UnityEngine;

public class ScalingBg : MonoBehaviour
{
    void Start()
    {
        float HeightCamera = Camera.main.orthographicSize * 2f; //get width value of camera

        float WidthCamera = HeightCamera * Screen.width / Screen.height; //get height value of camera

        transform.localScale = new Vector3(WidthCamera, HeightCamera, 0f); //scale picture
    }
    
}
