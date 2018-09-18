using System.Collections;
using UnityEngine;
using LowPoly.CameraUI;
using System;

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


    public Transform camTransform;
    public Transform myTransform;
    public CameraArmFollow cameraArmFollow;
    public ThirdPerson_Camera thirdPerson_Camera;
    public ThirdPersonCameraMouseControls thirdPersonCameraMouse;

    bool inPosition = false;

    void Start()
    {
        myTransform = transform;
        DialogueEvents.OnDialogueEnd += Handle_OnDialogueEnd;

    }

    void Handle_OnDialogueEnd(Dialogue dialogueItem)
    {
        AdjustCameraForPlaying();
        cameraChanger.ChangeCam(1);
    }

    public void RotateCamera (Transform target)
    {
        AdjustCameraForSpeaking();
        StartCoroutine(SmoothRotate(target));
    }

    IEnumerator SmoothRotate(Transform target)
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
        cameraChanger.ChangeCam(0); //0 is speaker's cam. 1 is player's cam.
        audioSource.PlayOneShot(cameraLockSound);
    }


    void AdjustCameraForSpeaking() 
    {
        /* Reverse camera logic here to the old way => CameraArmFollow parent changes, camera stays fixed
        * Set const fixed values for camera. We have to do this short lil process to keep the whooshy camera 
        * panning effect.
        */

        //disable new system.
        thirdPerson_Camera.enabled = false;

        //Load old system values
        camTransform.localPosition = fixedCameraPositionVals;
        camTransform.localRotation = fixedCameraRotationVals;
        camTransform.localEulerAngles = new Vector3(20, 0, 0);
        //enable old system.
        cameraArmFollow.enabled = true;
    }
    void AdjustCameraForPlaying()
    {
        //disable old system.
        cameraArmFollow.enabled = false;
        //enable new system.
        thirdPerson_Camera.enabled = true;
    }

    void OnDisable()
    {
        DialogueEvents.OnDialogueEnd -= Handle_OnDialogueEnd;
    }
}


//we want the left (x) to go down --....up  (y) to go up ++ by about |15|