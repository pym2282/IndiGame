using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Tool : MonoBehaviour
{
    Rigidbody rb;
    public ToolType toolType = ToolType.None;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(int power)
    {
        rb.isKinematic = false;
        rb.velocity = new Vector3(-power, 0.5f, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground") || other.transform.CompareTag("Player"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Right"))
            {
 
                gameObject.layer = LayerMask.NameToLayer("RightTool");
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Left"))
            {
                Debug.Log("왼쪽");
                gameObject.layer = LayerMask.NameToLayer("LeftTool");
            }
        }
    }
}
