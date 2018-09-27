using UnityEngine;

public class RelayAnimationEvent : MonoBehaviour 
{
    public MainMenu mainMenu;

    public void AllowGameToLoad()
    {
        mainMenu.AllowedToLoad = true;
    }
}
