using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Image FirstBaseUI;
    [SerializeField] Image SecondBaseUI;
    [SerializeField] Image ThirdBaseUI;
    [SerializeField] Image OneOutUI;
    [SerializeField] Image TwoOutUI;
    [SerializeField] TMP_Text BallCountUI;
    [SerializeField] TMP_Text InningUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreBoard();

        
    }

    void UpdateScoreBoard()
    {
        if (GameManager.Instance.m_isRunner[0])
        {
            FirstBaseUI.color = Color.yellow;
        }
        else
        {
            FirstBaseUI.color = Color.white;
        }

        if (GameManager.Instance.m_isRunner[1])
        {
            SecondBaseUI.color = Color.yellow;
        }
        else
        {
            SecondBaseUI.color = Color.white;
        }

        if (GameManager.Instance.m_isRunner[2])
        {
            ThirdBaseUI.color = Color.yellow;
        }
        else
        {
            ThirdBaseUI.color = Color.white;
        }

        switch (GameManager.Instance.m_nOutCnt)
        {
            case 0:
                OneOutUI.color = Color.white;
                TwoOutUI.color = Color.white;
                break;
            case 1:
                OneOutUI.color = Color.red;
                TwoOutUI.color = Color.white;
                break;
            case 2:
                OneOutUI.color = Color.red;
                TwoOutUI.color = Color.red;
                break;
        }

        BallCountUI.text = string.Format("{0} - {1}", GameManager.Instance.m_nBallCnt, GameManager.Instance.m_nStrikeCnt);

        string m_strInningDetail = "TOP";
        if (GameManager.Instance.m_nInningData % 2 == 1)
        {
            m_strInningDetail = "BOT";
        }
        else
        {
            m_strInningDetail = "TOP";
        }



        InningUI.text = string.Format("{0} {1}",  m_strInningDetail, GameManager.Instance.m_nInningData / 2 + 1);

    }
}
