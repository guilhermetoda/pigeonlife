using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flying : MonoBehaviour
{
    private Rigidbody _rb;
    

    [SerializeField] private Transform _triggerPos;
    // Range of X-Axis of the overlap box
    [SerializeField] private float _triggerBoxX;
    // Range of Y-Axis of the overlap box
    [SerializeField] private float _triggerBoxY;
    // Range of Z-Axis of the overlap box
    [SerializeField] private float _triggerBoxZ;

    [SerializeField] private LayerMask _itemLayers;


    [SerializeField] private float _flyForce = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    private float _flyForwardForce = 10f;

    private float _xInput = 0f; // X-Axis Input 
    private float _yInput = 0f; // Y-Axis Input 
    private float _leftTrigger = 0f; // Left trigger axis

    private float _flySpeed = 0.5f;

    private bool _flyPressed = false;
    private bool _flyHold = false;

    private bool _gliding = false;

    private float _gravity = 9.8f;
    [SerializeField] private float _gravityScale = 9.8f;
    private float _earthGravity = 9.8f;

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

    public bool _hasItem = false;

    private GameObject _holdingObject;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camTransform = _camPivot.GetComponentInChildren<Camera>().transform;
    }

    private void ApplyGravity() 
    {
       
        //Vector3 gravity = -Vector3.up * _gravity * Time.deltaTime;
        Vector3 gravity = -Vector3.up * _gravity;
        _rb.AddForce(gravity);
        Debug.Log("Gravity Applied: "+gravity);
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
        ApplyGravity();
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
            _birdCamera.fieldOfView = 60f;
        }
        else 
        {
            _birdCamera.fieldOfView = 30f;
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
        _leftTrigger = Input.GetAxis("LeftTrigger");

        Debug.Log("Left trigger input:" +_leftTrigger);

        if (_leftTrigger > 0) 
        {
            _gravity = _earthGravity * _leftTrigger * _gravityScale;

            
        } 
        else 
        {
            _gravity = _earthGravity;
        }
        
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

        if (Input.GetButtonDown("Fire3")) 
        {
            if (_hasItem) 
            {
                //Drop Item
                DropItem();
            }
            else 
            {
                Debug.Log("Pickup");
                PickupItem();
            }
            
        }
    }

    private void DropItem() 
    {
        Debug.Log("AAAA");
        _holdingObject.GetComponent<FollowPlayer>().PickItem(false);
        _holdingObject.GetComponent<Rigidbody>().detectCollisions = true;
        _holdingObject.GetComponent<Rigidbody>().useGravity = true;
        _holdingObject.gameObject.transform.SetParent(null);
        _holdingObject = null;
        _hasItem = false;

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
        Debug.Log("Dustino");

/*      var newVelocity = transform.forward * _flySpeed*_xInput;
        newVelocity = transform.TransformVector(newVelocity);
        _rb.velocity = newVelocity; */
    }

    public void MakingPigeonFlyUp() 
    {
        _rb.AddForce(Vector3.up * 1f, ForceMode.Impulse);
        Debug.Log("HELLO");
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

    private void PickupItem(bool drop = false) 
    {
        Collider[] colliders = Physics.OverlapBox(_triggerPos.position, new Vector3(_triggerBoxX, _triggerBoxY, _triggerBoxZ), Quaternion.identity, _itemLayers);

        if (colliders.Length > 0) 
        {
            
            for (int i=0; i < colliders.Length; i++) 
            {
                if (colliders[i].CompareTag("Item")) 
                {
                    colliders[i].gameObject.transform.SetParent(transform);
                    colliders[i].gameObject.transform.position = _triggerPos.position;
                    if (!drop) 
                    {
                        colliders[i].gameObject.GetComponent<FollowPlayer>().PickItem(true);
                        colliders[i].gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                        colliders[i].gameObject.GetComponent<Rigidbody>().useGravity = false;
                        _hasItem = true;
                        _holdingObject = colliders[i].gameObject;
                    }
                    
                    break;
                }
                else if (colliders[i].CompareTag("EnvItem")) 
                {
                    colliders[i].GetComponent<EnvironmentInteractables>().Action(this.gameObject);
                }
            }
        }

    }

    // Debug the Attack Radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_triggerPos.position, new Vector3(_triggerBoxX, _triggerBoxY, _triggerBoxZ));
    }


}
