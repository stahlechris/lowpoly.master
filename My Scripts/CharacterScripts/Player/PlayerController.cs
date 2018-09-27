using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using LowPoly.Weapon;

namespace LowPoly.Character
{
    [RequireComponent(typeof(CustomMovement))]
    public class PlayerController : MonoBehaviour
    {
        public SetCamera thirdPersonCam;
        CustomMovement m_Character;
        Transform m_Cam;
        Vector3 m_CamForward;       // The current forward direction of the camera
        Vector3 m_Move;
        Rigidbody myRigidbody;
        bool m_Jump;
        bool m_AutoRunPressed;
        bool m_isAutoRunning;
        public InventoryList inventory;
        public GameObject m_hand;
        public HUD hud;
        public Animator animator;
        SpecialAbilities m_SpecialAbilities;
        WeaponSystem m_WeaponSystem; 
        public StaminaSystem m_StaminaSystem;
        int startHealth;
        [SerializeField] InventoryItemBase mItemIAmHolding = null;
        public InventoryItemBase mItemRequestingToBeCollected;
        public GameObject m_PersonRequestingToBeSpokenWith;
        //private bool alreadySetSpeedMultiplier = false;
        public bool alreadySpeaking = false;
        [SerializeField] AnimatorOverrideController m_AnimatorOverrideController;
        //public Dialogue conversation;
        const string DROP_ITEM = "DropItem";
        public AnimatorOverrideController GetAnimatorOverrideController()
        {
            return m_AnimatorOverrideController;
        }

        void Start()
        {
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("No camera tagged as 'MainCamera'.");
            }
            m_Character = GetComponent<CustomMovement>(); //be able to move
            myRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>(); //pick stuff up & drop stuff
            m_SpecialAbilities = GetComponent<SpecialAbilities>(); //cast spells
            m_WeaponSystem = GetComponent<WeaponSystem>(); //hit stuff

            //Subscribe to these events
            inventory.OnItemUsed += Inventory_ItemUsed;
            inventory.OnItemRemoved += Inventory_ItemRemoved;

            animator.SetTrigger("WakeUp");
        }

        void Update()
        {
            if (Global.USER_INPUT_ENABLED)
            {
                if (!m_Jump && CrossPlatformInputManager.GetButtonDown("Jump")) //If we are not currently performing a jump...
                {
                    if (m_StaminaSystem.AttemptJump())
                    {
                        m_Jump = true;
                    }
                    else
                    {
                        m_Jump = false;
                    }
                }

                //UNDER CONSTRUCTION
                //if (m_AutoRunPressed = CrossPlatformInputManager.GetButtonDown("AutoRun"))
                //{
                //    m_isAutoRunning = !m_isAutoRunning;
                //}

                //check if trying to interact with an npc or an item.
                if (CrossPlatformInputManager.GetButtonDown("Interact"))
                {
                    //it's an NPC
                    if (m_PersonRequestingToBeSpokenWith != null) 
                    {
                        if (!alreadySpeaking)
                        {//!!ORDER IS IMPORTANT HERE!!\\
                            alreadySpeaking = true;
                            SetPlayerPositionForConversation();
                            m_PersonRequestingToBeSpokenWith.GetComponent<Interactable>().Interact();
                        }
                        else
                        {
                            //you're talking
                        }
                    }

                    //its an item
                    if (mItemRequestingToBeCollected != null)
                    {
                        animator.SetTrigger("Pickup");
                        //mItemRequestingToBeCollected.GetDetailsAboutItem();
                        inventory.AddItem(mItemRequestingToBeCollected);
                        //OnPickup() destroys rigidbody
                        mItemRequestingToBeCollected.OnPickup();

                        if (hud.IsMessagePanelOpened)
                        {
                            hud.CloseMessagePanel();
                        }
                        mItemRequestingToBeCollected = null;
                    }
                    else
                    {
                        Debug.Log("item is null or not able to be collected");
                    }


                }
                if (CrossPlatformInputManager.GetButtonDown("SelfHeal"))
                {
                    m_SpecialAbilities.AttemptSpecialAbility(0, this.gameObject);
                }
                if (CrossPlatformInputManager.GetButtonDown("AOE"))
                {
                    m_SpecialAbilities.AttemptSpecialAbility(1, this.gameObject);
                }
                if (CrossPlatformInputManager.GetButtonDown("PowerAttack"))
                {
                    m_SpecialAbilities.AttemptSpecialAbility(2, m_WeaponSystem.GetCurrentTarget());
                }
            }
        }
        void FixedUpdate()
        {
            if (Global.USER_INPUT_ENABLED)
            {
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");


                // calculate move direction to pass to character
                if (m_Cam != null)
                {
                    // calculate camera relative direction to move:
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move = v * m_CamForward + h * m_Cam.right;
                }

                //TODO: sync footsteps with sprinting
                //if (Input.GetKey(KeyCode.LeftShift) ) //little weird innit? //TODO add "&& isMoving
                if( (!m_StaminaSystem.isCatchingBreath && Input.GetKey(KeyCode.LeftShift) && myRigidbody.velocity.magnitude > 0)|| 
                   (!m_StaminaSystem.isCatchingBreath && CrossPlatformInputManager.GetButton("AutoRun")) )
                {
                    if (m_StaminaSystem.AttemptSprint())
                    {
                        m_Character.m_MoveSpeedMultiplier = 1.5f;
                        m_Character.m_AnimSpeedMultiplier = 1.5f;
                    }
                    else
                    {
                    }
                }
                else //
                {
                    m_StaminaSystem.isSprinting = false;
                    m_Character.m_MoveSpeedMultiplier = 1f;
                    m_Character.m_AnimSpeedMultiplier = 1f;
                }


                /// <summary>
                /// Vector3.forward and the others are in WORLD SPACE, 
                /// so they will always move you in the same direction -  
                /// no matter which direction your character is in.
                /// 
                /// To FIX THIS you can replace the Vector3.forward with transform.forward. 
                /// transform.forward is the forward direction of the transform, 
                /// taking rotation into account.
                /// </summary>

                //AUTORUN UNDER CONSTRUCTION

                //TODO: get rid of this autorun shit
                //if autoRun is true AND theres no input coming from the left stick/wsad/keys
                if (m_isAutoRunning && Math.Abs(CrossPlatformInputManager.GetAxis("Horizontal"))
                    + Math.Abs(CrossPlatformInputManager.GetAxis("Vertical")) < 2 * float.Epsilon)
                {
                    //pass constant forward direction to the move function WITH transform's rotation in mind
                    Vector3 forward = transform.forward;
                    m_Character.Move(forward, m_Jump);
                }
                else
                {
                    m_Character.Move(m_Move, m_Jump);
                    m_Jump = false;
                }

                if (mItemIAmHolding != null && CrossPlatformInputManager.GetButtonDown("DropItem"))
                {
                    DropCurrentItem();
                }
            }
        }

        //TODO: Think about below
        void SetPlayerPositionForConversation()
        {
            //don't set positioning with eyeguards or helpful fish
            if(m_PersonRequestingToBeSpokenWith.CompareTag("Eyeguard"))
            {//Right now the rotation is all off when Liam interacts becasue the objects orientation and local vs world is off
                /* Interacting with the eyeguard might or might not happen only once : 
                 * In The stonehengeManager,
                 * 0. We need to set Liams position close enough for animation to look like he's poking Eyeguard in the eye and disable input.
                 * 1. We need to turn off our main cam and activate a fp eyeguard cam
                 * 2. We need to trigger ReachForward 
                 * 3. Eyeguard UI needs to display an angry red "!?" symbol above head.
                 * 4. We need to shutoff this camera and enable the stonehengeCam....
                 */
                //animator.SetTrigger("ReachForward");

            }

            else if (!m_PersonRequestingToBeSpokenWith.CompareTag("Tome")) 
                //we dont set positioning for conversations with tomes..because we HATE tomes
            {
                //smoothly rotate the camera to the speaker
                thirdPersonCam.RotateCamera(m_PersonRequestingToBeSpokenWith.transform);

                //freeze him
                Rigidbody m_RigidBody = GetComponent<Rigidbody>();
                m_RigidBody.constraints = RigidbodyConstraints.FreezePosition;

                Vector3 speakersPosition = m_PersonRequestingToBeSpokenWith.transform.position;
                Vector3 speakersDirection = m_PersonRequestingToBeSpokenWith.transform.forward;
                Quaternion speakersRotation = m_PersonRequestingToBeSpokenWith.transform.rotation;

                //adjust the below number to move player closer or farther from speaker
                Vector3 playerPlacement = speakersPosition + (speakersDirection * 1.75f);
                transform.position = playerPlacement;


                animator.SetFloat("Forward", 0.0f);
                animator.SetFloat("JumpLeg", 0.0f);

                //reset rotation before setting
                transform.rotation = Quaternion.identity;
                Vector3 relativePosition = m_PersonRequestingToBeSpokenWith.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePosition);
                transform.rotation = rotation;
                //freeze rotation or else hell float away
                m_RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                //dont set and just interact freely

            }
        }
        //TODO: Call this to set Unarmed Animations vs Armed Animations when ready
        public bool IsArmed
        {
            get
            {
                if(mItemIAmHolding != null)
                {
                    return mItemIAmHolding.ItemType == ItemType.Weapon;
                }
                return false;
            }
        }
        void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
        {
            //Do not do behavior below if the item has been given a death setence
            if (!e.m_Destroy)
            {
                InventoryItemBase itemBase = e.m_Item;

                GameObject goItem = (itemBase as MonoBehaviour).gameObject;
                goItem.SetActive(true);
                goItem.transform.parent = null;
            }
        }


        //TODO fix - the logic below for using an inventory item is totally B0rked
        void Inventory_ItemUsed(object sender, InventoryEventArgs e)
        {  
            if(e.m_Item.ItemType == ItemType.QuestGivingItem)
            {
                return;
            }
            if(e.m_Item.ItemType == ItemType.Key)
            {
                Debug.Log("I'll need to find the right crate for this key...");
                return;
            }
            //if a consumable item is missing a component, this check will not execute
            if(e.m_Item.ItemType != ItemType.Consumable && e.m_Item.ItemType != ItemType.QuestItem)//todo change this to "... == weapon aka only carry weapons
            {
                if(mItemIAmHolding != null)//if im holding an item already
                {
                    Debug.Log("I'm already holding an item! Drop it to carry a different one");
                    SetItemActive(mItemIAmHolding, false);//deactivate it
                }
            }
            else
            {
                //Debug.Log("A consumable item was used");
            }
            //..then ignore all checks and just do it. lol. TODO fix this
            //InventoryItemBase itemTemp = e.m_Item; //get the new item 
            mItemIAmHolding = e.m_Item;
            SetItemActive(mItemIAmHolding, true); //activate this one

        }
        void SetItemActive(InventoryItemBase item, bool active)
        {
            GameObject currentItem = (item as MonoBehaviour).gameObject;
            currentItem.SetActive(active);
            currentItem.transform.parent = active ? m_hand.transform : null;
        }

        void DropCurrentItem()
        {
            GameObject itemToDrop = mItemIAmHolding.gameObject;

            inventory.RemoveItem(mItemIAmHolding);

            Rigidbody rbItem = itemToDrop.AddComponent<Rigidbody>();

            if (rbItem != null)
            {
                animator.SetTrigger(DROP_ITEM);   

                //toss with enough force to not trigger pickup message
                rbItem.AddForce(transform.up * 2.75f, ForceMode.Impulse);
                rbItem.AddForce(transform.forward * 2f, ForceMode.Impulse);

                //we need a delay so the item doesn't just hang in the air
                Invoke("DestroyDropItem", 0.6f);
            }
        }
        public void DestroyDropItem()
        {
            Destroy((mItemIAmHolding as MonoBehaviour).GetComponent<Rigidbody>());
            mItemIAmHolding = null;
            mItemRequestingToBeCollected = null;

            if (hud.IsMessagePanelOpened)
            {
                hud.CloseMessagePanel();
            }
        }
    }
}
