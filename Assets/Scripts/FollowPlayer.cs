using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _follow;
    public bool picked =false;
    [SerializeField] Text _wineText;
    private void Update()
    {
        if (picked) 
        {
            transform.position = _follow.position + new Vector3(0f, -0.2f, 0f);
        }
    }

    public void PickItem(bool pick) 
    {
        picked = pick;
        if (pick) 
        {
            Action();
        }
        
    }

    private void Action() 
    {
        Destroy(_wineText);
        Debug.Log(_wineText);
    }
}
