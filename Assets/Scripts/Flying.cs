using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flying : MonoBehaviour
{
    private Rigidbody _rb;
    
    [SerializeField] private float _flyForce = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    private float _flyForwardForce = 10f;

    private float _xInput = 0f; // X-Axis Input 
    private float _yInput = 0f; // Y-Axis Input 

    private float _flySpeed = 0.5f;

    private bool _flyPressed = false;
    private bool _flyHold = false;

    private bool _gliding = false;

    private bool _isGrounded = false; // is the player grounded?

    [SerializeField] private Camera _birdCamera;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance = 3f; // Distance from the character to the ground
    [SerializeField] private LayerMask _groundMask; // layers that represent the ground


    [Header("Camera")]
    [SerializeField] private Transform _camPivot;
    private Transform _camTransform;

    private float _CurrentAngle = 0f;

    public Rigidbody plane;
	public float maxSpeed = 20f;
	public float acceleration = 5f;
	public float speed = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camTransform = _camPivot.GetComponentInChildren<Camera>().transform;
    }

    private void FixedUpdate()
    {
        FlyingPhysics();

        Vector3 targetVelocity = transform.forward * speed;
		plane.AddForceAtPosition (targetVelocity,transform.position);
        if (_isGrounded) 
        {
            MovingPhysics();
        }
    }

    private void Update()
    {
        _isGrounded = GroundCheck();
        ReadInputs();
        UpdateRotations();
        if (!_isGrounded) 
        {
            float changeInVelocity = Input.GetAxis ("Vertical") * acceleration * Time.deltaTime;
		    speed = Mathf.Clamp (speed + changeInVelocity,0f,maxSpeed);
        }
        else 
        {
            speed = 0f;
        }
    }

     // checks for the ground
    private bool GroundCheck()
    {
        // origin of the raycast
        Vector3 originLeft = transform.position + new Vector3(-0.4f, 0.2f, 0f);
        Vector3 originRight = transform.position + new Vector3(0.4f, 0.2f, 0f);
        // perform raycast
        bool raycastLeft = Physics.Raycast(originLeft, -Vector3.up, _groundCheckDistance, _groundMask);
        bool raycastRight = Physics.Raycast(originRight, -Vector3.up, _groundCheckDistance, _groundMask);
        if(raycastLeft || raycastRight)
        {
            // debug raycast hit
            Debug.DrawRay(originLeft, -Vector3.up * _groundCheckDistance, Color.green);
            Debug.DrawRay(originRight, -Vector3.up * _groundCheckDistance, Color.green);
            // return success
            //Debug.Log("GROUND");
            return true;
        }

        // raycast failed
        // debug raycast miss
        Debug.DrawRay(originLeft, -Vector3.up * _groundCheckDistance, Color.red);
        Debug.DrawRay(originRight, -Vector3.up * _groundCheckDistance, Color.red);
        //Debug.Log("NOT GROUND");
        // return failure
        return false;
    }


    // Update Camera Rotation
    private void UpdateRotations()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float leftTrigger = Input.GetAxis("LeftTrigger");
        float rightTrigger = Input.GetAxis("RightTrigger");


        
        // // Rotate around our y-axis
        // transform.Rotate(new Vector3(0f,_yInput,0f));
        // if (_yInput == 0) 
        // {
        //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(45f*leftTrigger,transform.rotation.y,transform.rotation.z), Time.deltaTime*speed);
        // }
        
        //transform.Rotate(0f, mouseX, 0f);

        transform.Rotate(new Vector3(0f,_yInput,0f));
        // increase the angle using the Input from the Mouse
        _CurrentAngle += mouseY;
        // Checks if the angle is between 90f and -90f
        if (_CurrentAngle < 90f && _CurrentAngle > -90f) 
        {
            // Rotates only with the Angle is inside of the allowed range.
            _camPivot.Rotate(-mouseY, 0, 0);
        }
        else 
        {
            // if the angle is higher or lower, clamp the angle!
            _CurrentAngle = Mathf.Clamp(_CurrentAngle, -90f, 90f);
        }

    }

    private void ReadInputs()
    {
        _xInput = Input.GetAxis("Vertical");
        _yInput = Input.GetAxis("Horizontal");

        
         if (Input.GetButtonDown("Jump")) 
        {
           _flyPressed = true;
        }

        if (Input.GetButtonUp("Jump")) 
        {
           _flyPressed = false;
           _flyHold = false;
        }

        if (Input.GetButton("Jump")) 
        {
            _flyHold = true;
        }

        if (Input.GetKey(KeyCode.G))
        {
            _gliding = true;
        }

        if (Input.GetKeyUp(KeyCode.G)) 
        {
            _gliding = false;
        }
    }

    private void MovingPhysics() 
    {
        Vector3 vec;
        vec.x = _xInput;
        vec.z = _yInput;
        vec = Camera.main.transform.TransformDirection(vec.x, 0f, vec.z);  // Make relative to main camera
        vec.y = 0;  // optional for no y movement.
        Vector3 force = vec.normalized * 10f;
        _rb.AddForce(force);

/*      var newVelocity = transform.forward * _flySpeed*_xInput;
        newVelocity = transform.TransformVector(newVelocity);
        _rb.velocity = newVelocity; */
    }


    private void FlyingPhysics() 
    {
        if (_flyPressed) 
        {   
            _rb.AddForce(Vector3.up * _flyForce, ForceMode.Impulse);
            /* var newVelocity = new Vector3(0f , 0f, 1f).normalized * _flySpeed;
            newVelocity = transform.TransformVector(newVelocity);
            _rb.velocity = newVelocity;*/
            
        }
        else if (_flyHold) 
        {   
            _rb.AddForce(Vector3.up * 2f, ForceMode.Acceleration);
            //_rb.AddForce(transform.forward*5f);
            /* var newVelocity = new Vector3(0f , 0f, 1f).normalized * _flySpeed;
            newVelocity = transform.TransformVector(newVelocity);
            _rb.velocity = newVelocity;*/
        }

        if (_gliding) 
        {
            _rb.AddForce(Vector3.forward * _flySpeed*2f, ForceMode.Acceleration);
        }
        
        //_rb.AddForce(transform.forward*_xInput*5f);


        
        
        if (_xInput <= 0) {
            
            //transform.Rotate(new Vector3(_xInput, 0f, 0f));
        }
        

        //_birdCamera.transform.Rotate(new Vector3(1.26f,0f,0f));
    }

}
