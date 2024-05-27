using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int m_nOutCnt = 0;
    public int m_nStrikeCnt = 0;
    public int m_nBallCnt = 3;
    public int m_nInningData = 1;
    public bool[] m_isRunner = { false, false, false };
    public bool m_isChangeOverCount = false;
    public BatterAttributes m_BANowAttribute = null;
    public BatterStat m_BSNowStat = null;



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        BallCountCal();
    }

    void BallCountCal()
    {
        if (m_nBallCnt >= 4)
        {
            m_nBallCnt = 0;
            m_nStrikeCnt = 0;
            m_isChangeOverCount = true;
        }
        if(m_nStrikeCnt >= 3)
        {
            m_nBallCnt = 0;
            m_nStrikeCnt = 0;
            m_nOutCnt++;
            m_isChangeOverCount = true;
        }
        if(m_nOutCnt >= 3)
        {
            m_nBallCnt = 0;
            m_nStrikeCnt = 0;
            m_nOutCnt = 0;
            m_nInningData++;
            m_isChangeOverCount = true;
        }
    }

    public void ChangeSaveData(BatterAttributes changeAttirbute, BatterStat changeStat)
    {
        m_BANowAttribute = changeAttirbute;
        m_BSNowStat = changeStat;
    }
}
