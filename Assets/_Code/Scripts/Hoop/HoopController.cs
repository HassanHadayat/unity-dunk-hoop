using UnityEngine;

public class HoopController : MonoBehaviour
{
    public GameObject streakVFX;

    public float horizontalSpeed;
    public float testingSpeed;

    public float minX;
    public float maxX;

    [HideInInspector] public float initialHorizontalSpeed;
    private Touch currTouch;
    private void Start()
    {
        initialHorizontalSpeed = horizontalSpeed;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            currTouch = Input.GetTouch(0);
            if (currTouch.phase == TouchPhase.Moved)
            {
                float deltaX = currTouch.deltaPosition.x;

                Vector3 newPos = transform.position;
                //newPos.x += deltaX * testingSpeed;
                newPos.x += deltaX * Time.fixedDeltaTime * initialHorizontalSpeed;
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
                transform.position = newPos;
            }
        }

    }
    public void ShowStreakVFX()
    {
        Instantiate(streakVFX, transform);
    }
}
