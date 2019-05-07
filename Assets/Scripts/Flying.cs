using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flying : MonoBehaviour
{
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            _rb.AddForce(Vector3.up*10f);
        }

        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            _rb.AddForce(Vector3.forward * 10f);
        }
    }
}
