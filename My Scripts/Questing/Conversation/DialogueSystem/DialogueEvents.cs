using UnityEngine;


/*You MUST remember when using static events to unsubscribe from them.
 * If you do not unsubscribe, null reference errors will occur that you cannot track.
 * 
 * This will also null reference is nobody has subscribed to the event
 */
public class DialogueEvents : MonoBehaviour
{
    public delegate void EventHandler<DialogueEventArgs>(Dialogue dialogueItem);
    public static event EventHandler<DialogueEventArgs> OnDialogueStart;
    public static event EventHandler<DialogueEventArgs> OnDialogueEnd;

    public static void FireAnEvent_OnDialogueStart(Dialogue dialogueItem)
    {
        if (OnDialogueStart != null)
        {
            OnDialogueStart(dialogueItem);
        }
        else
        {
            Debug.Log("null start dialogue event");
        }
    }

    public static void FireAnEvent_OnDialogueEnd(Dialogue dialogueItem)
    {
        if (OnDialogueEnd != null)
        {
            OnDialogueEnd(dialogueItem);
        }
        else
        { 
            Debug.Log("null end dialogue event of " + dialogueItem);
        }
    }

}