using UnityEngine;

public class AILooking : MonoBehaviour
{
    [SerializeField] Transform _posTrigger;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _layerMask;

    private void Update()
    {
        LookingForPigeon();
    }

    private void LookingForPigeon() 
    {
        Collider[] colliders = Physics.OverlapSphere(_posTrigger.position, _radius, _layerMask);
        
        if (colliders.Length > 0) 
        {
            Debug.Log("GET OUT PIGEON!!!");
            colliders[0].GetComponent<Flying>().MakingPigeonFlyUp();
        }

    }
    // Debug the Attack Radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_posTrigger.position, _radius);
    }
}
