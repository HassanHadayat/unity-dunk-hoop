using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Ball : MonoBehaviour
{
    public bool isCollided = false;
    public bool isTriggered = false;
    public GameObject m_body;
    public GameObject m_particleSystemGO;
    public Rigidbody m_rigidbody;
    public MeshRenderer m_meshRenderer;
    public ParticleSystem m_particleSystem;
    public MainModule particlesMainModule;

    private void Start()
    {
        if (m_particleSystem)
            particlesMainModule = m_particleSystem.main;
        m_particleSystem.Stop();
        m_particleSystemGO.SetActive(false);
    }

    private void Update()
    {
        m_particleSystemGO.transform.position = transform.position;
        Vector3 dir = -m_rigidbody.velocity.normalized;

        m_particleSystemGO.transform.LookAt(m_particleSystemGO.transform.position + dir);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hoop"))
        {
            isCollided = true;
            GameManager.Instance.DisableEffects();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
