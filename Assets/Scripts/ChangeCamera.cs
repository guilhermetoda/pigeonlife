using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    
    [SerializeField] GameObject _camera1;
    [SerializeField] GameObject _camera2;

    private bool selectedCamera  = false;

    private void Update()
    {
        if (Input.GetButtonDown("ChangeCamera")) 
        {
            if (selectedCamera) 
            {
                _camera2.gameObject.SetActive(false);
                _camera1.gameObject.SetActive(true);
                selectedCamera = false;
            }
            else 
            {
                _camera2.gameObject.SetActive(true);
                _camera1.gameObject.SetActive(false);
                selectedCamera = true;
            }
        }
        
    }

}
