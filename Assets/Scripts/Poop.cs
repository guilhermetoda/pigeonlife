using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private void Update()
    {
        if (Input.GetButtonDown("Poop")) 
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}
