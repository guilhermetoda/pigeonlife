using UnityEngine;
using TMPro;

public class AIMode : MonoBehaviour
{
    private string state;
    [SerializeField] TextMeshPro _textMesh;
    

    private void Start()
    {
        state = "Patrol";
        _textMesh.text = state;
    }

    public string GetState() 
    {
        return state;
    }

    public void SetState(string newState) 
    {
        state = newState;
        _textMesh.text = state;
    }
}
