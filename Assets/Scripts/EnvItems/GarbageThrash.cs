using UnityEngine;
using UnityEngine.UI;

public class GarbageThrash : EnvironmentInteractables
{

    public override void Action(GameObject interact) 
    {
        interact.GetComponent<Poop>().SetDiarrhea();
        
    }

}
