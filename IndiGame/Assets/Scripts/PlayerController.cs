using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Enums;

public class PlayerController : MonoBehaviour
{
    public bool isRight;
    public int speed = 3;
    public Camera camera;
    public ToolType tool;
    public bool isGameover = false;
    public AudioSource playerAudio;

    private Vector3 originPos;
    private SpriteRenderer _sprite;

    

    private void Awake()
    {
        originPos = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.timeScale == 0 || isGameover)
            return;
        Move();

    }

    private void Move()
    {
        if (isRight)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _sprite.flipX = false;
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _sprite.flipX = true;
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
                _sprite.flipX = false;
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _sprite.flipX = true;
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
            transform.position = new Vector3(originPos.x + 2.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= originPos.x - 2.5f)
        {
            transform.position = new Vector3(originPos.x - 2.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.z >= originPos.z + 2.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, originPos.z + 2.5f);
        }
        if (transform.position.z <= originPos.z - 2.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, originPos.z - 2.5f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Vector3 playerVec(int i)
        {
            return new Vector3(transform.position.x, transform.position.y + i, transform.position.z);
        }

        if (col.CompareTag("Throw")) // 던지기 영역에 갔을때
        {
            if (tool == ToolType.None)
                return;
            Transform target = transform.GetChild(0);
            if (target.gameObject.layer == LayerMask.NameToLayer("RightTool")||
                target.gameObject.layer == LayerMask.NameToLayer("LeftTool"))
            {
                target.parent = transform.parent;
                if (isRight)
                {
                    target.GetComponent<Tool>().Throw(8);//8의 파워만큼 도구를 던진다
                }
                else
                {
                    target.GetComponent<Tool>().Throw(-8);
                }
                tool = ToolType.None;
                playerAudio.Play();
            }
        }
        else if (col.CompareTag("Tool")) // 도구를 주웠을 때
        {
            if (tool == ToolType.None)
            {
                col.transform.rotation = new Quaternion(0,0,0,0);
                col.transform.parent = transform;
                col.transform.position = playerVec(1);
                col.GetComponent<Rigidbody>().isKinematic = true;
                tool = col.GetComponent<Tool>().toolType;
            }
        }
    }
}
