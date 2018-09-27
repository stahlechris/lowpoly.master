using System.Collections;
using UnityEngine;


public class OfferingBowl : MonoBehaviour 
{
    [SerializeField] OfferingAreaManager manager;
    [SerializeField] GameObject receivedItem;
    [SerializeField] GameObject m_loot;

    const string ITEM_ONE = "Lamb Leg";
    const string ITEM_TWO = "Space Rock";
    const string ITEM_THREE = "Ancient Flower";
    const string ITEM_FOUR = "Bird_Book";
    const string ITEM_FIVE = "something";

    [SerializeField] GameObject ITEM_ONE_REWARD;
    [SerializeField] GameObject ITEM_TWO_REWARD;
    [SerializeField] GameObject ITEM_THREE_REWARD;
    [SerializeField] GameObject ITEM_FOUR_REWARD;
    [SerializeField] GameObject ITEM_FIVE_REWARD;

    bool GaveItemOne { get; set; }
    bool GaveItemTwo { get; set; }
    bool GaveItemThree { get; set; }
    bool GaveItemFour { get; set; }
    bool GaveItemFive { get; set; }

    bool HasEntered { get; set; }                       //Prohibit object triggering twice.
    public bool CheckForCollisions { get; set; }        //Don't waste power checking for collisions if were doing this.
    bool InitiatedLootSequence { get; set; }            //Protect against dropping loot twice.

    private void Start()
    {
        manager = GetComponentInParent<OfferingAreaManager>();
        CheckForCollisions = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the player's thrown object hasnt't entered, and we are checking for collisions, and it isn't the player
        if (!HasEntered && CheckForCollisions && !collision.collider.CompareTag("Player"))
        {
            CheckForCollisions = false;                                         //Only check for collisions after the process is over
            HasEntered = true;
            receivedItem = collision.gameObject;
            bool success = false;

            switch (collision.transform.name)
            {
                case ITEM_ONE:
                    GaveItemOne = true;
                    //Add the corresponding reward item to the loot object as the last child
                    ITEM_ONE_REWARD.transform.SetParent(m_loot.transform); 
                    success = true;
                    break;
                case ITEM_TWO:
                    GaveItemTwo = true;
                    success = true;
                    break;
                case ITEM_THREE:
                    GaveItemThree = true;
                    success = true;
                    break;
                case ITEM_FOUR:
                    GaveItemFour = true;
                    success = true;
                    break;
                case ITEM_FIVE:
                    GaveItemFive = true;
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }
            //Tell the manager to do something about it, im just an employee, bro.
            manager.Inform(success);
            //and deactivate the item if its a match so player cant interupt the process
            if (success)
            {
                DisableReceivedItem(collision.collider);
            }
            else
            {
                StartCoroutine(TossItemBackAtPlayer());
            }
        }
        HasEntered = false;
    }
    IEnumerator TossItemBackAtPlayer() //This has to wait for 0.6f before adding a RB or else there's already one
    {
        yield return new WaitForSeconds(0.6f);
        Rigidbody rbItem = receivedItem.AddComponent<Rigidbody>();

        if (rbItem != null)
        {
            rbItem.AddForce(transform.up * 2.75f, ForceMode.Impulse);
            rbItem.AddForce(-transform.forward * 2f, ForceMode.Impulse);

            //we need a delay so the item doesn't just hang in the air
            Destroy(rbItem, 0.6f);
        }
        receivedItem = null;
    }
    void DisableReceivedItem(Collider item)
    {
        //Disable item so player can't loot it mid scene.
        //Disabling the collider is the fastest way to "disable" the item.
        item.enabled = false;
        Destroy(receivedItem,2.5f);

    }
    public void GiveLoot()
    {
        if (!InitiatedLootSequence)
        {
            InitiatedLootSequence = true;

            ActivateLootItems();
            PlayDeathFX();
        }
        Debug.Log("you already started the death sequence");
    }

    private void PlayDeathFX()
    {
        AudioSource[] deathSound = transform.Find("Loot").GetComponentsInChildren<AudioSource>();
        ParticleSystem[] fx = GetComponentsInChildren<ParticleSystem>();
        fx[0].Play();
        deathSound[0].Play();
        deathSound[1].Play();
        StartCoroutine(LootAppears(deathSound[2], fx[2]));
    }
    IEnumerator LootAppears(AudioSource itemAppears, ParticleSystem itemCloud)
    {
        yield return new WaitForSeconds(4.5f);
        itemAppears.Play();
        itemCloud.Play();
        ITEM_ONE_REWARD.GetComponent<MeshRenderer>().enabled = true;
    }
    void ActivateLootItems()
    {
        if (m_loot != null)
        {   //Transform implements numerable NOT gameobject...this is stupid (can't change item.gameobject to gameobject)
            m_loot.SetActive(true);
            foreach (Transform item in m_loot.transform)
            {
                item.gameObject.SetActive(true);
                foreach (Behaviour component in item.GetComponentsInChildren<Behaviour>())
                {
                    component.enabled = true;
                }
            }
        }
        else
        {
            //didn't have any loot
            //play a womp womp woOoommmp sound fx because you fucking suck
        }
        StartCoroutine(DeactivateLootHolder());
        manager.StartedSequence = false;
        manager.SequenceOver();
    }
    IEnumerator DeactivateLootHolder()
    {
        yield return new WaitForSeconds(10);
        m_loot.SetActive(false);
    }
}
