using UnityEngine;

public class WaterHydrant : EnvironmentInteractables
{   
    [SerializeField] GameObject _waterPrefab;
    [SerializeField] GameObject[] _Guys;
    

    public override void Action(GameObject interact) 
    {
        
        Quaternion newRotation = Quaternion.Euler(0, 90, 0);
        Instantiate(_waterPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity * newRotation);
        Vector3 rotation = new Vector3(0f, 90f, 0f);
        for (int i=0; i< _Guys.Length; i++) 
        {
            _Guys[i].gameObject.transform.Rotate(0f, -90f, 0f, Space.Self);
            _Guys[i].gameObject.transform.position += new Vector3(-3f, 0f,0f);
            
        }

    }
}
