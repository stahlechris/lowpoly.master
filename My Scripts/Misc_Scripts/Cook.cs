using UnityEngine;
using TMPro;
using System.Collections;
using EZCameraShake;
public class Cook : Interactable
{
    public AudioSource audioSource;
    public string[] materials = new string[5];
    public GameObject eternityPotion; //end result
    public GameObject displayCanvas;
    public TextMeshProUGUI displayText;
    public ParticleSystem fire;
    public ParticleSystem smoke;
    InventoryList inventory;
    CameraShakeInstance instance;
    public AudioClip thudNoise;
    public AudioClip bubblingNoiseUnsuccesful;
    public AudioClip bubblingNoiseSuccesful;
    public AudioClip dropMaterialsInPot;


    void Start()
    {
        materials[0] = "Empty Beaker"; //need 1
        materials[1] = "Nasty Caps"; // need 1 group of 3
        materials[2] = "Dirty Harry"; // need 1
        materials[3] = "Red Speckly"; //need 1
        materials[4] = "Blue Lagoon"; //need 1
    }
    public override void Interact()
    {
        if (inventory != null)
        {
            //then go on...
            if (SearchForAllTheItems())
            {
                DisplayMessage(1);
            }
        }
        else
        {
            //grab it
            inventory = GameObject.FindWithTag("Inventory").GetComponent<InventoryList>();
            //then do it
            if (SearchForAllTheItems())
            {
                DisplayMessage(1);
            }
        }
        Debug.Log("Carl hasn't given you permission to use his cauldron yet!");
    }
    bool SearchForAllTheItems()
    {
        Debug.Log("Cauldron started searching for items");
        if (SearchInventory(materials[0], 1))
        {
            //has the beaker to cook the potion, so check for needed mushrooms
            if (SearchInventory(materials[1], 1))
            {
                //player has nastyCaps
                if (SearchInventory(materials[2], 1))
                {
                    //player has dirtyHarry
                    if (SearchInventory(materials[3], 2))
                    {
                        //player has redSpecklys
                        if (SearchInventory(materials[4], 1))
                        {
                            //player has all the needed materials
                            MakePotion();
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    bool SearchInventory(string itemName, int reqAmt)
    {
        if (inventory.SearchInventory(itemName) == reqAmt)
        {
            return true;
        }
        else
        {
            PlayNothingHappenedScene();
            return false;
        }
    }

    void MakePotion()
    {
        //remove all materials if all are found
        foreach (string s in materials)
        {
            inventory.SearchInventoryAndRemoveIfFound(s);
        }
        //particlefx
        //sound
        //cutscene..
        //camera shakey shakey
        //etc...
        //enable object so player can loot it
        eternityPotion.SetActive(true);
        //play a short cutscene of player throwing the potion at him
        //cauldron class resets the alreadySpeaking flag 
    }
    void DisplayMessage(int message)
    {
        StartCoroutine(DisplayMessageRoutine(message));
    }

    IEnumerator DisplayMessageRoutine(int message)
    {
        yield return new WaitForSeconds(3);

        displayCanvas.SetActive(true);
        if(message > 0)
        {
            displayText.SetText("You crafted the eternity potion!...\n It smells horrible and looks VERY unstable.");
        }
        else
        {
            displayText.SetText("Nothing happened...");
        }
        yield return new WaitForSeconds(3);
        displayText.SetText("");
        displayCanvas.SetActive(false);
        //give the camera back to the player.
        GetComponent<Cauldron>().cameraChanger.ChangeCam(1);
        Global.USER_INPUT_ENABLED = true;
    }
    public void ShakeCamera(float duration)
    {
        StartCoroutine(ShakeTheCameraRoutine(duration));
    }
    IEnumerator ShakeTheCameraRoutine(float duration)
    {
        instance = CameraShaker.Instance.StartShake(1f, 1f, 0.25f);
        yield return new WaitForSeconds(duration);
        instance.StartFadeOut(0.5f);
    }
    void PlayNothingHappenedScene()
    {
        //disable user input
        Global.USER_INPUT_ENABLED = false;
        StartCoroutine(NothingHappenedRoutine());
    }
    IEnumerator NothingHappenedRoutine()
    {
        Animator animator = ObjectFinder.PlayerAnimator;

        yield return new WaitForSeconds(1);
        //play a thud noise to signify scene starting
        audioSource.PlayOneShot(thudNoise);
        yield return new WaitForSeconds(1);
        //get the player's animator component to make him drop something in the pot
        animator.SetTrigger("DropItem");
        //TODO instantiate a potion at X apply force to make it go into pot

        //wait for his hand to be over the pot
        yield return new WaitForSeconds(1);
        //play a dropping in the pot noise
        audioSource.PlayOneShot(dropMaterialsInPot);
        //change the particle color to greenish
        ParticleSystem.MainModule main = fire.main;
        main.startColor = new Color(0, 123, 0);
        //shake the camera for 4 seconds while we do other stuff
        ShakeCamera(4);
        ParticleSystem.MainModule mainTwo = smoke.main;
        mainTwo.startColor = new Color(0, 0, 0);

        //make the player look scared
        animator.SetBool("IsBlocking", true);
        //play an unsucesful cooking noise
        audioSource.PlayOneShot(bubblingNoiseUnsuccesful);
        yield return new WaitForSeconds(2);
        //0 for false, nothing happened
        DisplayMessage(0);
        yield return new WaitForSeconds(3);
        //think we have to make more temp particlesystems here
        //change the fire back to normal fire color
        main.startColor = new Color(255, 197, 146);
        //change the smoke back to normal smoke
        mainTwo.startColor = new Color(255, 255, 255);
        animator.SetBool("IsBlocking", false);


    }







    IEnumerator SomethingHappenedRoutine()
    {
        yield return new WaitForSeconds(4);

    }




    void PlayPotionCraftedScene()
    {
        
    }
}
