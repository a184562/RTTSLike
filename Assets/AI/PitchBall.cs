using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchBall : MonoBehaviour
{
    public static PitchBall instance;

    public Rigidbody m_rigid = null;
    public SphereCollider m_sphereCollider = null;

    public float m_fPitchPower = 40.0f; // ���� �ӵ�
    public Vector3 m_vecPitchTargetPos = Vector3.zero; // ���� ��ǥ ��ġ

    private bool m_isPitched = false;   // ���� ������ ����
    private bool m_isBatted = false;    // ���� Ÿ�ݵ� ����
    private bool m_isGround = false;    // ���� ���� ���� ����

    public Vector3 m_vecHitTargetPos;   // Ÿ�� ���� �� ���ư� ��ġ

    public float maxHeight = 2.5f; // �ְ��� ����
    public float m_fJourneyTime = 5.0f; // ���� ���� �ð�
    public float m_fTime = 0.0f;  // ���� Ÿ�ݵ� �ð�
    public float m_fFlightTime = 0.0f; // ���� ���� �ð�

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
            // ���� Ÿ�ݵ� �� �������� �׸��� �̵�
            float elapsedTime = (Time.time - m_fTime) / m_fJourneyTime;
            float height = Mathf.Sin(Mathf.PI * elapsedTime) * maxHeight;

            Vector3 newPos = Vector3.Lerp(m_vecInitPos, m_vecHitTargetPos, elapsedTime);
            newPos.y += height;

            transform.position = newPos;

            // ��ǥ �������� ���� �Ÿ� ���� �����ϸ� �������� Ȱ��ȭ
            if (Vector3.Distance(transform.position, m_vecHitTargetPos) < 10.0f)
            {
                m_rigid.useGravity = true;
                m_sphereCollider.material = m_material;

                // ��ǥ ���� �������� ���� ���մϴ�.
                Vector3 direction = (m_vecHitTargetPos - transform.position).normalized;
                m_rigid.velocity = direction * Mathf.Sqrt(2 * Physics.gravity.magnitude * maxHeight); // �ʱ� �ӵ� ����
            }
        }
        else if (!m_isBatted && !m_isGround)
        {
            // ���� Ÿ�ݵ��� �ʾҰ�, ���� �ð��� ������ �ı�
            Destroy(gameObject, 20.0f); // ���÷� 20�� ����
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
        Debug.Log("BattedBall ȣ���");

        m_vecInitPos = transform.position;
        m_isPitched = false;
        m_isBatted = true;
        m_isGround = false;
        m_vecHitTargetPos = m_vecHitTarget;
        m_fTime = Time.time;
        m_rigid.useGravity = false; // Ÿ�� �� �߷� ��Ȱ��ȭ

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

            // ���� ���� ������ �ӵ��� �����ϸ鼭 ���������� �پ�鵵�� ����
            Vector3 direction = (m_vecHitTargetPos - m_vecInitPos).normalized;
            m_rigid.velocity = direction * m_rigid.velocity.magnitude; // ���� �ӵ��� 50%�� ����

            // �߰� �� ���� ��� ������ ����
            // m_rigid.AddForce(direction * 10.0f / m_fFlightTime, ForceMode.Impulse);
        }

        // ����: ���� �����ϴ� ���� (�ʿ�� ����)
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
