using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornBag : EnvironmentInteractables
{

    public override void Action(GameObject interact) 
    {
        interact.GetComponent<PlayerMovement>().SetItem();
        Destroy(this.gameObject);
    }

}
