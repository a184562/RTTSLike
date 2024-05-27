using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRunning : MonoBehaviour
{
    public static PlayerRunning Instance;
    public bool isRunningState = false;

    public int m_nTargetBase = 1;  // Ÿ�� ���� : 0, 1�� ���̽� : 1, 2�� ���̽� : 2, 3�� ���̽� : 3, Ȩ���̽� : 4
    public int m_nNowBase = 0;

    Vector3 m_vecFirstBase = new Vector3(-28.5f, 0.0f, 31.5f);                             // 1�� ���̽� ��ġ
    Vector3 m_vecSecondBase = new Vector3(0.25f, 0.0f, 5.25f);                          // 2�� ���̽� ��ġ
    Vector3 m_vecThirdBase = new Vector3(28.5f, 0.0f, 31.5f);                             // 3�� ���̽� ��ġ
    Vector3 m_vecHomeBase = new Vector3(0.25f, 0.0f, 62.35f);                          // Ȩ ���̽� ��ġ

    Vector3 m_vecBetweenHomeToFirst = new Vector3(-30.0f, 0.0f, 40.0f);         // Ȩ�� 1�� ����(1�� ��� ���� 2��� ���� �� ������ ��ġ)
    Vector3 m_vecBetweenFirstToSecond = new Vector3(-25.0f, 0.0f, 19.5f);       // 1��� 2�� ����(m_vecBetweenHomeToFirst�� ��� 2��� ���� �� ���� ��ġ)
    Vector3 m_vecBetweenSecondToThird = new Vector3(12.5f, 0.0f, 5.0f);         // 2�縦 ���� 3��� ���� �� ������ ��ġ(2�翡�� ����� �ƴ� �� ������ ��� ��)
    Vector3 m_vecBetweenThirdToHome = new Vector3(28.0f, 0.0f, 42.5f);          // 3�� ������ ��� �� 3�縦 ���� Ȩ���� ���� �� ������ ��ġ

    Vector3 m_vecOverRunFirstBase = new Vector3(-45.0f, 0.0f, 22.0f);


    Vector3[] m_vecTargetBaseList = new Vector3[4];

    bool m_isCheckFirstBase = false;


    // 0�� �� : Ÿ�� �� 1��� �� ��, 1�� �� : 1�� ��� 2��� �� ��, 2�� �� : 2�� ��� 3��� �� ��, 3�� �� : 3�� ��� Ȩ���� �� �� => enum���� �����ϴ°� ������?
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
        // ���� Ÿ���� �����ϴ� ����
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

        // �÷��̾ ���� Ÿ�� ��ġ�� �̵�
        if (currentTarget != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * /*PlayerStat.Instance.m_fSpeedRun*/ 40f * 0.25f);
        }

        // ���� Ÿ�ٿ� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, currentTarget) < 1.0f)
        {
            m_nNowTargetNum++;
        }
    }

    void InputBaseRun()
    {
        bool m_isNextBase = Input.GetKeyDown(KeyCode.W);
        bool m_isPrevBase = Input.GetKeyDown(KeyCode.E);
        Debug.Log("���� ���̽� : " + m_nNowBase + "�� ���̽�");
        Debug.Log("��ǥ ���̽� : " + m_nTargetBase + "�� ���̽�");

        // �� �κ� ������ ���� 1-2�� ���̿��� w�� ������ 3��� ���� ���� -> �� �κ� �ݵ�� ����
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
        // Ÿ�� ���� 1��� �޸� ��
        if(m_nNowBase == 0)
        {
            m_vecTargetBaseList[0] = m_vecFirstBase;
            m_vecTargetBaseList[1] = m_vecOverRunFirstBase;
            m_vecTargetBaseList[2] = m_vecFirstBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 1���̻󿡼� 1��� ���ư� ��
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
        // Ÿ�� ���� 1�� ��� �� 2�� �Է� �Ǿ��� ��
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
        // 1�縦 ��� ���� 2�� �Է� �Ǿ��� ��
        else if(m_nNowBase == 1)
        {
            m_vecTargetBaseList[0] = m_vecSecondBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        } 
        // 2-3�� ���̿��� 2��� ���ư� ��
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
        // 1�� ��� 2�縦 ���� ��� ��
        if(m_nNowBase == 1)
        {
            m_vecTargetBaseList[0] = m_vecSecondBase;
            m_vecTargetBaseList[1] = m_vecBetweenSecondToThird;
            m_vecTargetBaseList[2] = m_vecThirdBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 2�� ��� 3�縦 ���� ��
        else if(m_nNowBase == 2)
        {
            m_vecTargetBaseList[0] = m_vecThirdBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // Ȩ���� ���ٰ� ���ƿ� ��
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
        // 2�翡�� Ȩ���� �� ��
        if(m_nNowBase == 2)
        {
            m_vecTargetBaseList[0] = m_vecThirdBase;
            m_vecTargetBaseList[1] = m_vecBetweenThirdToHome;
            m_vecTargetBaseList[2] = m_vecHomeBase;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
        // 3�翡�� Ȩ���� �� ��
        else
        {
            m_vecTargetBaseList[0] = m_vecHomeBase;
            m_vecTargetBaseList[1] = Vector3.zero;
            m_vecTargetBaseList[2] = Vector3.zero;
            m_vecTargetBaseList[3] = Vector3.zero;
        }
    }
}
