using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI pointsText;

    public HoopController hoopController;
    //public BallSpawner ballSpawner;

    public float speedFactor = 10f;
    public int points = 0;
    public int hoopStreak = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1;
        points = 0;
        pointsText.text = points.ToString();
    }
    public void ScorePoint(bool isPerfectHoop)
    {
        if (isPerfectHoop)
        {
            hoopStreak++;
            hoopController.ShowStreakVFX();
        }
        else
        {
            hoopStreak = 1;
        }


        int prePoints = points;
        points += hoopStreak;
        pointsText.text = points.ToString();
        int postPoints = points;

        int diff = postPoints - prePoints;
        int rem = 10- (prePoints % 10);
        if(diff > rem || postPoints%10 == 0)
        {
            Debug.Log("Pre Points = " + prePoints + "  , Post Points = " + postPoints + "\nDiff = " + diff + "\nRem = " + rem);
            RecalculateSpeed();
        }
    }

    private void RecalculateSpeed()
    {
        Time.timeScale += (1 / speedFactor);
        if (Time.timeScale > 2)
        {
            Time.timeScale = 2f;

        }
        else
        {
            hoopController.horizontalSpeed -= (hoopController.initialHorizontalSpeed / speedFactor);

        }

    }


    public void OnClick_RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
