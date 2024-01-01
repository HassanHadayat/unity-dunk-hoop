using PathCreation;
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
    private int speedCheckpoint = 20;
    public float ballSpawningDelay = 2f;
    private bool isShootingContinously = false;

    public Transform dynamicTrans;

    public float minInstantiateX;
    public float maxnInstantiateX;

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

    public PathCreator pathCreator;

    private void Start()
    {
        if (pathCreator)
        {
            Debug.Log("GOOD START");
        }
        else
        {
            Debug.Log("BAD START");
        }

        // Prepar the Balls Pool
        InstantiateBallsPool();

        InvokeRepeating("ShootBall", 1f, ballSpawningDelay);
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
        while (ShootCounter > 0)
        {

            if (ballsPoolIndex >= ballsPoolSize)
            {
                ballsPoolIndex = 0;
            }
            GameObject ball = ballsPool[ballsPoolIndex++];

            ball.GetComponent<Ball>().m_parent.transform.position = instantiateTrans.position;

            if (GameManager.Instance.points >= speedCheckpoint)
            {
                speedCheckpoint += 20;
                ballSpawningDelay -= 0.2f;
                ballSpawningDelay = Mathf.Clamp(ballSpawningDelay, 1f, 2f);

                ball.GetComponent<Ball>().ResetPosition(pathCreator, Vector3.right * randInstantiatePosX, 0.5f);
                CancelInvoke("ShootBall");
                InvokeRepeating("ShootBall", 1f, ballSpawningDelay);

            }
            else
            {

                ball.GetComponent<Ball>().ResetPosition(pathCreator, Vector3.right * randInstantiatePosX);
            }

            yield return new WaitForSeconds(0.15f);

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

        ball.GetComponent<Ball>().m_parent.transform.position = instantiateTrans.position;
        ball.GetComponent<Ball>().ResetPosition(pathCreator, Vector3.right * Random.Range(minInstantiateX, maxnInstantiateX));
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
