using UnityEngine;

public class FigureMgr : MonoBehaviour
{
    #region Singleton class: FigureMgr

    public static FigureMgr Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    Camera cam;

    public Plane plane;
    public Trajectory trajectory;
    [SerializeField]
    float pushForce = 4f;

    [HideInInspector]
    public bool isLive;//Live or die?
    [HideInInspector]
    public bool isTurn;//Your turn
    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;

    [HideInInspector]
    public string nameCloudAbove;
    [HideInInspector]
    public int myPoint = 0;
    [HideInInspector]
    public Vector3 oldPosFigure;
    [HideInInspector]
    public Vector3 newPosFigure;
    [HideInInspector]
    public bool swapPos;

    //just for debug
    //[HideInInspector]
    //public GameObject myFigure;
    //[HideInInspector]
    //public RaycastHit hit;

    void Start()
    {
        cam = Camera.main;
        plane.DesactivateRb();

        //just for debug
        //myFigure = GameObject.FindGameObjectWithTag("Figure");
    }

    void Update()
    {
        if (isTurn)
        {
            for (int i = 0; i < SkyMgr.Instance.cloudList.Count; i++)
            {
                if (nameCloudAbove == SkyMgr.Instance.cloudList[i].name)
                {
                    SkyMgr.Instance.cloudList[i].GetComponent<PolygonCollider2D>().isTrigger = true;
                }
            }
            swapPos = false;//check swap position
            if (Input.GetMouseButtonDown(0))
            {
                //ActiveAndNegative(true);//set trigger or not
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnDragEnd();
            }

            if (isDragging)
            {
                OnDrag();
            }
        }
        
        //just for debug
        //if (Physics.Raycast(plane.transform.position, force, out hit))
        //{
        //    Debug.DrawLine(plane.transform.position, force, Color.red);
        //}
    }
    
    #region drag-drop func
    void OnDragStart()
    {
        plane.DesactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();
    }
    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        if (distance >= 3.7f) distance = 3.7f;
        force = direction * distance * pushForce;
        trajectory.UpdateDots(plane.pos, force);
        //just for debug
        //Debug.DrawLine(startPoint, endPoint);
        //Debug.Log("Distance: " + distance);
        
    }
    void OnDragEnd()
    {
        //push the plane
        plane.ActivateRb();
        plane.Push(force);
        trajectory.Hide();

        oldPosFigure = newPosFigure = transform.position;
        //just for debug
        //Debug.Log("Name cloud: "+ nameCloudAbove);
    }
    #endregion

    #region Check swap position
    public void SwapPosFigure()
    {
        if (!swapPos && isLive)
        {
            transform.position = newPosFigure;

            //just for debug
            //Debug.Log($"oldPos: " + newPosFigure);
            //Debug.Log($"newPos: " + oldPosFigure);
            
        }
    }

    //just for debug
    //set Figure gravity or not?
    //public void ActiveAndNegative(bool isKinematic)
    //{
    //    if (isKinematic)
    //    {
    //        myFigure.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    //        myFigure.GetComponent<Rigidbody2D>().isKinematic = true;
    //        myFigure.GetComponent<PolygonCollider2D>().isTrigger = true;
    //    }
    //    else
    //    {
    //        myFigure.GetComponent<PolygonCollider2D>().isTrigger = false;
    //        myFigure.GetComponent<Rigidbody2D>().isKinematic = false;
    //        myFigure.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    //    }
    //}

    #endregion
}

