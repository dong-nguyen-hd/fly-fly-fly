using UnityEngine;
using System.Collections.Generic;

public class Plane : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public PolygonCollider2D col;
    [HideInInspector]
    public Vector3 pos { get { return transform.position; } }

    [HideInInspector]
    public GameObject myPlane;
   
    private bool tempStayOnCloud;//checking Figure stay on cloud + pointing
    private float checkOffset;//checking bug point 1 cloud
    private Vector2 tempForce;//force value assign Push method
    private int tempPoint;//point/1 turn

    //to calculate raycast
    public LayerMask whatLayer;
    private bool checkPush;
    private RaycastHit2D hit;
    private List<string> checkNameList;
    //condition of Time ~ 0.5s
    private float timeOfGame { get; set;}
    public float limitedTime = 0.05f;

    void Start()
    {
        myPlane = GameObject.FindGameObjectWithTag("Plane");
        Hide();//Hiden plane while start game
        
    }

    void Update()
    {
        if (checkPush)
        {
            hit = Physics2D.Raycast(myPlane.transform.position, tempForce, 5f, whatLayer);
            if (hit.collider != null)
            {
                checkNameList.Add(hit.collider.name);
            }
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        col = GetComponent<PolygonCollider2D>();
        tempStayOnCloud = false;
        checkPush = false;
        tempPoint = 0;
        checkOffset = -9876;
        checkNameList = new List<string>();
    }

    public void Push(Vector2 force)
    {
        timeOfGame = 0;
        tempForce = force;
        checkPush = true;
        Show();
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DesactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    private int CalculatePoint()
    {
        List<string> temp = new List<string>();
        foreach (var item in checkNameList)
        {
            if (!temp.Contains(item))
            {
                temp.Add(item);
            }
        }
        checkNameList.Clear();
        return temp.Count-1;
    }

    public void Hide()
    {
        myPlane.SetActive(false);
    }

    public void Show()
    {
        myPlane.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //just for debug
        //Debug.Log("Tao con song" + FigureMgr.Instance.isLive);
        if (other.tag == "Box")
        {
            Hide();
            myPlane.transform.position = FigureMgr.Instance.transform.position;
            tempPoint = 0;
            checkPush = false;
            FigureMgr.Instance.isTurn = false;
            
            //just for debug
            //Debug.Log(FigureMgr.Instance.newPosFigure);
        }
        if (other.tag == "DarkHole")
        {
            FigureMgr.Instance.isLive = false;
            FigureMgr.Instance.isTurn = false;
            checkPush = false;
            Hide();
            //just for debug
            //Debug.Log("Tao da chet" + FigureMgr.Instance.isLive);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "BoxCloud" && FigureMgr.Instance.isTurn)
        {
            if (!tempStayOnCloud)
            {
                checkOffset = other.GetComponent<BoxCollider2D>().offset.x;
                tempStayOnCloud = true;
            }
            if (checkOffset == other.GetComponent<BoxCollider2D>().offset.x)
            {
                tempPoint = 0;
            }
            
            //just for debug
            //Debug.Log(tempPoint);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //just for debug
        //Debug.Log("TempPoint: " + tempPoint);
        if (FigureMgr.Instance.isTurn && FigureMgr.Instance.isLive && other.gameObject.tag == "Cloud")
        {
            timeOfGame += Time.deltaTime;
            if (timeOfGame >= limitedTime)
            {
                tempPoint = CalculatePoint();
                FigureMgr.Instance.nameCloudAbove = other.gameObject.name;
                FigureMgr.Instance.newPosFigure = myPlane.transform.position;
                FigureMgr.Instance.oldPosFigure = FigureMgr.Instance.transform.position;
                FigureMgr.Instance.SwapPosFigure();
                if (tempPoint > 0) FigureMgr.Instance.myPoint += tempPoint;
                tempPoint = 0;
                tempStayOnCloud = false;
                checkPush = false;
                FigureMgr.Instance.swapPos = true;
                Hide();
                FigureMgr.Instance.isTurn = false;

                //just for debug
                //Debug.Log(oldPosCloud);
                //Debug.Log(stayPosCloud);
                //Debug.Log("point: " + FigureMgr.Instance.myPoint);
                //Debug.Log("dau tren:" + FigureMgr.Instance.nameCloudAbove);
            }
        }
    }
}
