
using UnityEngine;

/*This class defines an ADT, Stat, for using to define stat values.
 * When equipment is implemented, we will cycle through a 'modifiers' list 
 * to get the stats that the equipment gives us.
 */
[System.Serializable]
public class Stat
{
    [SerializeField] int baseValue;
    //List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;

        //modifiers.ForEach(x => finalValue += x);

        return finalValue;
    }
    /*
    public void AddModifier(int modifer)
    {
        if(modifer !=0)
        {
            modifiers.Add(modifer);
        }
    }
    public void RemoveModifier(int modifier)
    {
        if(modifier !=0)
        {
            modifiers.Remove(modifier);
        }
    }
    */
}
