using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingRowBehaviour : MonoBehaviour
{
    [SerializeField]
    private Text _rank;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _score;

    public void SetData(int rank, HighScoreUtil.HighScore data)
    {
        _rank.text = (rank > 0) ? $"{rank}." : "---";
        SetUserName(data.userName);
        _score.text = data.score.ToString("0.00");
    }

    public void SetColor(Color color)
    {
        _rank.color = color;
        _name.color = color;
        _score.color = color;
    }

    public void SetUserName(string name)
    {
        _name.text = name.ToUpper();
    }
}
