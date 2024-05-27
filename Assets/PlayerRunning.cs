using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRunning : MonoBehaviour
{
    public static PlayerRunning Instance;
    public bool isRunningState = false;

    public int m_nTargetBase = 1;  // 타격 시작 : 0, 1루 베이스 : 1, 2루 베이스 : 2, 3루 베이스 : 3, 홈베이스 : 4
    public int m_nNowBase = 0;

    Vector3 m_vecFirstBase = new Vector3(-28.5f, 0.0f, 31.5f);                             // 1루 베이스 위치
    Vector3 m_vecSecondBase = new Vector3(0.25f, 0.0f, 5.25f);                          // 2루 베이스 위치
    Vector3 m_vecThirdBase = new Vector3(28.5f, 0.0f, 31.5f);                             // 3루 베이스 위치
    Vector3 m_vecHomeBase = new Vector3(0.25f, 0.0f, 62.35f);                          // 홈 베이스 위치

    Vector3 m_vecBetweenHomeToFirst = new Vector3(-30.0f, 0.0f, 40.0f);         // 홈과 1루 사이(1루 밟기 전에 2루로 향할 때 지나갈 위치)
    Vector3 m_vecBetweenFirstToSecond = new Vector3(-25.0f, 0.0f, 19.5f);       // 1루와 2루 사이(m_vecBetweenHomeToFirst를 밟고 2루로 향할 때 밟을 위치)
    Vector3 m_vecBetweenSecondToThird = new Vector3(12.5f, 0.0f, 5.0f);         // 2루를 지나 3루로 향할 때 지나갈 위치(2루에서 출발이 아닌 그 이전에 출발 시)
    Vector3 m_vecBetweenThirdToHome = new Vector3(28.0f, 0.0f, 42.5f);          // 3루 이전에 출발 시 3루를 지나 홈으로 향할 때 지나갈 위치

    Vector3 m_vecOverRunFirstBase = new Vector3(-45.0f, 0.0f, 22.0f);


    Vector3[] m_vecTargetBaseList = new Vector3[4];

    bool m_isCheckFirstBase = false;


    // 0일 때 : 타격 후 1루로 갈 때, 1일 때 : 1루 밟고 2루로 갈 때, 2일 때 : 2루 밟고 3루로 갈 때, 3일 때 : 3루 밟고 홈으로 갈 때 => enum으로 구분하는게 쉽겠지?
    int m_nNowTargetNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunningState)
        {
            InputBaseRun();
            CheckNowBase();
            BaseRunning();
        } 
    }

    void CheckNowBase()
    {
        if(Vector3.Distance(transform.position, m_vecFirstBase) <= 1.0f)
        {
            m_nNowBase = 1;
        }
        if(Vector3.Distance(transform.position, m_vecSecondBase) <= 1.0f)
        {
            m_nNowBase = 2;
        }
        if (Vector3.Distance(transform.position,m_vecThirdBase) <= 1.0f)
        {
            m_nNowBase = 3;
        }
    }

   void BaseRunning()
    {
        // 현재 타겟을 결정하는 로직
        Vector3 currentTarget = Vector3.zero;

        switch (m_nNowTargetNum)
        {
            case 0:
                currentTarget = m_vecTargetBaseList[0];
                break;
            case 1:
                currentTarget = m_vecTargetBaseList[1];
                break;
            case 2:
                currentTarget = m_vecTargetBaseList[2];
                break;
            case 3:
                currentTarget = m_vecTargetBaseList[3];
                break;
        }
        Debug.Log(m_vecTargetBaseList[0] + " / " + m_vecTargetBaseList[1] + " / " + m_vecTargetBaseList[2] + " / " + m_vecTargetBaseList[3]);

        // 플레이어를 현재 타겟 위치로 이동
        if (currentTarget != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * /*PlayerStat.Instance.m_fSpeedRun*/ 40f * 0.25f);
        }

        // 현재 타겟에 도달했는지 확인
        if (Vector3.Distance(transform.position, currentTarget) < 1.0f)
        {
            m_nNowTargetNum++;
        }
    }

    void InputBaseRun()
    {
        bool m_isNextBase = Input.GetKeyDown(KeyCode.W);
        bool m_isPrevBase = Input.GetKeyDown(KeyCode.E);
        Debug.Log("현재 베이스 : " + m_nNowBase + "루 베이스");
        Debug.Log("목표 베이스 : " + m_nTargetBase + "루 베이스");

        // 이 부분 때문에 지금 1-2루 사이에서 w를 눌러도 3루로 가지 않음 -> 이 부분 반드시 수정
        if(m_isNextBase && (m_nTargetBase - m_nNowBase) <= 2)
        {
            m_nTargetBase++;
            if (m_nTargetBase == 4)
            {
                m_nTargetBase = 0;
            }
            
            switch (m_nTargetBase)
            {
                case 0:
                    RunHomeBase(); break;
                case 1:
                    RunFirstBase(); break;
                case 2:
                    RunSecondBase(); break;
                case 3:
                    RunThirdBase(); break;
            }

            m_nNowTargetNum = 0;
        }
        else if(m_isPrevBase && !(m_nNowBase == 0))
        {
            m_nTargetBase--;
            if (m_nTargetBase < 0)
            {
                m_nTargetBase = 3;
            }

            switch (m_nTargetBase)
            {
                case 0:
                    RunHomeBase(); break;
                case 1:
                    RunFirstBase(); break;
                case 2:
                    RunSecondBase(); break;
                case 3:
                    RunThirdBase(); break;
            }

            m_nNowTargetNum = 0;
        }

        
    }

    public void RunFirstBase()
    {
        // 타격 직후 1루로 달릴 때
        if(m_nNowBase == 0)
        {
            m_vecTargetBaseList[0] = m_vecFirstBase;
            m_vecTargetBaseList[1] = m_vecOverRunFirstBase;
            m_vecTargetBaseList[2] = m_vecFirstBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 1루이상에서 1루로 돌아갈 때
        else
        {
            if(m_nNowBase == 2)
            {
                m_vecTargetBaseList[0] = m_vecSecondBase;
                m_vecTargetBaseList[1] = m_vecFirstBase;
                m_vecTargetBaseList[2] = Vector3.zero;
                m_vecTargetBaseList[3] = Vector3.zero;
            }
            else if(m_nNowBase == 1)
            {
                m_vecTargetBaseList[0] = m_vecFirstBase;
                m_vecTargetBaseList[1] = Vector3.zero;
                m_vecTargetBaseList[2] = Vector3.zero;
                m_vecTargetBaseList[3] = Vector3.zero;
            }
        }
    }

    void RunSecondBase()
    {
        // 타격 직후 1루 밟기 전 2루 입력 되었을 때
        if(m_nNowBase == 0)
        {
           
            if(Vector3.Distance(transform.position, m_vecFirstBase) >= 9.0f && !m_isCheckFirstBase)
            {
                m_vecTargetBaseList[0] = m_vecBetweenHomeToFirst;
                m_vecTargetBaseList[1] = m_vecFirstBase;
                m_vecTargetBaseList[2] = m_vecBetweenFirstToSecond;
                m_vecTargetBaseList[3] = m_vecSecondBase;
            }
            else
            {
                m_isCheckFirstBase = true;
                m_vecTargetBaseList[0] = m_vecFirstBase;
                m_vecTargetBaseList[1] = m_vecBetweenFirstToSecond;
                m_vecTargetBaseList[2] = m_vecSecondBase;
                m_vecTargetBaseList[3] = Vector3.zero;
            }
        }
        // 1루를 밟고 나서 2루 입력 되었을 때
        else if(m_nNowBase == 1)
        {
            m_vecTargetBaseList[0] = m_vecSecondBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        } 
        // 2-3루 사이에서 2루로 돌아갈 때
        else
        {
            m_vecTargetBaseList[0] = m_vecSecondBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
    }

    void RunThirdBase()
    {
        // 1루 밟고 2루를 아직 밟기 전
        if(m_nNowBase == 1)
        {
            m_vecTargetBaseList[0] = m_vecSecondBase;
            m_vecTargetBaseList[1] = m_vecBetweenSecondToThird;
            m_vecTargetBaseList[2] = m_vecThirdBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 2루 밟고 3루를 향할 때
        else if(m_nNowBase == 2)
        {
            m_vecTargetBaseList[0] = m_vecThirdBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 홈으로 가다가 돌아올 때
        else
        {
            m_vecTargetBaseList[0] = m_vecThirdBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
    }

    void RunHomeBase()
    {
        // 2루에서 홈으로 갈 때
        if(m_nNowBase == 2)
        {
            m_vecTargetBaseList[0] = m_vecThirdBase;
            m_vecTargetBaseList[1] = m_vecBetweenThirdToHome;
            m_vecTargetBaseList[2] = m_vecHomeBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 3루에서 홈으로 갈 때
        else
        {
            m_vecTargetBaseList[0] = m_vecHomeBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
    }
}
