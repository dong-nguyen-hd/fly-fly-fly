using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton class: GameManager
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public Text textPoint;
    public Text textPointEnd;
    public GameObject panelGameOver;
    public Image isYourTurn;

    void Start()
    {
        isYourTurn.enabled = false;
        panelGameOver.SetActive(false);
        FigureMgr.Instance.isLive = true;
        FigureMgr.Instance.isTurn = true;
    }
    void Update()
    {
        if (FigureMgr.Instance.isLive && !FigureMgr.Instance.isTurn)
        {
            panelGameOver.SetActive(false);
            textPoint.text = string.Format("Score: " + FigureMgr.Instance.myPoint);
            SkyMgr.Instance.allowRnd = true;
            //just for debug
            //FigureMgr.Instance.isTurn = true;//just for debug
        }
        else
        {
            SkyMgr.Instance.allowRnd = false;
        }

        if (!FigureMgr.Instance.isLive)
        {
            var temp = FigureMgr.Instance.myPoint;
            panelGameOver.SetActive(true);
            textPointEnd.text = string.Format(temp.ToString());
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    
}
