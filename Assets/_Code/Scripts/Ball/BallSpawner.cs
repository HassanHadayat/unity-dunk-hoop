using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;

    public Transform hoopParentTrans;
    public Transform hoopTargetTrans;
    public Transform instantiateTrans;
    public float time;
    public int pointsCheckpoint = 50;
    private bool isShootingContinously = false;

    public Transform dynamicTrans;

    public float minInstantiateX;
    public float maxnInstantiateX;

    public float gravity = -9.81f;

    public GameObject[] ballsPool;
    private int ballsPoolIndex = 0;
    private int ballsPoolSize = 10;
    public Transform ballsPoolPos;

    public Gradient smokeStreakOne;
    public Gradient smokeStreakTwice;
    public Gradient fireStreakThrice;

    public Material normalMaterial;
    public Material fireMaterial;

    public GameObject normalBG;
    public GameObject fireBG;

    private void Start()
    {
        // Prepar the Balls Pool
        InstantiateBallsPool();

        InvokeRepeating("ShootBall", 2f, 1f);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ShootBall();
    //    }
    //}

    private void InstantiateBallsPool()
    {
        ballsPool = new GameObject[ballsPoolSize];
        for (int i = 0; i < ballsPoolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab, dynamicTrans);
            ballsPool[i] = ball.transform.GetChild(0).gameObject; // Get Child (Body)
            ballsPool[i].transform.position = ballsPoolPos.position;
        }
    }
    public void ShootBall()
    {

        if (GameManager.Instance.points >= pointsCheckpoint && !isShootingContinously)
        {
            pointsCheckpoint += 50;
            // Shoot Continously
            StartCoroutine(ShootContinously());
        }
        else if (!isShootingContinously)
        {
            ShootNormal();
        }

    }

    IEnumerator ShootContinously()
    {
        isShootingContinously = true;

        int ShootCounter = 3;
        float randInstantiatePosX = Random.Range(minInstantiateX, maxnInstantiateX);
        float randEndPosX = Random.Range(-1f, 1f);
        while (ShootCounter > 0)
        {

            if (ballsPoolIndex >= ballsPoolSize)
            {
                ballsPoolIndex = 0;
            }
            GameObject ball = ballsPool[ballsPoolIndex++];
            Vector3 newPos = instantiateTrans.position;
            newPos.x = randInstantiatePosX;
            ball.transform.position = newPos;

            Vector3 instantiatePos = newPos;
            Vector3 endPos = hoopParentTrans.position;
            endPos.x = Random.Range(-1f, 1f);

            Vector3 displacement = endPos - instantiatePos;
            float displacementZ = displacement.z;
            float displacementY = displacement.y;

            // Calculate initial horizontal velocity
            float initialVelocityZ = displacementZ / (time);

            // Calculate initial vertical velocity
            float initialVelocityY = (displacementY - (0.5f * gravity * time * time)) / time;

            // Apply the calculated initial velocity to the Rigidbody
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0f, initialVelocityY, initialVelocityZ);

            ball.GetComponent<Ball>().m_body.transform.Rotate(Random.insideUnitCircle, 0.5f);
            rb.AddTorque(Vector3.right, ForceMode.Impulse);


            yield return new WaitForSeconds(0.3f);

            ShootCounter--;
        }
        isShootingContinously = false;
    }
    private void ShootNormal()
    {
        if (ballsPoolIndex >= ballsPoolSize)
        {
            ballsPoolIndex = 0;
        }
        GameObject ball = ballsPool[ballsPoolIndex++];
        Vector3 newPos = instantiateTrans.position;
        newPos.x = Random.Range(minInstantiateX, maxnInstantiateX);
        ball.transform.position = newPos;

        Vector3 instantiatePos = newPos;
        Vector3 endPos = hoopParentTrans.position;
        endPos.x = Random.Range(-1f, 1f);

        Vector3 displacement = endPos - instantiatePos;
        float displacementZ = displacement.z;
        float displacementY = displacement.y;

        // Calculate initial horizontal velocity
        float initialVelocityZ = displacementZ / (time);

        // Calculate initial vertical velocity
        float initialVelocityY = (displacementY - (0.5f * gravity * time * time)) / time;

        // Apply the calculated initial velocity to the Rigidbody
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0f, initialVelocityY, initialVelocityZ);

        ball.GetComponent<Ball>().m_body.transform.Rotate(Random.insideUnitCircle, 0.5f);
        rb.AddTorque(Vector3.right, ForceMode.Impulse);
    }

    public void BallStreakVFX(int hoopStreak)
    {
        Gradient newGradient = smokeStreakOne;
        Material material = normalMaterial;

        if (hoopStreak == 1)
        {
            normalBG.SetActive(true);
            fireBG.SetActive(false);
            GameManager.Instance.pointsText.color = new Color(0.1647059f, 0.1647059f, 0.1647059f, 1);

            //Time.timeScale = 0f;

            foreach (var ball in ballsPool)
            {
                Ball b = ball.GetComponent<Ball>();

                b.m_meshRenderer.material = normalMaterial;
                b.m_particleSystemGO.SetActive(false);
                b.m_particleSystem.Stop();
            }

            return;
        }
        else if (hoopStreak == 2)
        {
            newGradient = smokeStreakOne;
            material = normalMaterial;

            normalBG.SetActive(true);
            fireBG.SetActive(false);
        }
        else if (hoopStreak == 3)
        {
            material = normalMaterial;
            newGradient = smokeStreakTwice;

            normalBG.SetActive(true);
            fireBG.SetActive(false);
        }
        else if (hoopStreak == 4)
        {
            newGradient = fireStreakThrice;
            material = fireMaterial;

            fireBG.SetActive(true);
            normalBG.SetActive(false);

            GameManager.Instance.pointsText.color = Color.white;
        }
        else if (hoopStreak > 4)
        {
            return;
        }

        foreach (var ball in ballsPool)
        {
            Ball b = ball.GetComponent<Ball>();
            b.particlesMainModule.startColor = new ParticleSystem.MinMaxGradient(newGradient);
            b.m_meshRenderer.material = material;
            b.m_particleSystemGO.SetActive(true);
            b.m_particleSystem.Play();
        }

    }
}
