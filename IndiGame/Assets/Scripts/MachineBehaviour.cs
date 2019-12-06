using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MachineBehaviour : MonoBehaviour
{
    public MachineState state = MachineState.Normal;
    public Image hpBarImage;

    private float _currentHp = 100f;
    private float _recoverStoppedCounter = 0;
    private float _brokenPenaltyCounter = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _currentHp = MachineManager.Instance.machineMaxHp;
        UpdateHpBar();
    }

    // Update is called once per frame
    private void Update()
    {
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

    private void OnTriggerEnter(Collider collider)
    {
        // 만약 플레이어가 맞는 도구를 들고있다면 state = MachineState.Recovering;
    }

    private void OnTriggerExit(Collider collider)
    {
        // state == MachineState.Recovering일 경우
    }

    public void AddHp(float hp)
    {
        _currentHp = Mathf.Clamp(_currentHp + hp, 0f, MachineManager.Instance.machineMaxHp);
        if (_currentHp <= 0 && state == MachineState.Damaged)
        {
            state = MachineState.Broken;
            _brokenPenaltyCounter = MachineManager.Instance.machineBrokePenaltyTime;
        }
        else if (_currentHp >= MachineManager.Instance.machineMaxHp)
        {
            state = MachineState.Normal;
        }

        UpdateHpBar();
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
}
