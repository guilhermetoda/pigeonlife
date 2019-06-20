using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class DrunkEffect : MonoBehaviour
{
    [SerializeField] 
    private PostProcessProfile _ppp;
    
    private LensDistortion _ld;
    private bool _up = true;

    private void Awake()
    {
        _ld = _ppp.GetSetting<LensDistortion>();    
    }

    private void Update()
    {
        float randomFactor = Random.Range(0.2f, 1f);
        if (_ld.intensity >= 100) 
        {
            _up = false;
        }
        else if (_ld.intensity <= -100) 
        {
            _up = true;
        }

        if (_up) 
        {
            _ld.intensity.value++;
            //_ld.intensity.value = _ld.intensity.value *randomFactor;

        }
        else 
        {
            _ld.intensity.value--;
            //_ld.intensity.value = _ld.intensity.value *randomFactor;
        }
    }

    private void ChangeLensDistortion() 
    {
        
    }
}


