using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flying : MonoBehaviour
{
    private Rigidbody _rb;
    
    private float _flyForce = 10f;
    private float _flyForwardForce = 10f;

    private float _xInput = 0f; // X-Axis Input 
    private float _yInput = 0f; // Y-Axis Input 

    private float _flySpeed = 1f;

    private bool _flyPressed = false;
    private bool _flyHold = false;

    [SerializeField] private Camera _birdCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _xInput = Input.GetAxis("Vertical");
        _yInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
           _flyPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Space)) 
        {
           _flyPressed = false;
           _flyHold = false;
        }

        if (Input.GetKey(KeyCode.Space)) 
        {
            _flyHold = true;
        }


    }

    private void FixedUpdate()
    {
        if (_flyPressed) 
        {   
            _rb.AddForce(Vector3.up * 15f);
            /* var newVelocity = new Vector3(0f , 0f, 1f).normalized * _flySpeed;
            newVelocity = transform.TransformVector(newVelocity);
            _rb.velocity = newVelocity;*/
        }
        else if (_flyHold) 
        {   
            _rb.AddForce(Vector3.up * 5f);
            /* var newVelocity = new Vector3(0f , 0f, 1f).normalized * _flySpeed;
            newVelocity = transform.TransformVector(newVelocity);
            _rb.velocity = newVelocity;*/
        }

        _rb.AddRelativeForce(transform.forward*_xInput*5f);

        //transform.Rotate(new Vector3(_xInput, 0f, 0f));
        
        //_birdCamera.transform.Rotate(new Vector3(0f,_yInput,0f));
        transform.Rotate(new Vector3(0f,_yInput,0f));
    }
}
