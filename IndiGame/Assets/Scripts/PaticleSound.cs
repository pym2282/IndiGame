using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticleSound : MonoBehaviour
{
    private AudioSource audioSouce;
    private void Awake()
    {
        audioSouce = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        audioSouce.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
