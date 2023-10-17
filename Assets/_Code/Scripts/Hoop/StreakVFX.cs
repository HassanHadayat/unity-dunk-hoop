using UnityEngine;

public class StreakVFX : MonoBehaviour
{
    public float startScale = 0.25f;
    public float endScale = 0.5f;

    public float scaleSpeed = 1.0f;

    private void Start()
    {
        transform.localScale = Vector3.one * startScale;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newScale = new Vector3(1 * Time.deltaTime * scaleSpeed, 1 * Time.deltaTime * (scaleSpeed / 2), 0);
        //transform.localScale += Vector3.forward * Time.deltaTime * scaleSpeed;
        transform.localScale += newScale;

        if (transform.localScale.x >= endScale)
        {
            Destroy(gameObject);
        }

    }
}
