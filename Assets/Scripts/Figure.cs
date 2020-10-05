using UnityEngine;

public class Figure : MonoBehaviour
{
    #region Singleton class: Figure
    public static Figure Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    //condition of Time
    private float timeOfGame = 0;
    public float limitedTime = 0.2f;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void ActiveAndNegative(bool isKinematic)
    {
        if (isKinematic)
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            this.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
        else
        {
            this.GetComponent<PolygonCollider2D>().isTrigger = false;
            this.GetComponent<Rigidbody2D>().isKinematic = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            Hide();
            this.transform.position = FigureMgr.Instance.transform.position;
            FigureMgr.Instance.isLive = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DarkHole")
        {
            FigureMgr.Instance.isLive = false;
            this.gameObject.SetActive(false);
            this.transform.position = Vector3.zero;
            //just for debug
            //Debug.Log("die");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Cloud")
        {
            timeOfGame += Time.deltaTime;
            if (timeOfGame >= limitedTime)
            {
                FigureMgr.Instance.nameCloudAbove = other.name;
                timeOfGame = 0;
            }
        }
    }
}
