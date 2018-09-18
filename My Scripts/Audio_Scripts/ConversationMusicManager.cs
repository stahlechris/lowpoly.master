using UnityEngine;
using System.Collections;
using System;

public enum MusicType
{ //TODO: figure out enum conversion stuff
    Ip =0,
    Ed =1,
    Carl=2,
    HelpfulFish=3,
    Bird=4,
    GuitarByTheTree=5

}
public class ConversationMusicManager : MonoBehaviour 
{
    public AudioSource audioSource;
    public AudioClip Ip;
    public AudioClip Ed;
    public AudioClip Carl;
    public AudioClip HelpfulFish;
    public AudioClip Bird;
    public AudioClip GuitarByTheTree;

    bool Active { set; get; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DialogueEvents.OnDialogueStart += Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
    }

    void Handle_OnDialogueStart(Dialogue dialogue)
    {
        PlayConversationMusic(dialogue);
    }

    void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        FadeOutConversationMusic();
    }

    public void PlayConversationMusic(Dialogue dialogue)
    {
        ParseDialogueID(dialogue);
    }

    internal void ParseDialogueID(Dialogue dialogue)
    {
        //comes back as "Ed"
        string stringID = StringHelperClass.ParseDialogueIDForString(dialogue);
        SearchForMusicByStringID(stringID);
    }
    internal void SearchForMusicByStringID(string id)
    { //TODO, use this to have npc's have an AudioClip rather an AudioSource on them
        switch(id)
        {
            case "Ip":
                if(audioSource!= null && Ip != null)
                    audioSource.PlayOneShot(Ip);
                break;
            case "Ed":
                if (audioSource != null && Ed != null)
                    audioSource.PlayOneShot(Ed);
                break;
            case "Carl":
                if (audioSource != null && Carl != null)
                    audioSource.PlayOneShot(Carl);
                break;
            case "HelpfulFish":
                if (audioSource != null && HelpfulFish != null)
                    audioSource.PlayOneShot(HelpfulFish);
                break;
            case "Bird":
                if (audioSource != null && Bird != null)
                    audioSource.PlayOneShot(Bird);
                break;
            case "GuitarByTheTree":
                if (audioSource != null && GuitarByTheTree != null)
                    audioSource.PlayOneShot(GuitarByTheTree);

                break;
            default:
                Debug.Log("Invalid dialogue id after parsing! cant play convo music");
                break;
        }
    }
    public void FadeOutConversationMusic()
    {
        StartCoroutine(FadeOutMusic());
    }
    IEnumerator FadeOutMusic()
    {
        while(audioSource.volume > 0.001f) //1 is 100% loudness
        {
            yield return new WaitForSeconds(0.1f);
            audioSource.volume -= 0.025f;
        }
    }

    void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= Handle_OnDialogueStart;
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }
}
