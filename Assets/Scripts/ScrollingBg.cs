using UnityEngine;

public class ScrollingBg : MonoBehaviour
{
    public float speed = 0.1f;//speed to scrolling
    private Material mat;    
    private Vector2 offset = Vector2.zero;
    
    void Start()
    {
        
        mat = GetComponent<Renderer>().material;//define material
        offset = mat.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        offset.x += speed * Time.deltaTime*-1;//x to sideway slide, y for vertical slide
        mat.SetTextureOffset("_MainTex", offset);
    }
}
