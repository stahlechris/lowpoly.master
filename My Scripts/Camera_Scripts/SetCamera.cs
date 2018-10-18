using System.Collections;
using UnityEngine;
using LowPoly.CameraUI;


public class SetCamera : MonoBehaviour 
{
    public CameraChanger cameraChanger;
    public AudioClip cameraRotateSound; //~ 2sec in length
    public AudioClip cameraLockSound;   //~ < 1 sec in length
    [Tooltip("The AudioSource this behavior will use to play its AudioClips")]
    public AudioSource audioSource;
    [Tooltip("How long (sec) does it take for your rotation to finish?")]
    public float rotationSpeed = 2f;
    [Tooltip("Over how many frames should this rotation happen?")]
    public int frameRate = 50;
    internal bool isFacingTarget;


    Vector3 fixedCameraPositionVals = new Vector3(0, 1.6f, -1.44f);
    Quaternion fixedCameraRotationVals = Quaternion.identity;


    Quaternion rotationOfCameraBeforeSpeaking;

    public Transform camTransform;
    public Transform myTransform;
    public CameraArmFollow cameraArmFollow;
    public ThirdPerson_Camera thirdPerson_Camera;
    public ThirdPersonCameraMouseControls thirdPersonCameraMouse;

    private void OnEnable()
    {
        myTransform = transform;
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;
    }

    void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        Debug.Log("set camera heard dialogue ended, giving control back to players camera");
        AdjustCameraForPlaying();
        cameraChanger.ChangeCam(1);
    }

    //This method is called from the PlayerController upon an Interaction
    public void RotateCamera (Transform target) //remove all these dependencies and have the player send us the GO that they're interacting with so we can deduce if we want the camera to
    {
        //cache the rotation the speaker initiated the conversation in so we can put it back how we found it
        rotationOfCameraBeforeSpeaking = camTransform.localRotation;

        AdjustCameraForSpeaking(target);
        StartCoroutine(SmoothRotate(target));
    }

    IEnumerator SmoothRotate(Transform target)
    {
        if (!target.CompareTag("HelpfulFish") )
        {
            //Play a slow whoooooosh turning sound
            audioSource.PlayOneShot(cameraRotateSound);
            //Finish rotation in ~frameRate frames or less
            for (int i = 0; i < frameRate; i++)
            {
                //Frame by frame positioning value
                Vector3 targetDir = target.position - myTransform.position;
                //Every frame for frameRate frames, adjust our rotation value.
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                Quaternion.LookRotation(targetDir), (rotationSpeed * Time.deltaTime));
                //wait till the end of the frame
                yield return null;
            }
            //Play a "Camera is Set" sound
            isFacingTarget = true;

            cameraChanger.ChangeCam(0); //0 is always speaker's cam. 1 is always player's cam.

            audioSource.PlayOneShot(cameraLockSound);
        }
    }


    void AdjustCameraForSpeaking(Transform target) 
    {
        /* Reverse camera logic here to the old way => CameraArmFollow parent changes, camera stays fixed
        * Set const fixed values for camera. 
        * 
        * Explain: I implemented a new 3rd person camera, but this code was written with my old camera system installed.
        * We temporary uninstall the new 3rd person camera system and reinstall the old for this to work.
        */

        //We do not want to go to a camera of the fishes or the cauldrons, we just want to interact with the camera
        if (!target.CompareTag("HelpfulFish") )
        {
            //disable new system.
            thirdPerson_Camera.enabled = false;

            //Load old system values
            camTransform.localPosition = fixedCameraPositionVals;
            camTransform.localRotation = fixedCameraRotationVals;
            camTransform.localEulerAngles = new Vector3(20, 0, 0);
            //enable old system.
            cameraArmFollow.enabled = true;
        }
    }
    public void AdjustCameraForPlaying()
    {
        //disable old system. That we temporarily turned on so the camera could pan and make a swooshy noise
        cameraArmFollow.enabled = false;
        //enable new system.
        thirdPerson_Camera.enabled = true;
        //give cached rotation back
        rotationOfCameraBeforeSpeaking = camTransform.localRotation;

    }

    void OnDisable()
    {
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }
}

