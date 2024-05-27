using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBatting : MonoBehaviour
{
    public static PlayerBatting instance;

    // 애니메이션 제어
    Animator m_anim = null;

    GameObject m_objBallPos = null;
    PitchBall ballTmp = null;

    
    public Camera m_camMissCam = null;
    public Camera m_camMainCam = null;

    public bool m_isSwing = false;
    public bool m_isFailHit = false;


    public bool m_isCount = false;

    public BatterAttributes CharacterAttribute = null;
    public BatterStat CharacterStat = null;

    public GameObject m_objLandingSpotPrefab = null;
    public GameObject m_objLandingSpot = null;

    private void Start()
    {
        instance = this;

        m_anim = GetComponent<Animator>();

        CharacterAttribute = GameManager.Instance.m_BANowAttribute;
        CharacterStat = GameManager.Instance.m_BSNowStat;

    }

    private void Update()
    {
        if (m_objBallPos == null && Pitcher_AI.Instance.ball != null)
        {
            m_objBallPos = Pitcher_AI.Instance.ball;
            ballTmp = m_objBallPos.GetComponent<PitchBall>();
        }
        else
        {
        }
        Batting();
    }

    void Batting()
    {
        bool m_isBatting = Input.GetKeyDown(KeyCode.Space);

        if(m_isBatting && m_objBallPos != null)
        {
            // 현재 누르는 시점의 공 위치 정보를 받고 있는데 나중에 변화구 등 구속이 달라지는 구질의 경우 타이밍이 바뀌어야 하므로 위치를 새로 조정해야함
            m_anim.SetTrigger("Swing");
            Debug.Log("Batting Sucess");

            // 스윙하다가 타이밍 맞으면 타격 성공해서 Hit로 바꿔줌
            float m_fHitTiming = Time.time;
            float m_fNowTiming = (m_fHitTiming - ballTmp.m_fTime) / ballTmp.m_fFlightTime;


            if(PlayManager.instance.Pitcher.hand == Pitcher_AI.PitcherHand.Right)
            {
                Debug.Log(m_fNowTiming);
                Debug.Log( 0.6f - (CharacterAttribute.m_fContactRight * 0.0025f) * 10.0f);
                Debug.Log(0.6f + (CharacterAttribute.m_fContactRight * 0.0025f));
                if (m_fNowTiming >= 0.6f - (CharacterAttribute.m_fContactRight * 0.0025f) &&
                    m_fNowTiming < 0.6f + (CharacterAttribute.m_fContactRight * 0.0025f))
                {
                    m_anim.SetTrigger("Hit");
                    m_isFailHit = false;
                    // 애니메이션이 자연스럽게 만드는 건 나중으로
                    StartCoroutine(DelayedHit(0.75f, m_fNowTiming));
                    Debug.Log("Coroutine Sucess");
                    // ballTmp.BattedBall(SetHitVector(m_fNowTiming));
                    PlayerRunning.Instance.isRunningState = true;
                    PlayerRunning.Instance.RunFirstBase();
                }
                else
                {
                    Debug.Log("헛스윙");
                    if (!GameManager.Instance.m_isChangeOverCount)
                    {
                        GameManager.Instance.m_nStrikeCnt++;
                    }
                    m_isSwing = true;
                    m_isFailHit = true;

                    StartCoroutine(ActiveMissCam());
                }
            }

            
            Debug.Log(m_fNowTiming);

            /*
            PlayerRunning.Instance.isRunningState = true;
            PlayerRunning.Instance.RunFirstBase();
            */
        }
        else if(Vector3.Distance(ballTmp.m_vecPitchTargetPos,ballTmp.transform.position)  < 0.1f)
        {
            if(Pitcher_AI.Instance.m_isStrike && !m_isCount)
            {
                if (!GameManager.Instance.m_isChangeOverCount)
                {
                    GameManager.Instance.m_nStrikeCnt++;
                }
                m_isCount = true;
            }
            else if(!Pitcher_AI.Instance.m_isStrike && !m_isCount)
            {
                if (!GameManager.Instance.m_isChangeOverCount)
                {
                    GameManager.Instance.m_nBallCnt++;
                }
                
                m_isCount = true;
                m_isFailHit = true;
            }

            StartCoroutine(ActiveMissCam());

            GameObject m_objDestroyBall = GameObject.Find("Ball(Clone)");

            if (m_objDestroyBall != null && !m_isBatting && m_isFailHit)
            {
                StartCoroutine(DestroyBall(m_objDestroyBall));
            }
        }
    }

    Vector3 SetHitVector(float m_fHitTiming)
    {
        // float calculateTiming = Mathf.Abs(0.6f - (CharacterAttribute.m_fContactRight * 0.0025f)) * 2.0f;
        float calculateTiming = ((0.6f - m_fHitTiming) / (CharacterAttribute.m_fContactRight * 0.005f)) * (1/ (CharacterAttribute.m_fContactRight * 0.005f));
        float m_fTimningCenter = calculateTiming * 0.5f;

        /*
        float m_fTiming = m_fContactEnd - m_fContactStart;
        float m_fTimningCenter = m_fTiming / 2.0f;
        m_fHitTiming = m_fHitTiming - m_fContactStart;
        */
        // 치는 타이밍에 맞춰서 공이 날아가는 방향을 정하는 작업 이어서 해야 함
        float m_fHitTargetVectorX = 0.0f;

        if (m_fHitTiming >= m_fTimningCenter)
        {
            m_fHitTargetVectorX = calculateTiming * -130.0f;
        }
        else
        {
            m_fHitTargetVectorX = calculateTiming * 130.0f;
        }
        float foulRanNum = Random.Range(0.0f, 1.0f);
        float m_fHitTargetVectorZ;

        Debug.Log("foulRanNum : " + foulRanNum);
        if (foulRanNum < 0.05f)
        {
            m_fHitTargetVectorZ = Random.Range(62.5f, 100.0f);
        }
        else if (foulRanNum < 0.5f) // 공이 내야에서
        {
            ballTmp.m_isInfield = true;
            if (m_fHitTargetVectorX < 45.0f)
            {
                m_fHitTargetVectorZ = Random.Range(-7.0f, 39.0f);
                
            }
            else if (m_fHitTargetVectorX < 22.5f)
            {
                m_fHitTargetVectorZ = Random.Range(-12.0f, 61.0f);
            }
            else if (m_fHitTargetVectorX < 0.0f)
            {
                m_fHitTargetVectorZ = Random.Range(-12.0f, 39.0f);
            }
            else if (m_fHitTargetVectorX < -22.5f)
            {
                m_fHitTargetVectorZ = Random.Range(-12.0f, 61.0f);
            }
            else
            {
                m_fHitTargetVectorZ = Random.Range(-7.0f, 39.0f);
            }

        }
        else // 공이 외야에서
        {
            ballTmp.m_isInfield = false;
            if (foulRanNum < 0.9f)
            {
                if (m_fHitTargetVectorX < 90.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-45f, 15.0f);
                }
                else if (m_fHitTargetVectorX < 45.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-60.0f, -15.0f);
                }
                else if (m_fHitTargetVectorX < 0.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-60.0f, -15.0f);
                }
                else
                {
                    m_fHitTargetVectorZ = Random.Range(-45.0f, 15.0f);
                }
            }
            else // 홈런?
            {
                // m_fHitTargetVectorZ = Random.Range(-25.0f, -85.0f);

                if (m_fHitTargetVectorX < 90.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-70f, -25.0f);
                }
                else if (m_fHitTargetVectorX < 45.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-90.0f, -60.0f);
                }
                else if (m_fHitTargetVectorX < 0.0f)
                {
                    m_fHitTargetVectorZ = Random.Range(-90.0f, -60.0f);
                }
                else
                {
                    m_fHitTargetVectorZ = Random.Range(-70.0f, -25.0f);
                }
            }
        }

        Vector3 m_vecBatTarget;

        if (PlayManager.instance.Pitcher.hand == Pitcher_AI.PitcherHand.Right)
        {
            m_vecBatTarget = new Vector3(m_fHitTargetVectorX, 0.0f, m_fHitTargetVectorZ - (CharacterAttribute.m_fPowerRight * 0.02f));
        }
        else
        {
            m_vecBatTarget = new Vector3(m_fHitTargetVectorX, 0.0f, m_fHitTargetVectorZ - (CharacterAttribute.m_fPowerLeft * 0.02f));
        }

        m_vecBatTarget = new Vector3(-18.0f, 0.0f, -19.0f);



        m_objLandingSpot = Instantiate(m_objLandingSpotPrefab, new Vector3(m_vecBatTarget.x, 0.0f, m_vecBatTarget.z), Quaternion.identity);
        Debug.Log("낙구지점 : " + m_objLandingSpot);

        return m_vecBatTarget;
    }

    IEnumerator DelayedHit(float delay, float hitTiming)
    {
        Debug.Log("Courotine Start");
        yield return new WaitForSeconds(delay);

        Debug.Log("Courotine Good");

        ballTmp.BattedBall(SetHitVector(hitTiming));
        // Debug.Log("hitVector: " + SetHitVector(hitTiming));
        // m_isHit = false;
    }

    IEnumerator ActiveMissCam()
    {
        yield return new WaitForSeconds(1);

        m_camMissCam.gameObject.SetActive(true);
        m_camMainCam.gameObject.SetActive(false);

        yield return new WaitForSeconds(3);

        m_isCount = false;
        m_camMissCam.gameObject.SetActive(false);
        m_camMainCam.gameObject.SetActive(true);
    }

    IEnumerator DestroyBall(GameObject destroyBall)
    {
        yield return new WaitForSeconds(2);

        Destroy(destroyBall);
        Pitcher_AI.Instance.ResetBall();
        
        
    }
}
