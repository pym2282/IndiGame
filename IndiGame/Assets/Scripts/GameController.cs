using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Exploder.Utils;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameoverCanvas;
    public Rigidbody[] playerRb;

    public Text timeText;
    public GameObject[] ground;
    private float time;

    public int rightCount = 0;
    public int leftCount = 0;
    public int gameOverCount = 3;
    private bool isGameover = false;
    private void Start()
    {
        gameoverCanvas.SetActive(false); ;
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(!isGameover)
        timeText.text = "Time : " + time.ToString("0.00");
    }
    public void RightOverCount()
    {
        if (isGameover)
            return;
        rightCount++;
        if (rightCount >= gameOverCount)
        {
            isGameover = true;
            StartCoroutine(Deley(0));
        }
    }
    public void LeftOverCount()
    {
        if (isGameover)
            return;
        leftCount++;
        if (leftCount >= gameOverCount)
        {
            isGameover = true;
            StartCoroutine(Deley(1));
        }
    }
    private void Gameover(int num)
    {
        gameoverCanvas.SetActive(true);
    }

    public void ReStart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    void ExplodeTile(int num)
    {
        if(num == 1)
        {
            ExploderSingleton.Instance.ExplodeObject(ground[0]);
            playerRb[0].isKinematic = false;
        }
        else
        {
            ExploderSingleton.Instance.ExplodeObject(ground[1]);
            playerRb[1].isKinematic = false;
        }
    }
    IEnumerator Deley(int num)
    {
        ExplodeTile(num);
        yield return new WaitForSeconds(2.5f);
        Gameover(num);
    }
}
