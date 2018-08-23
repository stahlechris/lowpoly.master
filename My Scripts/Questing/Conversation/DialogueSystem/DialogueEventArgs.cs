using System;

public class DialogueEventArgs : EventArgs
{
    public Dialogue m_dialogueItem;

    public DialogueEventArgs(Dialogue dialogueItem)
    {
        m_dialogueItem = dialogueItem;
    }
}
