using PathCreation;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Ball : MonoBehaviour
{
    public bool isCollided = false;
    public bool isTriggered = false;
    public GameObject m_parent;
    public GameObject m_body;
    public GameObject m_particleSystemGO;
    public Rigidbody m_rigidbody;
    public MeshRenderer m_meshRenderer;
    public ParticleSystem m_particleSystem;
    public MainModule particlesMainModule;

    public PathCreator pathCreator;

    public float speed;
    private float distanceTravelled;


    private void Start()
    {
        if (m_particleSystem)
            particlesMainModule = m_particleSystem.main;
        m_particleSystem.Stop();
        m_particleSystemGO.SetActive(false);
    }

    private void Update()
    {
        if (pathCreator != null)
        {
            Debug.Log("PathCreator not NULL");
            Vector3 prePos = transform.position;

            // Player Parent Follow Path
            distanceTravelled += speed * Time.deltaTime;

            m_parent.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            Vector3 newPos = transform.position;
            newPos.y = m_parent.transform.position.y;
            newPos.z = m_parent.transform.position.z;

            transform.position = newPos;

            if (distanceTravelled + 0.5f >= pathCreator.path.length)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None;
                pathCreator = null;
                m_rigidbody.useGravity = true;
                m_rigidbody.velocity = Vector3.zero;
                m_rigidbody.AddForce(new Vector3(0f, -1, -0.4f) * 10f, ForceMode.Impulse);
            }

            Vector3 postPos = transform.position;

            m_particleSystemGO.transform.position = transform.position;
            Vector3 dir = -(postPos - prePos).normalized;
            m_particleSystemGO.transform.LookAt(m_particleSystemGO.transform.position + dir);
        }
        else
        {
            Debug.Log("ELSE Working");
            m_particleSystemGO.transform.position = transform.position;
            Vector3 dir = -m_rigidbody.velocity.normalized;

            m_particleSystemGO.transform.LookAt(m_particleSystemGO.transform.position + dir);
        }
    }

    public void ResetPosition(PathCreator _pathCreator, Vector3 localPos, float incrementSpeed = 0)
    {
        Debug.Log("ResetPosition Called!");
        if (_pathCreator)
        {
            Debug.Log("PathCreator YES");
        }
        else
        {
            Debug.Log("PathCreator NO");

        }

        speed += incrementSpeed;
        speed = Mathf.Clamp(speed, 10f, 20f);
        m_rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        distanceTravelled = 0f;
        pathCreator = _pathCreator;
        if (pathCreator)
        {
            Debug.Log("PathCreator SET");
        }
        else
        {
            Debug.Log("PathCreator NOTSET");

        }
        transform.localPosition = localPos;
        transform.localRotation = Quaternion.identity;
        m_rigidbody.AddTorque(new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y / 2f, Random.insideUnitCircle.x / 2f) * 10f, ForceMode.Impulse);

    }

    public void OnCollisionEnter(Collision collision)
    {
        pathCreator = null;
        m_rigidbody.constraints = RigidbodyConstraints.None;

        if (collision.collider.CompareTag("Hoop"))
        {
            isCollided = true;
            GameManager.Instance.DisableEffects();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        pathCreator = null;
        m_rigidbody.constraints = RigidbodyConstraints.None;

        if (other.CompareTag("HoopTrig"))
        {
            isTriggered = true;

            if (isTriggered && isCollided)
            {
                GameManager.Instance.ScorePoint(false);
            }
            else
            {
                GameManager.Instance.ScorePoint(true);
            }

        }

        if (!isTriggered && other.CompareTag("HoopMissedTrig"))
        {
            Time.timeScale = 0f;
        }
        else if (other.CompareTag("HoopMissedTrig"))
        {
            isCollided = false;
            isTriggered = false;
        }
    }
}
