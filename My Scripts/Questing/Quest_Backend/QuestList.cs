using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestList : MonoBehaviour
{
    IList<QuestBaseClass> questList = new List<QuestBaseClass>();
    AudioSource my_audioSource;
    public AudioClip startQuestSound;
    public AudioClip endQuestSound;

    public event EventHandler<QuestEventArgs> OnQuestAdded;
    public event EventHandler<QuestEventArgs> OnQuestRemoved; //consider an OnQuestUpdated static listener 

    void Start()
    {
        my_audioSource = GetComponent<AudioSource>();
    }
    public QuestBaseClass SearchQuestList(int id)
    {
        if(questList != null)
        {
            foreach (QuestBaseClass q in questList)
            {
                if (q.Quest_ID == id)
                {
                    return q;
                }
            }
        }
        return null;
    }
    public void AddQuestItem(QuestBaseClass questItem)
    {
        //questList.Add(questItem);
        if (OnQuestAdded != null)
        {
            questList.Add(questItem);
            //Debug.Log("Quest added " + questItem);
            OnQuestAdded(this, new QuestEventArgs(questItem));
            my_audioSource.PlayOneShot(startQuestSound);
            //TESTER_SHOW_ALL_QUESTS_IN_LIST();
        }
    }
    public void RemoveQuestItem(QuestBaseClass questItem)
    {
        foreach (QuestBaseClass quest in questList)
        {
            //Debug.Log("Found " + quest + " in questList while searching to remove quest item");
            if (OnQuestRemoved != null)
            {
                if(quest.QuestName == questItem.QuestName)
                {
                    questList.Remove(questItem);
                    //Debug.Log("OnQuestRemoved NOT null " + quest.QuestName + questItem.QuestName + " l = in list, r = argument");
                    OnQuestRemoved(this, new QuestEventArgs(questItem));
                    //TESTER_SHOW_ALL_QUESTS_IN_LIST();
                    my_audioSource.PlayOneShot(endQuestSound);
                    break;
                }
            }
            //Debug.Log(this + "OnQuestRemove is Null, cannot remove");
            //break;
        }
    }
    public void TESTER_SHOW_ALL_QUESTS_IN_LIST()
    {
        foreach (QuestBaseClass q in questList)
        {
            Debug.Log(("QuestList has " + q));
        }
    }
}
