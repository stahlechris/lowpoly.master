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
            if(camMode == 1)
            {
                camMode = 0;
            }
            else
            {
                camMode += 1; //cycle through cam modes
            }
            StartCoroutine(CamChange());
        }
	}

    IEnumerator CamChange() 
    {
        yield return new WaitForSeconds(0.01f);
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
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
