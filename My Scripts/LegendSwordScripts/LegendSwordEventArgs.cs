using System;

public class LegendSwordEventArgs : EventArgs
{
    public string m_LegendSwordItem;

    public LegendSwordEventArgs(string legendSwordItem)
    {
        m_LegendSwordItem = legendSwordItem;
    }
}