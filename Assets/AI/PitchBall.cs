using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchBall : MonoBehaviour
{
    public static PitchBall instance;

    public Rigidbody m_rigid = null;
    public SphereCollider m_sphereCollider = null;

    public float m_fPitchPower = 40.0f; // 공의 속도
    public Vector3 m_vecPitchTargetPos = Vector3.zero; // 던질 목표 위치

    private bool m_isPitched = false;   // 공이 던져진 상태
    private bool m_isBatted = false;    // 공이 타격된 상태
    private bool m_isGround = false;    // 공이 땅에 닿은 상태

    public Vector3 m_vecHitTargetPos;   // 타격 성공 시 날아갈 위치

    public float maxHeight = 2.5f; // 최고점 높이
    public float m_fJourneyTime = 5.0f; // 공의 비행 시간
    public float m_fTime = 0.0f;  // 공이 타격된 시간
    public float m_fFlightTime = 0.0f; // 공의 비행 시간

    [SerializeField]
    private PhysicMaterial m_material = null;
    private Vector3 m_vecInitPos;

    public bool m_isInfield;

    [SerializeField]
    private Fielder[] m_objFielders;
    Fielder m_objFielder;

    public bool m_isCatched = false;
    GameObject m_objTargetFielder;
    Vector3 m_vecThrowTarget;

    private void Start()
    {
        instance = this;
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        m_rigid.useGravity = false;
        m_sphereCollider = GetComponent<SphereCollider>();

        m_objFielders = FindObjectsOfType<Fielder>();

        foreach (Fielder fielder in m_objFielders)
        {
            fielder.m_pbBall = gameObject;
        }
    }


    private void FixedUpdate()
    {
        if (m_isPitched)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_vecPitchTargetPos, Time.fixedDeltaTime * m_fPitchPower);
            if (m_fPitchPower > 20.0f)
            {
                m_fPitchPower -= 0.002f;
            }
        }
        else if (m_isBatted && !m_isGround)
        {
            // 공이 타격된 후 포물선을 그리며 이동
            float elapsedTime = (Time.time - m_fTime) / m_fJourneyTime;
            float height = Mathf.Sin(Mathf.PI * elapsedTime) * maxHeight;

            Vector3 newPos = Vector3.Lerp(m_vecInitPos, m_vecHitTargetPos, elapsedTime);
            newPos.y += height;

            transform.position = newPos;

            // 목표 지점에서 일정 거리 내에 도달하면 물리엔진 활성화
            if (Vector3.Distance(transform.position, m_vecHitTargetPos) < 10.0f)
            {
                m_rigid.useGravity = true;
                m_sphereCollider.material = m_material;

                // 목표 지점 방향으로 힘을 가합니다.
                Vector3 direction = (m_vecHitTargetPos - transform.position).normalized;
                m_rigid.velocity = direction * Mathf.Sqrt(2 * Physics.gravity.magnitude * maxHeight); // 초기 속도 설정
            }
        }
        else if (!m_isBatted && !m_isGround)
        {
            // 공이 타격되지 않았고, 일정 시간이 지나면 파괴
            Destroy(gameObject, 20.0f); // 예시로 20초 설정
        }

        if(m_isCatched)
        {
            if (!m_objFielder.isInfield)
            {
                m_objTargetFielder =
                Vector3.Distance(m_objFielder.transform.position, FieldingManager.instance.FieldersPosition[3].transform.position) >
                Vector3.Distance(m_objFielder.transform.position, FieldingManager.instance.FieldersPosition[5].transform.position)
                ? FieldingManager.instance.FieldersPosition[5] : FieldingManager.instance.FieldersPosition[3];

                m_vecThrowTarget = new Vector3(m_objFielder.transform.position.x, 1.0f, m_objFielder.transform.position.z);

                FieldingOutFieldThrow(m_vecThrowTarget);
            }
            else
            {

                FieldeingInFieldThrow();
            }
        }
        
    }

    public void OnPitch(Vector3 m_vecTarget)
    {
        gameObject.transform.SetParent(null);
        m_vecPitchTargetPos = m_vecTarget;
        m_fFlightTime = CalculateTimeToReachTarget(transform.position, m_vecTarget, m_fPitchPower, m_fPitchPower - 1.0f);
        m_isPitched = true;
        m_fTime = Time.time;
    }

    public void BattedBall(Vector3 m_vecHitTarget)
    {
        Debug.Log("BattedBall 호출됨");

        m_vecInitPos = transform.position;
        m_isPitched = false;
        m_isBatted = true;
        m_isGround = false;
        m_vecHitTargetPos = m_vecHitTarget;
        m_fTime = Time.time;
        m_rigid.useGravity = false; // 타격 시 중력 비활성화

        float distance = 987654321.0f;
        foreach (Fielder fielder in m_objFielders)
        {
            fielder.ballLandingPosition = m_vecHitTargetPos;
            if (Vector3.Distance(fielder.transform.position, m_vecHitTarget) < distance && (m_isInfield == fielder.isInfield))
            {
                distance = Vector3.Distance(fielder.transform.position, m_vecHitTarget);
                fielder.isCatching = true;
                foreach(Fielder tmp in m_objFielders)
                {
                    if(tmp != fielder)
                    {
                        tmp.isCatching = false;
                    }
                }

                m_objFielder = fielder;
            }
            else if(!fielder.isInfield)
            {
                bool isCheck = false;

                foreach(Fielder tmp in m_objFielders)
                {
                    if(tmp.isCatching)
                    {
                        isCheck = true;
                        break;
                    }
                }

                if(!isCheck)
                {
                    distance = Vector3.Distance(fielder.transform.position, m_vecHitTarget);
                    fielder.isCatching = true;
                    foreach (Fielder tmp in m_objFielders)
                    {
                        if (tmp != fielder)
                        {
                            tmp.isCatching = false;
                        }
                    }
                }
                m_objFielder = fielder;
            }
            else
            {
                fielder.isCatching = false;
            }
        }

        transform.rotation = Quaternion.identity;
        m_fFlightTime = CalculateTimeToReachTarget(m_vecInitPos, m_vecHitTargetPos, m_fPitchPower, m_fPitchPower - 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            m_isGround = true;
            m_sphereCollider.material = m_material;
            m_rigid.useGravity = true;

            // 공이 땅에 닿으면 속도를 유지하면서 점진적으로 줄어들도록 설정
            Vector3 direction = (m_vecHitTargetPos - m_vecInitPos).normalized;
            m_rigid.velocity = direction * m_rigid.velocity.magnitude; // 기존 속도의 50%로 설정

            // 추가 힘 적용 대신 마찰력 조정
            // m_rigid.AddForce(direction * 10.0f / m_fFlightTime, ForceMode.Impulse);
        }

        // 예시: 공을 제거하는 로직 (필요시 수정)
        if (other.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
        }
    }

    private float CalculateTimeToReachTarget(Vector3 startPos, Vector3 targetPos, float initialSpeed, float minSpeed)
    {
        float distance = Vector3.Distance(startPos, targetPos);
        float averageSpeed = (initialSpeed + minSpeed) / 2;
        return distance / averageSpeed;
    }


    private Vector3 CalculateInitialVelocity(Vector3 startPos, Vector3 targetPos, float maxHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0.0f, targetPos.z - startPos.z);

        float timeToMaxHeight = Mathf.Sqrt(-2 * maxHeight / gravity);
        float totalTime = timeToMaxHeight + Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeight);
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + velocityY;
    }

    void FieldingOutFieldThrow(Vector3 targetThorw)
    {
        if(Vector3.Distance(m_objTargetFielder.transform.position, transform.position) < 1.0f)
        {
            m_objFielder = m_objTargetFielder.GetComponent<Fielder>();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetThorw, 5.0f);
    }

    void FieldeingInFieldThrow()
    {
        Vector3 throwTarget = Vector3.zero;

        if(PlayerRunning.Instance.m_nTargetBase == 1 && PlayerRunning.Instance.m_nNowBase == 0)
        {
            throwTarget = new Vector3(-28.5f, 1.0f, 31.5f);
        }
        else if(PlayerRunning.Instance.m_nTargetBase == 1 && PlayerRunning.Instance.m_nNowBase == 1 || PlayerRunning.Instance.m_nTargetBase == 2)
        {
            throwTarget = new Vector3(0.25f, 1.0f, 5.25f);
        }
        else if(PlayerRunning.Instance.m_nTargetBase == 3 && PlayerRunning.Instance.m_nNowBase == 2)
        {
            throwTarget = new Vector3(28.5f, 1.0f, 5.25f);
        }
        else if(PlayerRunning.Instance.m_nTargetBase == 4)
        {
            throwTarget = new Vector3(0.25f, 1.0f, 62.35f);
        }
        else
        {
            throwTarget = transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, throwTarget, 5.0f);
    }
}
