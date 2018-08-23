using System;

public class QuestEventArgs : EventArgs 
{
    public QuestBaseClass m_quest;

    public QuestEventArgs(QuestBaseClass quest)
    {
        m_quest = quest;
    }
}
