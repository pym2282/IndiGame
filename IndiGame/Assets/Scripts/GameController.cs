using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameoverCanvas;
    public Text timeText;
    private float time;

    public int rightCount = 0;
    public int leftCount = 0;

    private void Start()
    {
        gameoverCanvas.SetActive(false);
    }
    private void Update()
    {
        time += Time.deltaTime;
        timeText.text = "Time : " + time.ToString("0.00");
    }
    public void RightOverCount()
    {
        rightCount++;
        if (rightCount == 3)
        {
            Gameover();
        }
    }
    public void LeftOverCount()
    {
        leftCount++;
        if (leftCount == 3)
        {
            Gameover();
        }
    }
    private void Gameover()
    {
        gameoverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReStart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
