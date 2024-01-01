using UnityEngine;

public class HoopController : MonoBehaviour
{
    public GameObject streakVFX;

    public float horizontalSpeed;
    public float testingSpeed;

    public float minX;
    public float maxX;

    private Touch currTouch;
    

    private Vector3 targetPosition; // Store the target position
    public float lerpSpeed = 5.0f; // Adjust this value for the desired smoothness

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.touchCount > 0)
        {
            currTouch = Input.GetTouch(0);
            if (currTouch.phase == TouchPhase.Moved)
            {
                float deltaX = currTouch.deltaPosition.x;

                targetPosition.x += deltaX * Time.deltaTime * horizontalSpeed;
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            }
        }

        // Use Vector3.Lerp to smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }


    //private void Update()
    //{
    //    if (Time.timeScale == 0) return;

    //    if (Input.touchCount > 0)
    //    {
    //        currTouch = Input.GetTouch(0);
    //        if (currTouch.phase == TouchPhase.Moved)
    //        {
    //            float deltaX = currTouch.deltaPosition.x;

    //            Vector3 newPos = transform.position;
    //            newPos.x += deltaX * Time.deltaTime * initialHorizontalSpeed;
    //            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
    //            transform.position = newPos;
    //        }
    //    }

    //}
    public void ShowStreakVFX()
    {
        Instantiate(streakVFX, transform);
    }
}
