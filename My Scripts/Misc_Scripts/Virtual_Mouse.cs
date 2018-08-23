using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

/*Checks for new inputs every time seconds.
 */
public class Virtual_Mouse : MonoBehaviour 
{
    public Texture2D crosshairImage;
    public Rect position;
    bool xboxControllerPluggedIn;
    float time = 5;

	void Update () 
    {
        if(xboxControllerPluggedIn)
        {
            //position = new Rect((Screen.width - crosshairImage.width) / 2,
            //(Screen.height - crosshairImage.height) / 2,
            //crosshairImage.width, crosshairImage.height);

            //now we check if they're cycling through the quests
            if(CrossPlatformInputManager.GetButtonDown("CycleQuests"))
            {
                //iterate through the questcanvaschildren that have quests

            }
        }
        time -= Time.deltaTime;
        if (time < 1) //must keep checking for when it is unplugged;
        {
            ResetTimer();
            CheckForXboxController();
        }
	}
    //private void OnGUI()
    //{
    //    if (xboxControllerPluggedIn)
    //    {
    //        GUI.DrawTexture(position, crosshairImage);
    //    }
    //}

    void CheckForXboxController()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            xboxControllerPluggedIn = true;
            Global.XBOX_CONTROLLER_PLUGGED_IN = true;
        }
        else
        {
            xboxControllerPluggedIn = false;
            Global.XBOX_CONTROLLER_PLUGGED_IN = false;
        }
    }
    void ResetTimer()
    {
        time = 5;
    }
}



/* OnButtonPress left bumper cycle through quests, 
 * while hold the left bumper - view quest description.
 * 
 */