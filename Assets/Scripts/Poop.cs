using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poop : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabDiarrhea;
    [SerializeField] private float _diarrheaTime = 10f;

    [SerializeField] private GameObject _diarrheaSmokePrefab;

    [SerializeField] Text _diarrheaText;
    private GameObject _diarrheaEffect;

    private float _counter = 0f;
    private float _poopBar = 0.5f;
    private bool _diarrhea = false;

    
    private void Update()
    {
        if (Input.GetButtonDown("Poop")) 
        {
            ShootPoop();
        }
        if (_counter > _diarrheaTime) 
        {
            _counter = 0f;
            Destroy(_diarrheaEffect);
            _diarrhea = false;
        }
        else 
        {
            _counter +=Time.deltaTime;
        }
    }

    public void IncreasePoopBar(float poop) 
    {
        _poopBar += poop;
    }

    private void ShootPoop() 
    {
        if (_poopBar > 0) 
        {
            if (_diarrhea) 
            {
                Instantiate(_prefabDiarrhea, transform.position, Quaternion.identity);

            }
            else 
            {
                Instantiate(_prefab, transform.position, Quaternion.identity);
            }
            
            _poopBar -= 0.1f;
        }
    }

    public void SetDiarrhea() 
    {
        _diarrhea = true;
        Destroy(_diarrheaText);
        _diarrheaEffect = Instantiate(_diarrheaSmokePrefab, transform.position, Quaternion.identity);
        _diarrheaEffect.transform.SetParent(transform);
        
    }
}
