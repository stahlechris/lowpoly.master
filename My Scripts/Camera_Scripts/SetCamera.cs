using System.Collections;
using UnityEngine;

public class SetCamera : MonoBehaviour 
{
    public AudioClip cameraRotateSound; //~ 2sec in length
    public AudioClip cameraLockSound;   //~ < 1 sec in length
    [Tooltip("The AudioSource this behavior will use to play its AudioClips")]
    public AudioSource audioSource;
    [Tooltip("How long (sec) does it take for your rotation to finish?")]
    public float rotationSpeed = 2f;
    [Tooltip("Over how many frames should this rotation happen?")]
    public int frameRate = 50;
    internal bool isFacingTarget;

    public void RotateCamera (Transform target)
    {
        StartCoroutine(SmoothRotate(target));
        //StartCoroutine(LookUpALittle());
    }
    IEnumerator SmoothRotate(Transform target)
    {
        //Play a slow whoooooosh turning sound
        audioSource.PlayOneShot(cameraRotateSound);

        //Finish rotation in ~frameRate frames or less
        for (int i = 0; i < frameRate; i++)
        {
            //Frame by frame positioning value
            Vector3 targetDir = target.position - transform.position;
            //Every frame for frameRate frames, adjust our rotation value.
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(targetDir), (rotationSpeed * Time.deltaTime) );
            //wait till the end of the frame
            yield return null;
        }
        //Play a "Camera is Set" sound
        isFacingTarget = true;
        audioSource.PlayOneShot(cameraLockSound);
    }

    //TODO figure this rotation quaternion angle direction bullshit out
    IEnumerator LookUpALittle()
    {
        yield return new WaitUntil(() => isFacingTarget == true);
        for (int i = 0; i < 10; i++)
        {
            //Create a rotation that rotates X degrees around the defined axis
            //Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.left);

            Quaternion newRotation = Quaternion.Euler(new Vector3( -50, 0, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);

            yield return null;
        }
        isFacingTarget = false;
    }
}


//we want the left (x) to go down --....up  (y) to go up ++ by about |15|