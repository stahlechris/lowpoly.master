using UnityEngine;
using System.Collections;

public class CullingSystem : MonoBehaviour 
{
    [SerializeField] GameObject[] areas;

    public void InformOnTrigger(bool status, string areaName)
    {

        if(status)//1, entered trigger
        {
            Debug.Log("Culling system heard player entered " + areaName + " disabling areas out of view");
            DetermineAreasToActivate(status, areaName);

        }
        else //0, left trigger
        {
            Debug.Log("Culling system heard player left " + areaName + " enabling areas");
            DetermineAreasToActivate(status, areaName);
        }
        
    }
    /*Optimization => It seems the sword cave, flower garden and carls cave should always be off until X distance away.

    //1. Flower garden(starts off) only activates by passing into the forest or the enabling rock.
    //   Entering other areas deactivates the flower garden.
    //2. Sword cave (starts off) only activates by passing into a collider nearby.
         Entering other areas deactivates sword cave.

    */



    void DetermineAreasToActivate(bool status, string areaName)
    {
        if(areaName.Equals("Ed's Enclosure"))
        {
            //if I entered the trigger, turn them off
            if (status)
            {
                //If you are here, there is no way you can see the flower garden or the sword cave.
                areas[1].SetActive(false);
                areas[8].SetActive(false);
            }
            //if i left the trigger, turn them on
            else
            {
            }

        }
        else if(areaName.Equals("Flower Garden"))
        {
            //If you are here, there is no way you can see the sword cave or the enclosure
            //or carls mushroom cave

            if(status)
            {
                areas[8].SetActive(false);
                areas[0].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[0].SetActive(true);
                areas[11].SetActive(false);
            }
        }
        else if(areaName.Equals("Pers' Personal Garden"))
        {
            //No way you can see carl or sword cave
            if (status)
            {
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
            }
        }
        else if (areaName.Equals("World's Edge"))
        {
            //No way you can see carls, sword, flower garden, eds house area
            if (status)
            {
                areas[1].SetActive(false);
                areas[11].SetActive(false);
                areas[8].SetActive(false);
                areas[13].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
                areas[13].SetActive(true);
            }
            
        }
        else if (areaName.Equals("Dead Forest"))
        {
            //Cant see sword, carl, flower
            if (status)
            {
                areas[3].SetActive(false);
                areas[1].SetActive(false);
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[3].SetActive(true);
                areas[11].SetActive(true);
            }
            
        }
        else if (areaName.Equals("The Forest")) //Forest is a special case for glower garden activation!
        {
            //sword, carl, enclosure
            if (status)
            {
                //If you enter the forest, activate the nearby flower garden.
                areas[1].SetActive(true);

                areas[8].SetActive(false);
                areas[11].SetActive(false);
                areas[0].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
                areas[0].SetActive(true);
            }
        }
        else if (areaName.Equals("The Temple"))
        {
            //carl sword
            if (status)
            {
                areas[1].SetActive(false);
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
            }
        }
        else if (areaName.Equals("Ip's Campsite"))
        {
            //sword, carl, worlds edge
            if (status)
            {
                areas[3].SetActive(false); //water first
                areas[1].SetActive(false);
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
                areas[3].SetActive(true);
            }
        }
        else if (areaName.Equals("Forgotten Cave"))
        {
            //everything
            if (status)
            {
                areas[3].SetActive(false); //water first
                areas[1].SetActive(false);
                areas[0].SetActive(false);
                areas[2].SetActive(false);
                areas[4].SetActive(false);
                areas[5].SetActive(false);
                areas[9].SetActive(false);
                areas[13].SetActive(false);
            }
            else
            {
                areas[3].SetActive(true); //water first
                areas[0].SetActive(true);
                areas[2].SetActive(true);
                areas[4].SetActive(true);
                areas[5].SetActive(true);
                areas[9].SetActive(true);
                areas[13].SetActive(true);
            }
        }
        else if (areaName.Equals("The Stoned Henge"))
        {
            //flower, carl, sword, worlds edge
            if (status)
            {
                areas[3].SetActive(false); //water first
                areas[1].SetActive(false);
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[3].SetActive(true);
                areas[11].SetActive(true);
            }
        }
        else if (areaName.Equals("Top o' the world!"))
        {
            //sword, carl
            if (status)
            {
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
            }
        }
        else if (areaName.Equals("Carls Mushroomy Cave"))
        {
            //flower, worlds edge, forest, stonehenge, Ips site, sword
            if (status)
            {
                areas[3].SetActive(false);  //water first
                areas[1].SetActive(false);
                areas[4].SetActive(false);
                areas[5].SetActive(false);
                areas[9].SetActive(false);
                areas[7].SetActive(false);
            }
            else
            {
                areas[3].SetActive(true);  //water first
                areas[4].SetActive(true);
                areas[5].SetActive(true);
                areas[9].SetActive(true);
                areas[7].SetActive(true);
            }
        }
        else if (areaName.Equals("The Clearing"))
        {
            //flower, sword
            if (status)
            {
                areas[1].SetActive(false);
                areas[8].SetActive(false);
            }
            else
            {
            }
        }
        else if (areaName.Equals("Ed's Place"))
        {
            //sword, flower garden, worlds edge, 
            if (status)
            {
                areas[3].SetActive(false); //water first
                areas[1].SetActive(false);
                areas[8].SetActive(false);
            }
            else
            {
                areas[3].SetActive(true);
            }
        }
        else if (areaName.Equals("Temple's Path"))
        {
            //sword, carl, flower
            if (status)
            {
                areas[1].SetActive(false);
                areas[8].SetActive(false);
                areas[11].SetActive(false);
            }
            else
            {
                areas[11].SetActive(true);
            }
        }
    }


    IEnumerator WaitASec()
    {
        //wait for player to remain in trigger
        yield return new WaitForSeconds(0.5f);
    }

}
