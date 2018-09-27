using UnityEngine;
using System.Collections;

public class LegendSwordManager : MonoBehaviour 
{
    public AudioSource my_audioSource;
    Transform[] swordParts;
    public GameObject levitatingSwordParts;
    public GameObject cameraArm;
    public GameObject templeCam;

    void Start()
    {
        swordParts = GetComponentsInChildren<Transform>();
        my_audioSource = GetComponent<AudioSource>();
        LegendSwordEvents.OnLegendarySwordPieceLooted += Handle_OnLegendarySwordPieceLooted;
        LegendSwordEvents.OnLegendarySwordPieceDropped += Handle_OnLegendarySwordPieceDropped;
    }

    private void Handle_OnLegendarySwordPieceLooted(string legendSwordItemName)
    {
        SearchForSwordPiece(legendSwordItemName);
    }
    private void Handle_OnLegendarySwordPieceDropped(string legendSwordItemName)
    {
        Debug.Log(legendSwordItemName + " dropped");
        //show the temple, no light, not floating
        //Reparent the object
        //my_audioSource.Play();
    }
    void SearchForSwordPiece(string legendSwordItemName)
    {
        for (int i = 1; i< swordParts.Length-1;i++)
        {
            if(legendSwordItemName.Equals(swordParts[i].name))
            {
                //turn temple camera on for x seconds to show the piece activated
                //reparent the object into the floating group to make it levitate
                swordParts[i].SetParent(levitatingSwordParts.transform);

                StartCoroutine(ShowTemple(swordParts[i]));
                break;
            }
        }
    }
    IEnumerator ShowTemple(Transform swordPart)
    {
        cameraArm.SetActive(false);
        templeCam.SetActive(true);
        templeCam.GetComponent<Animator>().SetTrigger("ShowSword");
        swordPart.GetComponent<Light>().enabled = true;
        my_audioSource.Play();
        yield return new WaitForSeconds(5f);
        swordPart.GetComponent<Light>().enabled = false;
        templeCam.SetActive(false);
        cameraArm.SetActive(true);
    }

    //void ShowLegendPieceCollected
    //void ShowLegendPieceDropped

    private void OnDisable()
    {
        LegendSwordEvents.OnLegendarySwordPieceLooted -= Handle_OnLegendarySwordPieceLooted;
        LegendSwordEvents.OnLegendarySwordPieceDropped -= Handle_OnLegendarySwordPieceDropped;
        StopAllCoroutines();
    }
}
