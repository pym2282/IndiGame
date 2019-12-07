using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class HighScoreUtil
{    
    public struct HighScore
    {
        public HighScore(string userName, float score)
        {
            this.userName = userName;
            this.score = score;
        }
        public string userName;
        public float score;
    }

    private static List<HighScore> _highScores;

    public const string HIGHSCORE_FILE_NAME = "highscores.dat";
    public const int MAX_COUNT = 5;

    public static int GetNewHighScoreIndex(float score)
    {
        int index = 0;
        foreach (HighScore highScore in GetHighScores())
        {
            if (score > highScore.score)
            {
                break;
            }
            index++;
        }
        return index;
    }

    public static void SetNewHighScore(string userName, float score)
    {
        int index = GetNewHighScoreIndex(score);
        GetHighScores().Insert(index, new HighScore(userName, score));
        SaveHighScore();
    }

    public static List<HighScore> GetHighScores()
    {
        if (_highScores == null)
        {
            _highScores = new List<HighScore>(MAX_COUNT);
            if (!File.Exists(Application.persistentDataPath + "/" + HIGHSCORE_FILE_NAME))
            {
                for (int i = 0; i < MAX_COUNT; i++)
                {
                    _highScores.Add(new HighScore("___", 0));
                }
                return _highScores;
            }
            StreamReader stream = File.OpenText(Application.persistentDataPath + "/" + HIGHSCORE_FILE_NAME);
            while (true)
            {
                string line = stream.ReadLine();
                if (line == null || line.Length <= 0)
                {
                    break;
                }
                HighScore highScore = new HighScore(line, 0);
                line = stream.ReadLine();
                if (line == null || line.Length <= 0)
                {
                    break;
                }
                if (float.TryParse(line, out float score))
                {
                    highScore.score = score;
                    _highScores.Add(highScore);
                }
            }
            stream.Close();
        }

        for (int i = _highScores.Count; i < MAX_COUNT; i++)
        {
            _highScores.Add(new HighScore("___", 0));
        }

        return _highScores;
    }

    public static void SaveHighScore()
    {
        if (GetHighScores().Count > MAX_COUNT)
        {
            GetHighScores().RemoveRange(MAX_COUNT, GetHighScores().Count - MAX_COUNT);
        }
        StreamWriter stream = File.CreateText(Application.persistentDataPath + "/" + HIGHSCORE_FILE_NAME);
        foreach (HighScore highScore in GetHighScores())
        {
            stream.WriteLine(highScore.userName);
            stream.WriteLine(highScore.score);
        }
        stream.Close();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Clear Highscore")]
#endif
    public static void ClearHighScore()
    {
        if (File.Exists(Application.persistentDataPath + "/" + HIGHSCORE_FILE_NAME))
        {
            File.Delete(Application.persistentDataPath + "/" + HIGHSCORE_FILE_NAME);
            Debug.Log($"Deleted {Application.persistentDataPath}/{HIGHSCORE_FILE_NAME}");
        }
    }
}
