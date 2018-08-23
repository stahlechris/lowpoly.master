using System;

public class DiscoveryEventArgs : EventArgs
{
	public string m_discoveryName;

	public DiscoveryEventArgs(string discoveryName)
    {
		m_discoveryName = discoveryName;
    }
}
