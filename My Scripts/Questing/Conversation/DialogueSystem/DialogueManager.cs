using UnityEngine;

/*This class receives TextAssets and assigns them to the given objects Dialogue.
 */
public class DialogueManager : MonoBehaviour 
{
    Dialogue DialogueOfNPC { get; set; }
    string Path { get; set; }

    public void SetupNewDialogue(GameObject npc, string path)
    {
        //Check if attempting to load same conversation
        if (!path.Equals(this.Path))
        {
            this.DialogueOfNPC = npc.GetComponentInChildren<Dialogue>(); //get their throat
            this.Path = path; //cram this down it

            DialogueOfNPC.conversationOver = false;
            DialogueOfNPC.SetupNewConversation(path);
        }
        else
        {
            //If same conversation, save time, just reset
            ResetConversation(npc);
        }
    }
    void ResetConversation(GameObject npc)
    {
        DialogueOfNPC.conversationOver = false;
        DialogueOfNPC.isTyping = false;
        DialogueOfNPC.index = 3;
        Debug.Log("You attempted to load the same conversation!, I just reset it instead");
    }

}
