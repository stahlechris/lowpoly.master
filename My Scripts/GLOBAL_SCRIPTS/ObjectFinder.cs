using UnityEngine;
using LowPoly.Character;

public class ObjectFinder : MonoBehaviour
{
    #region Systems
    public static HUD HUD { get; set; }
    public static QuestList QuestLog { get; set; }
    public static EndlessPlatformSystem EndlessPlatformSystem { get; set; }
    #endregion

    #region PLAYER
    public static GameObject PlayerGameObject;
    public static Transform PlayerTransform { get; set; }
    public static PlayerController PlayerController { get; set; }
    public static Rigidbody PlayerRigidBody { get; set; }
    public static Animator PlayerAnimator { get; set; }
    #endregion

    #region NPC's
    public static Transform CarlTransform { get; set; }
    public static Transform BirdTransform { get; set; }
    public static Transform IpTransform { get; set; }
    public static Transform EdTransform { get; set; }
    #endregion

    #region NPC's UI
    public static NPC_UI Carl_UI { get; set; }
    public static NPC_UI Bird_UI { get; set; }
    public static NPC_UI Ip_UI { get; set; }
    public static NPC_UI Ed_UI { get; set; }
    #endregion

    void Start()
    {
        #region Systems
        HUD = GameObject.FindWithTag("HUD").GetComponent<HUD>();
        //Debug.Log(this + " found HUD? => " + HUD);
        QuestLog = GameObject.FindWithTag("QuestLog").GetComponent<QuestList>();
        //Debug.Log(this + " found Questlist? => " + QuestLog);
        EndlessPlatformSystem = GameObject.FindWithTag("EndlessPlatformSystem").GetComponent<EndlessPlatformSystem>();
         
        #endregion

        #region PLAYER
        PlayerGameObject = GameObject.FindWithTag("Player");
        //Debug.Log(this + " found Player's GO? => " + PlayerGameObject.name);

        PlayerController = PlayerGameObject.GetComponent<PlayerController>();
        //Debug.Log(this + " found Player's controller? => " + PlayerController.name);

        PlayerTransform = PlayerGameObject.GetComponent<Transform>();

        PlayerRigidBody = PlayerGameObject.GetComponent<Rigidbody>();


        PlayerAnimator = PlayerGameObject.GetComponent<Animator>();

        #endregion

        #region NPC's
        CarlTransform = GameObject.FindWithTag("Carl").GetComponent<Transform>();
        //Debug.Log(this + " found Carl's Transform? => " + CarlTransform.name);

        BirdTransform = GameObject.FindWithTag("Bird").GetComponent<Transform>();
        //Debug.Log(this + " found Bird's Transform? => " + BirdTransform.name);

        IpTransform = GameObject.FindWithTag("Ip").GetComponent<Transform>();
        //Debug.Log(this + " found Ip's Transform? => " + IpTransform.name);

        EdTransform = GameObject.FindWithTag("Ed").GetComponent<Transform>();
        //Debug.Log(this + " found Ed's Transform? => " + EdTransform.name);
        #endregion

        #region NPC's UI
        Carl_UI = CarlTransform.GetComponentInChildren<NPC_UI>();
        Bird_UI = BirdTransform.GetComponentInChildren<NPC_UI>();
        Ip_UI = IpTransform.GetComponentInChildren<NPC_UI>();
        Ed_UI = EdTransform.GetComponentInChildren<NPC_UI>();
        #endregion

    }

    void OnDisable()
    {
        //do ineed to null these out???   
    }
}
