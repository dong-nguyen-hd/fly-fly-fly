using System.Collections.Generic;
using UnityEngine;

public class SkyMgr : MonoBehaviour
{
    #region Singleton class: SkyMgr
    public static SkyMgr Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    [HideInInspector]
    public List<GameObject> cloudList;
    [HideInInspector]
    public bool allowRnd;//allow cloud random
    private GameObject tempFigure;
    private Vector3 tempVector3 = Vector3.zero;


    [HideInInspector]
    public bool checkGenerate;
    private List<Vector3> posVectorOfCloud = new List<Vector3>(6);
    private float step;
    public float speed = 1f;//speed of Move
    private List<float> rndList = new List<float>(6);
    private List<int> rndListInt = new List<int>(6);

    //condition of Time
    private float timeOfGame = 0;
    public float limitedTime = 2f;
    public float limitedTimeOfTurn;

    private Vector3 oldPosFigureMove;

    void Start()
    {
        cloudList.AddRange(GameObject.FindGameObjectsWithTag("Cloud"));
        checkGenerate = true;
        allowRnd = false;

        for (int i = 0; i < cloudList.Count; i++)
        {
            float rnd = Random.Range(-4f, 3.5f);
            if (i == 0)
            {
                cloudList[i].transform.position = new Vector3(cloudList[i].transform.position.x, -4f, 0);
            }
            else
            {
                if (i == 1)
                {
                    FigureMgr.Instance.transform.position = tempVector3Method(FigureMgr.Instance.transform.position.x,
                        rnd + 0.6f, 0);
                    cloudList[i].transform.position = tempVector3Method(cloudList[i].transform.position.x, rnd, 0);
                    continue;
                }
                cloudList[i].transform.position = tempVector3Method(cloudList[i].transform.position.x, rnd, 0);
            }
        }

        step = speed * Time.deltaTime;
    }

    void Update()
    {
        //FigureMgr.Instance.ActiveAndNegative(true);
        if (allowRnd) timeOfGame += Time.deltaTime;

        if (allowRnd && timeOfGame >= limitedTime)
        {
            GenerateVector(DefinePosition(checkGenerate));

            if (!checkGenerate && CheckPosCloud(cloudList[0])
                && CheckPosCloud(cloudList[1]) && CheckPosCloud(cloudList[2])
                && CheckPosCloud(cloudList[3]) && CheckPosCloud(cloudList[4])
                && CheckPosCloud(cloudList[5]))
            {
                //timeOfGame = 0;
                //checkGenerate = true;
                if (FigureMgr.Instance.isLive) GameManager.Instance.isYourTurn.enabled = true;
                
                if (timeOfGame >= limitedTimeOfTurn)
                {
                    GameManager.Instance.isYourTurn.enabled = false;
                    timeOfGame = 0;
                    checkGenerate = true;
                    FigureMgr.Instance.isTurn = true;
                }

            }
        }

    }
    private bool CheckPosCloud(GameObject cloud)
    {
        float currentPos = cloud.transform.position.x;
        if (currentPos == -6f || currentPos == -3f ||
            currentPos == 0 || currentPos == 3f ||
            currentPos == 6f || currentPos == 9f)
        {
            return true;
        }
        return false;
    }

    private Vector3 tempVector3Method(float x, float y, float z)
    {
        tempVector3.x = x;
        tempVector3.y = y;
        tempVector3.z = z;
        return tempVector3;
    }

    private bool DefinePosition(bool temp)
    {
        if (temp)
        {
            //just for debug
            //Debug.Log("OG.x: "+System.Math.Round(FigureMgr.Instance.transform.position.x, 3));
            //Debug.Log(FigureMgr.Instance.nameCloudAbove);

            oldPosFigureMove = FigureMgr.Instance.transform.position;
            oldPosFigureMove.x = (float)System.Math.Round(oldPosFigureMove.x, 1);
            oldPosFigureMove.y = (float)System.Math.Round(oldPosFigureMove.y, 1);

            //remove 1 cloud per turn
            tempFigure = cloudList[0];
            tempFigure.transform.position = tempVector3Method(cloudList[5].transform.position.x + 3, Random.Range(-4f, 3.5f), 0);
            //isLive is cloud one?
            if (cloudList[0].name == FigureMgr.Instance.nameCloudAbove) Figure.Instance.ActiveAndNegative(false);
            cloudList.RemoveAt(0);
            cloudList.Add(tempFigure);//remove 1 cloud per turn

            for (int i = 0; i < cloudList.Count; i++)
            {
                cloudList[i].GetComponent<PolygonCollider2D>().isTrigger = false;
                //rndList.Add(Random.Range(-4f, 3.5f));
                rndListInt.Add(Random.Range(-4, 3));
                posVectorOfCloud.Add(cloudList[i].transform.position);
                if (Random.Range(2, 99) % 2 == 0)
                {
                    rndList.Add((float)rndListInt[i]);
                }
                else
                {
                    rndList.Add((float)rndListInt[i] + 0.5f);
                }
            }
            checkGenerate = false;
        }
        return true;

    }
    private void GenerateVector(bool temp)
    {
        if (temp)
        {
            for (int i = 0; i < cloudList.Count; i++)
            {
                if (i == 0)
                {
                    if (cloudList[i].name == FigureMgr.Instance.nameCloudAbove)
                    {
                        FigureMgr.Instance.transform.position = Vector3.MoveTowards(FigureMgr.Instance.transform.position,
                            tempVector3Method(oldPosFigureMove.x - 3f, -4f + 0.4f, 0), step);

                        cloudList[i].transform.position = Vector3.MoveTowards(cloudList[i].transform.position,
                        tempVector3Method(posVectorOfCloud[i].x - 3f, -4f, 0), step);
                    }
                    else
                    {
                        cloudList[i].transform.position = Vector3.MoveTowards(cloudList[i].transform.position,
                        tempVector3Method(posVectorOfCloud[i].x - 3f, -4f, 0), step);
                    }
                }
                else
                {
                    if (cloudList[i].name == FigureMgr.Instance.nameCloudAbove)
                    {
                        //set position of figure
                        FigureMgr.Instance.transform.position = Vector3.MoveTowards(FigureMgr.Instance.transform.position,
                            tempVector3Method(oldPosFigureMove.x - 3f, posVectorOfCloud[i].y + 0.4f, 0), step);

                        cloudList[i].transform.position = Vector3.MoveTowards(cloudList[i].transform.position,
                            tempVector3Method(posVectorOfCloud[i].x - 3f, posVectorOfCloud[i].y, 0), step);
                    }
                    else
                    {
                        cloudList[i].transform.position = Vector3.MoveTowards(cloudList[i].transform.position,
                        tempVector3Method(posVectorOfCloud[i].x - 3f, rndList[i], 0), step);
                    }
                }
            }
        }
    }
}
