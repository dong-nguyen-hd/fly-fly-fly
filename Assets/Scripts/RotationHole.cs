using UnityEngine;

public class RotationHole : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * -90 * Time.deltaTime);
    }
    
}
