using UnityEngine;
/*WARNING: 
 * SOMETIMES UNITY WILL "GET STUCK" 
 * Objects left in scene and tested repeatedly will have weird behaviors.
 * A simple replacement of the prefab will correct whatever is stuck.
 */
public enum CrateName : byte//wtf c# enums are fucked up? why do i have to cast
{
    IpCrate,
    EdCrate,
    OceanCrate,
    TempleCrate,
    CarlCrate,
    BehindTempleCrate,
    DeadForestCrate,
    FlowerGardenCrate,
    SwordCaveCrate,
    GuardianCrate
}
//Every crate must first be opened with a key
public class OpenCrate : MonoBehaviour 
{
    [SerializeField] CrateName m_crateID;
    [SerializeField] GameObject m_loot;
    [SerializeField] Animator m_animator;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip[] m_openNoises;
    [SerializeField] AudioClip[] m_closeNoises;

    bool activatedItems = false;
    //if unlocked, then it can be opened again without the key.
    bool unlocked = false;
    bool open = false;
    //dont forget to drag and drop crate noises into the inspector like a dumbass
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other) 
	{
        //logical OR will not evaluate the right side if the left is true
        if( (!open && unlocked) || (!open && KeyManager.HasMatchingKey((byte)m_crateID)) )
        {
            if (!activatedItems)
            {
                ActivateItemsInCrate();
            }
            OpenTheCrate();
        }
        else
            m_animator.SetBool("open", false);
	}

    void OnTriggerExit(Collider other)
    {
        if (open)
        {
            CloseTheCrate();
        }
    }

    void OpenTheCrate()
    {
        m_animator.SetBool("open", true);
        m_audioSource.PlayOneShot(m_openNoises[Random.Range(0, m_openNoises.Length)]);
        unlocked = true;
        open = true;
        //destroy key?
    }
    void CloseTheCrate()
    {
        open = false;
        m_animator.SetBool("open", false);
        m_audioSource.PlayOneShot(m_closeNoises[0]);
    }
    //todo: roll for items in crate
    void ActivateItemsInCrate()
    {
        activatedItems = true;
        //if theres loot...
        if(m_loot != null)
        {   //Transform implements numerable NOT gameobject...this is stupid
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
    }
}
