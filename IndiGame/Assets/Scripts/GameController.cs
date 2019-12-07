﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Exploder.Utils;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameoverCanvas;
    public Transform[] playerTr;

    public Text timeText;
    public GameObject[] ground;
    private float time;
    public AudioClip[] audioClip;
    public AudioSource audioManager;

    public GameObject[] destroyObj;
    public RankingUIBehaviour rankingCanvas;
    public Rigidbody[] playerRb;
    public float Time { get; private set; }

    public int rightCount = 0;
    public int leftCount = 0;
    public int gameOverCount = 3;
    private bool isGameover = false;

    public GameObject dangerImage;
    private void Start()
    {
        gameoverCanvas.SetActive(false); ;
    }
    private void Update()
    {
        if (!isGameover)
        Time += UnityEngine.Time.deltaTime;
        timeText.text = "Time : " + Time.ToString("0.00");
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
        timeText.gameObject.SetActive(false);
    }

    public void ShowRanking()
    {
        gameoverCanvas.SetActive(false);
        rankingCanvas.gameObject.SetActive(true);
        rankingCanvas.Init();
    }

    public void ReStart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnityEngine.Time.timeScale = 1;
    }

    void ExplodeTile(int num)
    {
        if (num == 1)
        {
            ExploderSingleton.Instance.ExplodeObject(ground[1]);
            StartCoroutine(FallDelay(playerTr[1].gameObject));
        }
        else
        {
            ExploderSingleton.Instance.ExplodeObject(ground[0]);
            StartCoroutine(FallDelay(playerTr[0].gameObject));
        }
    }
    IEnumerator Deley(int num)
    {
        GameObject danPos = Instantiate(dangerImage);
        //z카메라
        if (num == 1)
        {
            danPos.transform.position = new Vector3(playerTr[1].position.x, playerTr[1].position.y + 2, playerTr[1].position.z);
        }
        else
        {
            danPos.transform.position = new Vector3(playerTr[0].position.x, playerTr[0].position.y + 2, playerTr[0].position.z);
        }
        playerTr[1].GetComponent<PlayerController>().isGameover = true;
        playerTr[0].GetComponent<PlayerController>().isGameover = true;
        FindObjectOfType<Wall>().speed = 0;
        Destroy(danPos, 3);
        foreach(GameObject go in destroyObj)
        {
            go.gameObject.SetActive(false);
        }
        GetComponent<AudioSource>().clip = audioClip[0];
        GetComponent<AudioSource>().Play();
        ExplodeTile(num);
        yield return new WaitForSeconds(5f);
        Gameover(num);
    }
    IEnumerator FallDelay(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        audioManager.Play();
    }
}
