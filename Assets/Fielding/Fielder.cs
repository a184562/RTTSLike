using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielder : MonoBehaviour
{
    public int arrayIndex;
    public Vector3 ballLandingPosition;
    public float speed = 5f;
    public float catchRadius = 5.0f;
    public bool isInfield;  // true: 내야수, false: 외야수
    public float flightTime = 5.0f;

    public bool isCatching = false;
    public bool isChasing = false;
    public GameObject m_pbBall = null;
    public PitchBall m_Ball = null;

    public bool m_isCanCatchInorOut;

    private void Start()
    {
        m_Ball = m_pbBall.GetComponent<PitchBall>();
    }


    void Update()
    {
        CheckCanCatch();

        flightTime = PitchBall.instance.m_fJourneyTime;

        if (isCatching && isChasing)
        {
            MoveToBallLandingPosition();
        }
        else if (isCatching)
        {
            ChaseRollingBall();
        }
        else
        {

        }

        CatchTheBall();
    }

    void CheckCanCatch()
    {
        if(!isInfield && Vector3.Distance(ballLandingPosition, transform.position) < catchRadius * flightTime)
        {
            isChasing = true;
        }
    }

    void MoveToBallLandingPosition()
    {
        Debug.Log("MoveToBallLandingPosition 실행");
        float distance = Vector3.Distance(transform.position, ballLandingPosition);
        if (distance <= 0.1f)
        {
            CatchBall();
        }
        else
        {
            Vector3 direction = (ballLandingPosition - transform.position).normalized;
            transform.position = Vector3.Lerp(transform.position, ballLandingPosition, speed * Time.deltaTime / distance);
        }
    }

    void CatchBall()
    {
        isCatching = true;
        Debug.Log("공을 잡았습니다!");
        // 추가 행동
    }

    void ChaseRollingBall()
    {
        Debug.Log("ChaseRollingBall 실행");
        Vector3 rollingBallPosition = GetRollingBallPosition();
        float distance = Vector3.Distance(transform.position, rollingBallPosition);

        if (distance <= catchRadius)
        {
            PickUpBall();
        }
        else
        {
            if(Vector3.Distance(m_pbBall.transform.position, transform.position) >= catchRadius * flightTime)
            {
                Vector3 direction = (ballLandingPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                Vector3 direction = (m_pbBall.transform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
            
        }
    }

    Vector3 GetRollingBallPosition()
    {
        // 실제 공 위치 로직으로 대체해야 함
        return ballLandingPosition;
    }

    void PickUpBall()
    {
        isChasing = false;
        Debug.Log("공을 주웠습니다!");
        // 추가 행동
    }

    private void OnDrawGizmos()
    {
        if(isInfield)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, catchRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, catchRadius * flightTime);
        }

        

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ballLandingPosition, 3.0f);
    }

    void CatchTheBall()
    {
        if(Vector3.Distance(m_pbBall.transform.position, transform.position) < 1.5f)
        {
            m_Ball.m_isCatched = true;
            m_Ball.m_rigid.useGravity = false;
            m_Ball.m_sphereCollider.material = null;
        }
    }
}
