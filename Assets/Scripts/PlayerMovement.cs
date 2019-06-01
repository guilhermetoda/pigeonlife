using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private float _zInput = 0f; // X-Axis Input 
    private float _yInput = 0f; // Y-Axis Input 
    private bool _jumpPressed;
    private bool _isGrounded;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance = 3f; // Distance from the character to the ground
    [SerializeField] private LayerMask _groundMask; // layers that represent the ground

    [SerializeField] private Transform _triggerPos;
    // Range of X-Axis of the overlap box
    [SerializeField] private float _triggerBoxX;
    // Range of Y-Axis of the overlap box
    [SerializeField] private float _triggerBoxY;
    // Range of Z-Axis of the overlap box
    [SerializeField] private float _triggerBoxZ;

    [SerializeField] private LayerMask _itemLayers;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void ReadInputs()
    {
        _zInput = Input.GetAxis("Vertical");
        _yInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Fire3")) 
        {
            PickupItem();
        }
    }

    private void Update()
    {
        _isGrounded = GroundCheck();
        if (_isGrounded) 
        {
            if (Input.GetButtonDown("Jump")) 
            {
                _jumpPressed = true;
            }
        }
        else 
        {
            _jumpPressed = false;
        }
        ReadInputs();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement() 
    {
        var newVel = new Vector3(0f, 0f, _zInput) * 0.5f;
        newVel = transform.TransformVector(newVel);
        
        newVel.y = _jumpPressed ? 1f : _rb.velocity.y;
        _rb.velocity = newVel;
        transform.Rotate(0f, _yInput, 0f);
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

    // Debug the Attack Radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_triggerPos.position, new Vector3(_triggerBoxX, _triggerBoxY, _triggerBoxZ));
    }

    private void PickupItem(bool drop = false) 
    {
        Collider[] colliders = Physics.OverlapBox(_triggerPos.position, new Vector3(_triggerBoxX, _triggerBoxY, _triggerBoxZ), Quaternion.identity, _itemLayers);

        if (colliders.Length > 0) 
        {
            
            for (int i=0; i < colliders.Length; i++) 
            {
                if (colliders[i].CompareTag("EnvItem")) 
                {
                    colliders[i].GetComponent<EnvironmentInteractables>().Action(this.gameObject);
                }
            }
        }

    }

    
}
