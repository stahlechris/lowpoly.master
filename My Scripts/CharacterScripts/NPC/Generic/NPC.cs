using UnityEngine;

public enum NPC_ID
{
    Ip,
    Ed,
    Carl,
    Pers
}
public class NPC : Interactable 
{
    //public NPC_ID NPC_ID { get; set; }
    [SerializeField] string npc_ID;
    public override void Interact()
    {
        //Debug.Log("Interacting with NPC");
        //haveConversation();

    }
}
