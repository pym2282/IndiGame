using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isRight;
    public int speed = 3;
    private Vector3 originPos;

    private void Awake()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if (isRight)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }

        if (transform.position.x >= originPos.x + 2.5f)
        {
            Debug.Log("오른쪽으로 넘었어");
            transform.position = new Vector3(originPos.x + 2.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= originPos.x - 2.5f)
        {
            Debug.Log("이ㅗㄴ쪽으로 넘었어");
            transform.position = new Vector3(originPos.x - 2.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.z >= originPos.z + 2.5f)
        {
            Debug.Log("위로 넘었어");
            transform.position = new Vector3(transform.position.x, transform.position.y, originPos.z + 2.5f);
        }
        if (transform.position.z <= originPos.z - 2.5f)
        {
            Debug.Log("아래로 넘었어");
            transform.position = new Vector3(transform.position.x, transform.position.y, originPos.z - 2.5f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tool"))
        {
            Vector3 playerVec = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            col.transform.parent = transform;
            col.transform.position = playerVec;
        }
        if (col.CompareTag("Throw"))
        {
            if (isRight)
            {

            }
        }
    }
}
