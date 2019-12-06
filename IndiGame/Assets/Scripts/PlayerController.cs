﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerController : MonoBehaviour
{
    public bool isRight;
    public int speed = 3;
    private Vector3 originPos;
    private Vector3 originScale;
    public Camera camera;


    public ToolType tool;
    

    private void Awake()
    {
        originPos = transform.position;
        originScale = transform.localScale;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (isRight)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localScale = new Vector3(originScale.x, originScale.y, originScale.z);
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
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
                transform.localScale = new Vector3(originScale.x, originScale.y, originScale.z);
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
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

        if (col.CompareTag("Throw"))
        {
            if (transform.childCount == 0)
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
            }
        }
        else if (col.CompareTag("Tool")) // 도구를 주웠을 때
        {
            col.transform.parent = transform;
            col.transform.position = playerVec(1);
            col.GetComponent<Rigidbody>().isKinematic = true;
            tool = col.GetComponent<Tool>().toolType;
        }
    }
}
