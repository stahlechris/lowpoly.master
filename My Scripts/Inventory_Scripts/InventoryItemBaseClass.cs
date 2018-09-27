using UnityEngine;
public enum ItemType
{
    Default,
    Consumable,
    Weapon,
    Key,
    LegendSwordPiece,
    QuestItem,
    QuestGivingItem
}
/// <summary>
/// TODO: Consider Inheriting from ScriptableObject...
/// ...then [create asset menu] for item. 
/// ...Choose between -> default, consumable, weapon
/// idk tho
/// </summary>
public class InventoryItemBase : MonoBehaviour 
{
    public ItemType ItemType;
    protected string itemPickupMessage = null; 
    public Vector3 PickPosition;
    public Vector3 PickRotation;
    public Vector3 DropRotation;
    //consider using int as the identifier key 
    public string Name;
    public Sprite image;
    protected HUD hud;

    public AudioClip pickupSound;
    //AudioSource audioSource;

    void Start()
    {
        hud = FindObjectOfType<HUD>();
    }
    public InventorySlotStack Slot
    {
        get; 
        set;
    }

    public virtual void OnUse()
    {
        /* This method is called when 
         * we put an item - like a weapon -  into our hand.
         */
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    /*
     * This method shoots a ray out of our current mouse position.
     * When that ray hits something with a collider on it...
     * It will 
     */
    public virtual void OnDrop()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
            gameObject.transform.eulerAngles = DropRotation;
        }
    }

    public virtual void OnPickup()
    {
        //Instantiate a temporary audiosource on the object long enough for the clip to play, then destroy the temporary audiosource
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        //Get rid of the rigidbody on the item we pick up. if it has one
        Destroy(gameObject.GetComponent<Rigidbody>());
        //Get rid of the levitate on the item if it has one.
        Destroy(gameObject.GetComponent<Levitate>());
        //Set Inactive (it's still there).
        gameObject.SetActive(false);
    }

    public virtual string GetDetailsAboutItem()
    {
        return Name;
        //Debug.Log("*shows stats*" + this);
    }
}
