using System;
using UnityEngine;
/*
*Keys can be collected in a generic key inventory slot... 
*but have different ID's on their backend
*/
public static class KeyManager 
{
    public static bool hasIpKey;
    public static bool hasEdKey;
    public static bool hasOceanKey;
    public static bool hasTempleKey;
    public static bool hasCarlKey;
    public static bool hasBehindTempleKey;
    public static bool hasDeadForestKey;
    public static bool hasFlowerGardenKey;
    public static bool hasSwordCaveKey;
    public static bool hasGuardianKey;

    //[RuntimeInitializeOnLoadMethod] not working, added to reset level script
    //static void OnRuntimeMethodLoad()
    //{
    //    hasIpKey = false;
    //    hasEdKey = false;
    //    hasOceanKey = false;
    //    hasTempleKey = false;
    //    hasCarlKey = false;
    //    hasBehindTempleKey = false;
    //    hasDeadForestKey = false;
    //    hasFlowerGardenKey = false;
    //    hasSwordCaveKey = false;
    //    hasGuardianKey = false;
    //}

    public static void SetKeyActive(string key, bool active)
    {
        switch(key)
        {
            case "IpKey":
                if (active)
                    hasIpKey = true;
                else
                    hasIpKey = false;
                break;
            case "EdKey":
                if (active)
                    hasEdKey = true;
                else
                    hasEdKey = false;
                break;
            case "OceanKey":
                if (active)
                    hasOceanKey = true;
                else
                    hasOceanKey = false;
                break;
            case "TempleKey":
                if (active)
                    hasTempleKey = true;
                else
                    hasTempleKey = false;
                break;
            case "CarlKey":
                if (active)
                    hasCarlKey = true;
                else
                    hasCarlKey = false;
                break;
            case "BehindTempleKey":
                if (active)
                    hasBehindTempleKey = true;
                else
                    hasBehindTempleKey = false;
                break;
            case "DeadForestKey":
                if (active)
                    hasDeadForestKey = true;
                else
                    hasDeadForestKey = false;
                break;
            case "FlowerGardenKey":
                if (active)
                    hasFlowerGardenKey = true;
                else
                    hasFlowerGardenKey = false;
                break;
            case "SwordCaveKey":
                if (active)
                    hasSwordCaveKey = true;
                else
                    hasSwordCaveKey = false;
                break;
            case "GuardianKey":
                if (active)
                    hasGuardianKey = true;
                else
                    hasGuardianKey = false;
                break;
        }
    }
    public static bool HasMatchingKey(byte crateID)
    {
        return DetermineIfMatch(crateID); 
    }
    private static bool DetermineIfMatch(byte crateID)
    {
        switch(crateID)
        {
            case 0:
                if (hasIpKey)
                    return true;
                else
                    return false;
            case 1:
                if (hasEdKey)
                    return true;
                else
                    return false;
            case 2:
                if (hasOceanKey)
                    return true;
                else
                    return false;
            case 3:
                if (hasTempleKey)
                    return true;
                else
                    return false;
            case 4:
                if (hasCarlKey)
                    return true;
                else
                    return false;
            case 5:
                if (hasBehindTempleKey)
                    return true;
                else
                    return false;
            case 6:
                if (hasDeadForestKey)
                    return true;
                else
                    return false;
            case 7:
                if (hasFlowerGardenKey)
                    return true;
                else
                    return false;
            case 8:
                if (hasSwordCaveKey)
                    return true;
                else
                    return false;
            case 9:
                if (hasGuardianKey)
                    return true;
                else
                    return false;
            default :
                Debug.Log("Can't reference key - See KeyManager.cs");
                return false;
        }
    }
}
