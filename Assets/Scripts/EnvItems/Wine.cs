using UnityEngine;

public class Wine : EnvironmentInteractables
{

    public override void Action(GameObject interact) 
    {
        interact.GetComponent<PlayerMovement>().SetDrunk();
    }

}