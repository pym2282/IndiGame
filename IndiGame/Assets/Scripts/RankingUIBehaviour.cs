using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingUIBehaviour : MonoBehaviour
{
    public RankingRowBehaviour rankingContentPrefab;
    public Transform rankingParent;
    public int maxNameLength = 3;

    private RankingRowBehaviour _currentPlayerRanking;
    private int _currentNameIndex = 0;
    private System.Text.StringBuilder _currentName = new System.Text.StringBuilder();
    private float _currentScore = 0;
    private bool _inited = false;

    private void Start()
    {
        gameObject.SetActive(false);
        _inited = false;
    }

    public void Init()
    {   
        _currentName.Append("___");
        GameController gc = FindObjectOfType<GameController>();
        if (gc == null)
        {
            Debug.LogAssertion("GameController가 없읍니다");
        }
        _currentScore = gc.Time;
        int playerIndex = HighScoreUtil.GetNewHighScoreIndex(_currentScore);
        Debug.Log($"PlayerIndex: {playerIndex}");

        int index = 0;
        foreach (HighScoreUtil.HighScore highScore in HighScoreUtil.GetHighScores())
        {
            if (index >= HighScoreUtil.MAX_COUNT)
            {
                break;
            }
            if (index == playerIndex)
            {
                _currentPlayerRanking = MakeRow(new HighScoreUtil.HighScore(_currentName.ToString(), _currentScore), playerIndex + 1, playerIndex);
                _currentPlayerRanking.SetColor(Color.green);
                index++;
            }
            MakeRow(highScore, index + 1, index);
            index++;
        }

        if (_currentPlayerRanking == null)
        {
            _currentPlayerRanking = MakeRow(new HighScoreUtil.HighScore("you", _currentScore), -1, playerIndex);
            _currentPlayerRanking.SetColor(Color.green);
            _currentNameIndex = _currentName.Length;
        }

        _inited = true;
    }

    private void Update()
    {
        if (!_inited)
        {
            return;
        }

        if (_currentNameIndex < _currentName.Length)
        {
            for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    _currentName.Replace('_', (char)i, _currentNameIndex, 1);
                    _currentNameIndex++;
                    _currentPlayerRanking.SetUserName(_currentName.ToString());
                    break;
                }
            }
            if (_currentNameIndex >= _currentName.Length)
            {
                HighScoreUtil.SetNewHighScore(_currentName.ToString(), _currentScore);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAndRestart();
        }


        //if (_currentNameIndex > 0)
        //{
        //    if (Input.GetKeyDown(KeyCode.Backspace))
        //    {
        //        _currentName.Remove(_currentName.)
        //    }
        //}
    }

    private RankingRowBehaviour MakeRow(HighScoreUtil.HighScore data, int rank, int index)
    {
        RankingRowBehaviour obj = Instantiate(rankingContentPrefab, rankingParent);
        obj.SetData(rank, data);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -rt.sizeDelta.y * index);
        return obj;
    }

    public void CheckAndRestart()
    {
        if (_inited && _currentNameIndex >= _currentName.Length)
        {
            FindObjectOfType<GameController>().ReStart();
        }
    }
}
