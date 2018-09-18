using System.Collections;
using UnityEngine;

public class CameraChanger : MonoBehaviour 
{
    public GameObject ThirdPersonCam; //0 
    public GameObject FirstPersonCam; //1 
    public int camMode;
	
	void Update () 
    {
        //TODO check on a timer, like in xbox controller script
        if(Input.GetButtonDown("Camera")) // "  \  "
        {
            ChangeCam(camMode);
        }
	}

    public void ChangeCam(int x)
    {//when you call ChangeCam, it will always be called when the camera is 0, or the player cam
        //assign the incoming value, redundancy ok here bc int
        camMode = x;

        if (camMode == 1) //only OnDialogueEnds call ChangeCam(1)
        {
            camMode = 0;
        }
        else
        {
            camMode += 1; //it's 0, increment to 1... aka cycle through cams
        }
        StartCoroutine(CamChange());
    }

    IEnumerator CamChange() 
    {
        yield return null;
        if(camMode == 0)
        {
            ThirdPersonCam.SetActive(true);
            FirstPersonCam.SetActive(false);
        }
        if(camMode == 1)
        {
            FirstPersonCam.SetActive(true);
            ThirdPersonCam.SetActive(false);
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
