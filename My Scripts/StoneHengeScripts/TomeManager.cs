using System.Collections;
using UnityEngine;
using LowPoly.Character;

public class TomeManager : MonoBehaviour 
{
    public StonehengeManager stonehengeManager;
    public Collider[] tomeColliders;
    public Levitate tomeLevitation;
    public void Handle_ReadingWithoutPermission(PlayerController player)
    {
        Debug.Log("Jesus, THIS IS NOT A DRILL, ALL TOMES DEACTIVATE!!!");
        DeactivateTomes(player);
        Debug.Log("WE'VE GOT A SITU-AY-TION!!!");
        stonehengeManager.Handle_ReadingWithoutPermission(player);
    }
    void DeactivateTomes(PlayerController player)
    {
        DisableMessagePanel(player);
        DisableColliders();
        StartDisablingLevitation();
    }

    void DisableColliders()
    {
        foreach (Collider c in tomeColliders)
        {
            Debug.Log("Deactivated " + c.name);
            c.enabled = false;
        }
    }
    void StartDisablingLevitation()
    {
        StartCoroutine(DisableLevitation());
    }
    IEnumerator DisableLevitation()
    {
        yield return new WaitForSeconds(3); //show levitation stopping when cameras switch
        //play disable sound fx
        tomeLevitation.doLevitate = false;
        tomeLevitation.enabled = false;
    }
    void DisableMessagePanel(PlayerController player)
    {
        player.hud.CloseMessagePanel();
        player.m_PersonRequestingToBeSpokenWith = null;
    }


}
