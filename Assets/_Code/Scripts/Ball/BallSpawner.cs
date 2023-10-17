using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;

    public Transform hoopParentTrans;
    public Transform hoopTargetTrans;
    public Transform instantiateTrans;
    public float time;

    public Transform dynamicTrans;

    public float minInstantiateX;
    public float maxnInstantiateX;

    public float gravity = -9.81f;

    public GameObject[] ballsPool;
    private int ballsPoolIndex = 0;
    private int ballsPoolSize = 10;
    public Transform ballsPoolPos;

    private void Start()
    {
        // Prepar the Balls Pool
        InstantiateBallsPool();


        InvokeRepeating("ShootBall", 2f, 1f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }
    }

    private void InstantiateBallsPool()
    {
        ballsPool = new GameObject[ballsPoolSize];
        for (int i = 0; i < ballsPoolSize; i++)
        {
            ballsPool[i] = Instantiate(ballPrefab, dynamicTrans);
            ballsPool[i].transform.position = ballsPoolPos.position;
        }
    }
    public void ShootBall()
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
        rb.AddTorque(Vector3.right, ForceMode.Impulse);
    }
}
