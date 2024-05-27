using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher_AI : MonoBehaviour
{
    public static Pitcher_AI Instance;

    public enum PitcherHand
    {
        Right, Left,
    }

    public PitcherHand hand;

    public bool m_isStrike = false;

    // 애니메이션
    Animator m_anim = null;

    // 공을 날릴 타겟(공이 도착해야할 곳)
    Vector3 m_vecTarget = Vector3.zero;
    float m_fTargetZ = 65.0f;

    // 던질 공
    [SerializeField]
    GameObject m_objBall = null;
    [SerializeField]
    GameObject m_objSpawnPos = null;

    // 던질 공이 나타날 위치(임시값)
    Transform m_posSpawnPoint = null;

    // 날아갈 공의 스크립트
    PitchBall m_PB = null;

    public GameObject ball = null;
    
    void Start()
    {
        Instance = this;

        m_anim = GetComponent<Animator>();

        // 임시버전 공이 날아갈 위치(0.3, 2, 62)
        m_vecTarget = new Vector3(0.3f, 2.0f, 65.0f);
        m_posSpawnPoint = m_objSpawnPos.transform;

        ball = Instantiate(m_objBall, m_posSpawnPoint.position, Quaternion.identity);
        ball.transform.SetParent(m_objSpawnPos.transform);

        m_PB = ball.GetComponent<PitchBall>();
    }

    // Update is called once per frame
    void Update()
    {
        PitchingBall();
    }

    void PitchingBall()
    {
        // 의도적으로 공을 던지게 하기 위한 임시 변수
        bool m_isPitching = Input.GetKeyDown(KeyCode.Q);

        if (m_isPitching)
        {
            SetPitchVector();
            
            m_anim.SetTrigger("Pitching");

            StartCoroutine(DelayedPitch(2.0f, m_vecTarget));
        }
        else
        {
        }

    }

    public void ResetBall()
    {
        if (ball == null)
        {
            ball = Instantiate(m_objBall, m_posSpawnPoint.position, Quaternion.identity);
            ball.transform.SetParent(m_objSpawnPos.transform);


            m_PB = ball.GetComponent<PitchBall>();
            Debug.Log("볼 리셋");
        }
        
    }

    void SetPitchVector()
    {
        float m_fStrikeRate = Random.Range(0.0f, 1.0f);
        float m_fRandPitchX = 0.0f;
        float m_fRandPitchY = 0.0f;

        // 스트라이크/볼 확률은 투수의 제구력 변수를 기반으로 바뀌도록 추후 수정
        if (m_fStrikeRate <= 0.65f)
        {
            m_isStrike= true;
        }
        else if(m_fStrikeRate <= 0.95f)
        {
            m_isStrike= false;
        }
        else
        {
            m_isStrike = false;
            m_fRandPitchX = 2.5f;
            m_fRandPitchY = 2.5f;
        }

        if (m_isStrike && m_fStrikeRate <= 0.95f)
        {
            m_fRandPitchX = Random.Range(-0.25f, 0.75f);
            m_fRandPitchY = Random.Range(0.9f, 2.1f);
        }
        else if(m_fStrikeRate <= 0.95f)
        {
            m_fRandPitchX = Random.Range(-0.75f, 1.3f);
            m_fRandPitchY = Random.Range(0.0f, 3.0f);

            while ((m_fRandPitchX >= -0.3f && m_fRandPitchX <= 1.0f) || (m_fRandPitchY <= 2.5f && m_fRandPitchY >= 0.89f))
            {
                m_fRandPitchX = Random.Range(-0.75f, 1.3f);
                m_fRandPitchY = Random.Range(0.0f, 3.0f);
            }
        }
        else
        {
        }

        m_vecTarget = new Vector3(m_fRandPitchX, m_fRandPitchY, m_fTargetZ);
    }


    IEnumerator DelayedPitch(float delay, Vector3 target)
    {
        yield return new WaitForSeconds(delay);

        m_PB.OnPitch(target);
    }
}
