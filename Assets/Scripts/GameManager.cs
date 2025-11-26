using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject panelGame;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private RawImage objectiveHealthImage;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textBestScore;
    [SerializeField] private TextMeshProUGUI textYourScore;

    private int maxObjectiveHealth = 500;
    [SerializeField] private int objectiveHealth = 500;

    // ENCAPSULATION
    [SerializeField] private int damage = 10;

    // ENCAPSULATION
    private int score = 0;

    // ENCAPSULATION
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            textScore.SetText($"Score : {score}");
        }
    }

    // ENCAPSULATION
    public int Damage
    {
        get { return damage; }
    }

    // ENCAPSULATION
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateObjectiveHealth();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        panelGame = GameObject.Find("Panel Game");
        panelGameOver = GameObject.Find("Panel Game Over");
        objectiveHealthImage = GameObject.Find("Image Objective Health").GetComponent<RawImage>();
        textScore = GameObject.Find("Text Score").GetComponent<TextMeshProUGUI>();
        textBestScore = GameObject.Find("Text Best Score").GetComponent<TextMeshProUGUI>();
        textYourScore = GameObject.Find("Text Your Score").GetComponent<TextMeshProUGUI>();

        panelGameOver.gameObject.SetActive(false);
    }

    void UpdateObjectiveHealth()
    {
        RectTransform rt = objectiveHealthImage.rectTransform;

        Vector2 size = rt.sizeDelta;
        size.x = objectiveHealth;
        rt.sizeDelta = size;
    }

    public void HurtObjective(int damage)
    {
        objectiveHealth -= damage;
        if (objectiveHealth <= 0)
        {
            GameOver();
        }
        UpdateObjectiveHealth();
    }

    public void HealObjective(int healing)
    {
        if (objectiveHealth < maxObjectiveHealth)
        {
            objectiveHealth += healing;
        }

        if (objectiveHealth > maxObjectiveHealth)
        {
            objectiveHealth = maxObjectiveHealth;
        }

        UpdateObjectiveHealth();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        panelGame.gameObject.SetActive(false);
        panelGameOver.gameObject.SetActive(true);

        if (ScoreManager.Instance != null)
        {
            if (score > ScoreManager.Instance.GetScoreValue())
            {
                ScoreManager.Instance.SaveScore(score);
                textBestScore.SetText("You got the best score");
            }
            else
            {
                textBestScore.SetText(ScoreManager.Instance.GetBestScoreText(score));
            }
        }

        textYourScore.SetText($"Your Score : {score}");
    }
}
