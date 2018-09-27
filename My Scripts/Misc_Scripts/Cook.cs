using UnityEngine;
using TMPro;
using System.Collections;
using EZCameraShake;

public class Cook : Interactable
{
    public SetCamera setCamera;

    public GameObject tossPotion;
    Rigidbody tossPotion_rb;
    Vector3 tossPotionPostion = new Vector3(-0.115f, 0.216f, 2.477f);

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
    public AudioClip bubblingNoiseClip;
    public AudioClip dropMaterialsInPot;

    bool Interacted { get; set; }

    void Start()
    {
        materials[0] = "Empty Beaker"; //need 1
        materials[1] = "Nasty Caps"; // need 1 group of 3
        materials[2] = "Dirty Harry"; // need 1
        materials[3] = "Red Speckly"; //need 1
        materials[4] = "Blue Lagoon"; //need 1

        tossPotion_rb = tossPotion.GetComponent<Rigidbody>();
    }
    public override void Interact()
    {
        if (!Interacted)
        {
            Interacted = true;
            if (inventory != null)
            {
                //then go on...
                if (SearchForAllTheItems())
                {
                }
            }
            else
            {
                //grab 'n cache baby
                inventory = GameObject.FindWithTag("Inventory").GetComponent<InventoryList>(); //TODO put a reference to the inventory in the ObjectFinder static class to eliminate this Find() operation.
                                                                                               //then do it
                if (SearchForAllTheItems())
                {
                }
            }
        }

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
                            Debug.Log("Playing something happened scene");
                            PlaySomethingHappenedScene();
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
            Debug.Log("Playing nothing happened scene");
            PlayNothingHappenedScene();
            return false;
        }
    }

    #region Something Happened or Nothing Happend Scene Calls
    void PlaySomethingHappenedScene()
    {
        Global.USER_INPUT_ENABLED = false;
        StartCoroutine(SomethingHappenedRoutine());
    }
    void PlayNothingHappenedScene()
    {
        Global.USER_INPUT_ENABLED = false;
        StartCoroutine(NothingHappenedRoutine());
    }
    #endregion

    #region Display Messages
    void DisplayMessage(int message)
    {
        StartCoroutine(DisplayMessageRoutine(message));
    }
    IEnumerator DisplayMessageRoutine(int message)
    {
        yield return new WaitForSeconds(3);

        displayCanvas.SetActive(true);
        if (message > 0)
        {
            displayText.SetText("You crafted the eternity potion!");
            //TODO make this potion damage you like a fire for as long as it's in your inventory
        }
        else
        {
            displayText.SetText("Nothing happened...");
        }

        yield return new WaitForSeconds(3);

        displayText.SetText("");
        displayCanvas.SetActive(false);
        //give the camera back to the player.
        Cauldron cauldron = GetComponent<Cauldron>();
        cauldron.cameraChanger.ChangeCam(1);
        setCamera.AdjustCameraForPlaying();
        Global.USER_INPUT_ENABLED = true;
        //let the player interact again.
        Interacted = false;
    }
    #endregion

    #region Camera Shake Methods
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
    #endregion

    #region Nothing Happened Routine 
    IEnumerator NothingHappenedRoutine()
    {
        Animator animator = ObjectFinder.PlayerAnimator;
        //wait for the swooshy camera fx
        yield return new WaitForSeconds(2);
        //play a thud noise to signify scene starting
        audioSource.PlayOneShot(thudNoise);
        yield return new WaitForSeconds(1.5f);
        //make him drop something in the pot
        animator.SetTrigger("DropItem");
        //TODO instantiate a potion at X apply force to make it go into pot
        StartCoroutine(TossPotionIntoCauldron());
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
        audioSource.PlayOneShot(bubblingNoiseClip);
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
    #endregion

    IEnumerator TossPotionIntoCauldron()
    {
        Renderer rend = tossPotion.GetComponent<Renderer>();

        tossPotion.SetActive(true);
        rend.enabled = true;
        //remove contrains so it can move
        tossPotion_rb.constraints = RigidbodyConstraints.None;


        if (tossPotion_rb != null)
        {
            tossPotion_rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            tossPotion_rb.AddForce(-transform.forward * 2.25f, ForceMode.Impulse);

            //we need a delay so the item doesn't just hang in the air
            Destroy(tossPotion_rb, 1.25f);
            //wait until the rb is destroyed
            yield return new WaitUntil(() => tossPotion_rb == null);

            //It's too slow to add a RB so instead it will always start with a rb
            rend.enabled = false;
            Rigidbody rb = tossPotion.AddComponent<Rigidbody>();
            tossPotion_rb = rb;
            //freeze it so it doesn't endlessly fall bc it is a trigger
            tossPotion_rb.constraints = RigidbodyConstraints.FreezeAll;
            //Same with its position and rotation
            tossPotion.transform.localPosition = tossPotionPostion;
            tossPotion.transform.localRotation = Quaternion.identity;

        }
    }

    #region Something Happened Routine
    IEnumerator SomethingHappenedRoutine()
    {
        Animator animator = ObjectFinder.PlayerAnimator;
        //wait for the swooshy camera fx

        yield return new WaitForSeconds(2);
        //play a thud noise to signify scene starting
        audioSource.PlayOneShot(thudNoise);
        yield return new WaitForSeconds(1.5f);
        //get the player's animator component to make him drop something in the pot
        animator.SetTrigger("DropItem");
        //TODO instantiate a potion at X apply force to make it go into pot
        StartCoroutine(TossPotionIntoCauldron());
        //wait for his hand to be over the pot
        yield return new WaitForSeconds(1);
        //play a dropping in the pot noise
        audioSource.PlayOneShot(dropMaterialsInPot);
        //change the particle color to yellowish
        ParticleSystem.MainModule main = fire.main;
        main.startColor = new Color(255, 250, 0);
        //shake the camera for 4 seconds while we do other stuff
        ShakeCamera(4);
        //Pop black smoke over the cauldron
        ParticleSystem.MainModule mainTwo = smoke.main;
        mainTwo.startColor = new Color(0, 0, 0);

        //make the player look scared
        animator.SetBool("IsBlocking", true);
        //play a successful cooking noise
        audioSource.pitch = 0.75f;
        audioSource.PlayOneShot(bubblingNoiseClip);
        yield return new WaitForSeconds(2);
        //1 for true, something happened
        DisplayMessage(1);
        yield return new WaitForSeconds(3);
        //change the pitch back to normal
        audioSource.pitch = 1f;
        //change the fire back to normal fire color
        main.startColor = new Color(255, 197, 146);
        //change the smoke back to normal smoke
        mainTwo.startColor = new Color(255, 255, 255);
        animator.SetBool("IsBlocking", false);

        //tell the cauldron not to let the player cook again until the potion is out of the cauldron
        GetComponent<Cauldron>().CanCook = false;
        yield return new WaitForSeconds(2);


        MakePotion();
    }


    void MakePotion()
    {
        //remove all materials if all are found
        foreach (string s in materials)
        {
            inventory.SearchInventoryAndRemoveIfFound(s);
        }
        //apply a force to the potion to spit it out of the cauldron at you
        TossPotionToPlayer();
    }

    void TossPotionToPlayer()
    {
        eternityPotion.SetActive(true);

        Rigidbody rbItem = eternityPotion.AddComponent<Rigidbody>();

        if (rbItem != null)
        {
            rbItem.AddForce(transform.up * 2.75f, ForceMode.Impulse);
            rbItem.AddForce(transform.forward * 2.5f, ForceMode.Impulse);

            //we need a delay so the item doesn't just hang in the air
            Destroy(rbItem, 0.7f);
        }
        //tell the cauldron the player is allowed to cook again after the potion is out of the cauldron
        GetComponent<Cauldron>().CanCook = true;
    }

    #endregion

}
