using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    private Vector3 originPos;
    private Vector3 additionalPos;

    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        originPos = new Vector3(player.transform.position.x, 3, player.transform.position.z - 4.5f);
        transform.position = originPos + additionalPos;
    }

    void CameraShake()
    {
    }

    public void Shake(float amount, float duration)
    {
        StartCoroutine(InnerShake(amount, duration));
    }

    private IEnumerator InnerShake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            if (Time.timeScale == 0)
                _duration = 0;

            Vector3 shakeVec = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)) * _amount;
            additionalPos = shakeVec;

            timer += Time.deltaTime;
            yield return null;
        }
        additionalPos = Vector3.zero;
    }

}
