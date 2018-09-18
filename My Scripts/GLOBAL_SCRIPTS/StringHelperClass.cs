public static class StringHelperClass 
{
    const int LENGTH_OF_LINE = 42;
    public static string PadQuestTextToLengthOfLine(string sentence)
    {
        int length = sentence.Length;
        string result = sentence;

        for (int i = length; i < LENGTH_OF_LINE; i++)
        {
            result += " ";
        }

        return result;
    }

    public static string DetermineLastQuestWord(string questType)
    {
        switch(questType)
        {
            case "CollectionGoal":
                return " collected";
            case "DiscoveryGoal":
                return " discovered";
            case "ConversationGoal":
                return " spoken with";
            case "KillGoal":
                return " killed";
            default:
                return "";
        }
    }

    public static string ConstructConversationPath(string questTurnInPointName)
    {
        int indexAfter_ = 13;
        string result = "Conversation_-5";
        result = result.Insert(indexAfter_, questTurnInPointName);

        return result;
        //"Conversation_questTurnInPointName-5"
    }

    public static string ParseDialogueIDForString(Dialogue item) //Carl-0
    {
        string itemName = item.dialogueID;
        //retrieve all chars up to the - character.
        string result = itemName.Substring(0, itemName.IndexOf('-'));
        return result;
    }
}
//public static void SetKeyActive(string key, bool active)
//{
    //switch (key)
    //{
        //case "IpKey":
            //if (active)
            //    hasIpKey = true;
            //else
            //    hasIpKey = false;
            //break;