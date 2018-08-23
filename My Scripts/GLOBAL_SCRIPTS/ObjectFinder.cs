using UnityEngine;
using LowPoly.Character;
public class ObjectFinder : MonoBehaviour 
{
    public static HUD HUD { get; set; }

    const string PLAYER_NAME = "Liam";
    public static GameObject PlayerGameObject { get; set; }
    public static Transform PlayerTransform { get; set; }
    public static PlayerController PlayerController { get; set; }
    public static Rigidbody PlayerRigidBody { get; set; }


    public static QuestList QuestLog { get; set; }

	void Start () 
    {
        HUD = GameObject.Find("HUD").GetComponent<HUD>();
        QuestLog = GameObject.Find("QuestLog").GetComponent<QuestList>();
        PlayerGameObject = GameObject.Find(PLAYER_NAME);
        PlayerController = PlayerGameObject.GetComponent<PlayerController>();
        PlayerTransform = PlayerGameObject.GetComponent<Transform>();
        PlayerRigidBody = PlayerGameObject.GetComponent<Rigidbody>();
	}
   

    void OnDisable()
    {
     //do ineed to null these out???   
    }

}
