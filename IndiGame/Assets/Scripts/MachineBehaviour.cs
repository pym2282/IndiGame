﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MachineBehaviour : MonoBehaviour
{
    public MachineState state = MachineState.Normal;
    public ToolType targetToolType;
    public Image hpBarImage;
    public bool isRight;

    private GameController gamecontroller;
    private float _currentHp = 100f;
    private float _recoverStoppedCounter = 0;
    private float _brokenPenaltyCounter = 0;
    private Vector3 cameraOriginPos;

    // Start is called before the first frame update
    private void Start()
    {
        gamecontroller = FindObjectOfType<GameController>();
        _currentHp = MachineManager.Instance.machineMaxHp;
        UpdateHpBar();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Time.timeScale == 0)
            return;
        switch (state)
        {
            case MachineState.Normal:

                break;

            case MachineState.Damaged:
                AddHp(-MachineManager.Instance.machineDamagePerSec * Time.deltaTime);
                break;

            case MachineState.Broken:

                break;

            case MachineState.Recovering:
                if (_brokenPenaltyCounter > 0)
                {
                    _brokenPenaltyCounter -= Time.deltaTime;
                    break;
                }
                AddHp(MachineManager.Instance.machineRecoverPerSec * Time.deltaTime);
                _recoverStoppedCounter = MachineManager.Instance.machineRecoverStopTime;
                break;

            case MachineState.RecoverStopped:
                _recoverStoppedCounter -= Time.deltaTime;
                if (_recoverStoppedCounter <= 0)
                {
                    state = (_currentHp <= 0) ? MachineState.Broken : MachineState.Damaged;
                }
                break;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // 만약 플레이어가 맞는 도구를 들고있다면 state = MachineState.Recovering;
        if (!collider.CompareTag("Player"))
            return;
        if (state != MachineState.Recovering && collider.GetComponent<PlayerController>().tool == targetToolType)
        {
            if (state == MachineState.Broken)
            {
                if (transform.position.x > 0)
                    gamecontroller.rightCount--;
                else
                    gamecontroller.leftCount--;
            }
            state = MachineState.Recovering;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // state == MachineState.Recovering일 경우 
        if (!collider.CompareTag("Player"))
            return;
        if (state == MachineState.Recovering)
        {
            state = MachineState.RecoverStopped;
        }
    }

    public void AddHp(float hp)
    {
        _currentHp = Mathf.Clamp(_currentHp + hp, 0f, MachineManager.Instance.machineMaxHp);
        if (_currentHp <= 0 && state == MachineState.Damaged)
        {
            state = MachineState.Broken;
            BrokenMashine();
            _brokenPenaltyCounter = MachineManager.Instance.machineBrokePenaltyTime;
        }
        else if (_currentHp >= MachineManager.Instance.machineMaxHp)
        {
            state = MachineState.Normal;
        }

        UpdateHpBar();
    }

    void BrokenMashine()
    {

        foreach(Camera camera in FindObjectsOfType<Camera>())
        {
            if (transform.position.x > 0)
            {
                if (camera.gameObject.layer == LayerMask.NameToLayer("Right"))
                {
                    cameraOriginPos = camera.transform.position;
                    gamecontroller.RightOverCount();
                    StartCoroutine(Shake(camera.gameObject, 0.1f, 0.5f));
                }
            }
            else
            {
                if (camera.gameObject.layer == LayerMask.NameToLayer("Left"))
                {
                    cameraOriginPos = camera.transform.position;
                    gamecontroller.LeftOverCount();
                    StartCoroutine(Shake(camera.gameObject, 0.1f, 0.5f));
                }
            }
        }
    }

    private void UpdateHpBar()
    {
        if (state == MachineState.Normal)
        {
            hpBarImage.gameObject.SetActive(false);
            return;
        }
        hpBarImage.gameObject.SetActive(true);
        hpBarImage.fillAmount = _currentHp / MachineManager.Instance.machineMaxHp;
        Color barColor = Color.white;
        if (state == MachineState.Broken)
        {
            hpBarImage.fillAmount = 1f;
            barColor = Color.black;
        }
        else if (_currentHp >= MachineManager.Instance.machineMinGreenHp)
        {
            barColor = Color.green;
        }
        else if (_currentHp >= MachineManager.Instance.machineMinYellowHp)
        {
            barColor = Color.yellow;
        }
        else if (_currentHp >= MachineManager.Instance.machineMinRedHp)
        {
            barColor = Color.red;
        }
        hpBarImage.color = barColor;
    }
    public IEnumerator Shake(GameObject camera, float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            if (Time.timeScale == 0)
                _duration = 0;

            Vector3 shakeVec = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)) * _amount + cameraOriginPos;
            camera.transform.localPosition = shakeVec;

            timer += Time.deltaTime;
            yield return null;
        }
        camera.transform.localPosition = cameraOriginPos;

    }

}
