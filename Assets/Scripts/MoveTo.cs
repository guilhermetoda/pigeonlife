using UnityEngine;

public class MoveTo : MonoBehaviour
{   
     // Gameobject to control the "trigger volume" using Overlap Sphere
    [SerializeField] private Transform _triggerPos;
    [SerializeField] private float _sphereRadius;
    [SerializeField] private LayerMask _platformLayers;

    [SerializeField] private float _moveToSpeed = 5f;
    [SerializeField] private float _stopMovementRadius = 0.5f;
    
    private bool _isMovingTo = false;
    private Transform _platformToMove;
    private Collider _nearestCollider;

    private void Update()
    {
    
        if (_isMovingTo) 
        {
            Debug.Log("CADE");
            if (Vector3.Distance(transform.position, _platformToMove.position) < _stopMovementRadius) 
            {
                _isMovingTo = false;
                _platformToMove = null;
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, _platformToMove.position, _moveToSpeed * Time.deltaTime);
            }
            
        }
        else 
        {
            Collider[] colliders = Physics.OverlapSphere(_triggerPos.position, _sphereRadius, _platformLayers);
            Debug.Log("COLLISION" +colliders.Length);
            if (colliders.Length > 0) 
            {
                _nearestCollider = GetNearCollider(colliders);
                //nearestCollider.transform.GetChild(0).gameObject.SetActive(true);        
            }   

            if ( Input.GetButton("Action")) 
            {
                Debug.Log("AAA");
                _platformToMove = _nearestCollider.transform;
                _isMovingTo = true;
                
            }
        }
    }

    private Collider GetNearCollider(Collider[] colliders) 
    {
        float closeDistance = int.MaxValue;
        Collider closerCollider = null;
        for (int i = 0; i < colliders.Length; i++) 
        {
            if (closerCollider == null) 
            {
                closerCollider = colliders[i];
            }

            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closeDistance) 
            {
                closerCollider = colliders[i];
                closeDistance = distance;
            }
        }

        return closerCollider;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_triggerPos.position, _sphereRadius);
    }
}
