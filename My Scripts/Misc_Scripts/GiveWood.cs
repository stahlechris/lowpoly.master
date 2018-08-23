using UnityEngine;
using LowPoly.Weapon;

public class GiveWood : MonoBehaviour 
{
    public GameObject wood;//log_regular

    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponent<Weapon>(); //call the players animator system, and reference its animation state to see == "Attack"
        if(weapon != null && weapon.Name == "Axe") 
        {//instantiate some wood relative to the players position
            //Instantiate(wood)
        }
    }
}
