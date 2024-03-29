﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MachineBehaviour : MonoBehaviour
{
    private MachineState _state = MachineState.Normal;

    public PlayableArea areaType;
    public ToolType targetToolType;
    public Image hpBarImage;
    public bool isRight;
    public GameObject particle;
    public GameObject smokeParticle;
    public GameObject shockParticle;
    public GameObject clockObj;

    private GameController gamecontroller;
    private SpriteRenderer mashineColor;
    private float _currentHp = 100f;
    private float _recoverStoppedCounter = 0;
    private float _brokenPenaltyCounter = 0;
    private AudioSource mashineAudio;
    public AudioClip[] audioClip;
    public MachineState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == MachineState.Broken && _state != value)
            {
                if (areaType == PlayableArea.Left)
                {
                    gamecontroller.LeftOverCount();
                }
                else
                {
                    gamecontroller.RightOverCount();
                }
            }
            else if (_state == MachineState.Broken && _state != value)
            {
                if (areaType == PlayableArea.Left)
                {
                    gamecontroller.leftCount--;
                }
                else
                {
                    gamecontroller.rightCount--;
                }
            }


            if (value == MachineState.Broken && _state == MachineState.Damaged)
            {
                BrokenMashine();
            }
            else if (value == MachineState.Damaged && _state == MachineState.Normal)
            {
                StartCoroutine(Paricle(shockParticle));
            }
            else if (value == MachineState.Normal && smokeParticle.activeInHierarchy)
            {
                smokeParticle.SetActive(false);
                mashineColor.color = Color.white;
                StartCoroutine(Deley());
            }
            _state = value;
        }
    }

    IEnumerator Deley()
    {
        mashineAudio.clip = audioClip[1];
        mashineAudio.Play();
        yield return new WaitForSeconds(0.5f);
        mashineAudio.clip = audioClip[0];
    }

    // Start is called before the first frame update
    private void Start()
    {
        gamecontroller = FindObjectOfType<GameController>();
        _currentHp = MachineManager.Instance.machineMaxHp;
        UpdateHpBar();
        particle.SetActive(false);
        shockParticle.SetActive(false);
        smokeParticle.SetActive(false);
        mashineColor = GetComponent<SpriteRenderer>();
        mashineAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Time.timeScale == 0)
            return;
        switch (State)
        {
            case MachineState.Normal:

                break;

            case MachineState.Damaged:
                AddHp(-MachineManager.Instance.machineDamagePerSec * Time.deltaTime);
                smokeParticle.SetActive(true);
                break;

            case MachineState.Broken:
                if (mashineAudio.isPlaying)
                    mashineAudio.Pause();

                break;

            case MachineState.Recovering:
                _recoverStoppedCounter = MachineManager.Instance.machineRecoverStopTime;
                if (_currentHp < MachineManager.Instance.machineMaxHp && !mashineAudio.isPlaying)
                {
                    mashineAudio.Play();
                    if (!clockObj.activeInHierarchy)
                    {
                        clockObj.SetActive(true);
                    }
                }
                if (_brokenPenaltyCounter > 0)
                {
                    _brokenPenaltyCounter -= Time.deltaTime;
                    break;
                }
                AddHp(MachineManager.Instance.machineRecoverPerSec * Time.deltaTime);

                if (clockObj.activeInHierarchy)
                    clockObj.SetActive(false);

                break;

            case MachineState.RecoverStopped:
                _recoverStoppedCounter -= Time.deltaTime;
                if (_recoverStoppedCounter <= 0)
                {
                    if(_currentHp <= 0)
                    {
                        State = MachineState.Broken;
                    }
                    else
                    State = MachineState.Damaged;
                }
                break;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // 만약 플레이어가 맞는 도구를 들고있다면 state = MachineState.Recovering;
        if (!collider.CompareTag("Player"))
            return;
        ToolType toolType = collider.GetComponent<PlayerController>().tool;
        if (State != MachineState.Recovering && toolType == targetToolType)
        {
            if(State == MachineState.Broken)
            {
                if (_brokenPenaltyCounter <= 0)
                    mashineColor.color = Color.white;
            }
            State = MachineState.Recovering;
        }
        else if (State == MachineState.Recovering && toolType != targetToolType)
        {
            State = MachineState.RecoverStopped;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // state == MachineState.Recovering일 경우 
        if (mashineAudio.isPlaying)
            mashineAudio.Pause();
        if (clockObj.activeInHierarchy)
            clockObj.SetActive(false);
        if (!collider.CompareTag("Player"))
            return;
        if (State == MachineState.Recovering)
        {
            State = MachineState.RecoverStopped;
        }
    }

    public void AddHp(float hp)
    {
        _currentHp = Mathf.Clamp(_currentHp + hp, 0f, MachineManager.Instance.machineMaxHp);
        if (_currentHp <= 0 && State == MachineState.Damaged )
        {
            State = MachineState.Broken;
            _brokenPenaltyCounter = MachineManager.Instance.machineBrokePenaltyTime;
        }
        else if (_currentHp >= MachineManager.Instance.machineMaxHp)
        {
            State = MachineState.Normal;
        }

        UpdateHpBar();
    }

    void BrokenMashine() // 기계가 부셔질 때
    {
        StartCoroutine(Paricle(particle));
        mashineColor.color = Color.white * 0.4f;
        foreach (CameraMove camera in FindObjectsOfType<CameraMove>())
        {
            if (transform.position.x > 0)
            {
                if (camera.gameObject.layer == LayerMask.NameToLayer("Right"))
                {
                    camera.Shake(0.1f, 0.5f);
                }
            }
            else
            {
                if (camera.gameObject.layer == LayerMask.NameToLayer("Left"))
                {
                    camera.Shake(0.1f, 0.5f);
                }
            }
        }
    }

    IEnumerator Paricle(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(1);
        obj.SetActive(false);
    }

    private void UpdateHpBar()
    {
        if (State == MachineState.Normal)
        {
            hpBarImage.gameObject.SetActive(false);
            return;
        }
        hpBarImage.gameObject.SetActive(true);
        hpBarImage.fillAmount = _currentHp / MachineManager.Instance.machineMaxHp;
        Color barColor = Color.white;
        if (State == MachineState.Broken)
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
}
