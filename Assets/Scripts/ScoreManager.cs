using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private string username;

    private ScoreData scoreData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScore();
    }

    public void UpdateUsername(string value)
    {
        username = value;
    }

    public void SaveScore(int score)
    {
        ScoreData data = new()
        {
            username = username != null ? username : "Unknown",
            score = score
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);

            scoreData = data;
        }
    }

    public int GetScoreValue()
    {
        if (scoreData == null)
        {
            return 0;
        }
        return scoreData.score;
    }

    public string GetScoreText()
    {
        if (scoreData == null)
        {
            return "Unknown : 0";
        }
        return $"{scoreData.username} : {scoreData.score}";
    }

    public string GetBestScoreText(int playerScore)
    {
        if (scoreData == null)
        {
            return $"Best Score : {username} : {playerScore}";
        }
        return $"Best Score : {scoreData.username} : {scoreData.score}";
    }
}
