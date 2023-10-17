using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool isCollided = false;
    public bool isTriggered = false;



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hoop"))
        {
            isCollided = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HoopTrig"))
        {
            isTriggered = true;

            if(isTriggered && isCollided)
            {
                GameManager.Instance.ScorePoint(false);
            }
            else
            {
                GameManager.Instance.ScorePoint(true);
            }

            isCollided = false;
            isTriggered = false;
        }
    }
}
