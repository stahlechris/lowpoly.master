using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EZCameraShake;

public class SpaceSheep : MonoBehaviour 
{
    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject canvasMessage;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] CenterPoint centerPoint;
    [SerializeField] CameraChanger cameraChanger;
    [SerializeField] SetCamera setCamera;
    [SerializeField] GameObject SpaceKey;
    [SerializeField] ParticleSystem Explosion;
    [SerializeField] GameObject SheepCam;
    [SerializeField] CameraShakeInstance instance;
    [SerializeField] AudioClip clip;
    const string MESSAGE_ONE = "BAAAA";
    const string MESSAGE_TWO = "BAAAAAAAA";
    const string MESSAGE_THREE = "BAAAAAAAAAA!!!";
    const string MESSAGE_FOUR = "BAAAAAAAAAAAAAAAAAAAAAAAA!!!";

    bool HasJumped { get; set; }
    bool HasJumpedTwice { get; set; }
    bool HasJumpedThrice { get; set; }
    
    bool HasGivenKey { get; set; }
    float totalTime { get; set; }

    public void InformPlayerHere()
    {
        canvasMessage.SetActive(true);
        StartCoroutine(WaitForCamera());
    }
    public void InformPlayerJumped()
    {
        if(!HasJumped && !HasJumpedTwice && !HasJumpedThrice)
        {
            NPC_SFX.Instance.PlaySFX(clip, 0.75f, 0.5f, false);
            HasJumped = true;
            centerPoint.HasJumped = true;
            tmp.fontSize += 3;
            tmp.SetText(MESSAGE_TWO);
        }
        else if(HasJumped && !HasJumpedTwice && !HasJumpedThrice)
        {
            NPC_SFX.Instance.PlaySFX(clip, 0.75f, 0.75f, false);
            HasJumpedTwice = true;
            centerPoint.HasJumpedTwice = true;
            tmp.fontSize += 3;

            tmp.SetText(MESSAGE_THREE);
            canvasMessage.SetActive(true);
        }
        else if(HasJumped && HasJumpedTwice && !HasJumpedThrice)
        {
            NPC_SFX.Instance.PlaySFX(clip, 0.75f, 1, false);
            centerPoint.enabled = false; //don't let the behavior run anymore
            HasJumpedThrice = true;
            centerPoint.HasJumpedThrice = true;
            tmp.color = new Color(220,20,60);
            tmp.fontSize += 3;
            ShakeCamera(3f);
            tmp.SetText(MESSAGE_FOUR);
            canvasMessage.SetActive(true);
            StartCoroutine(ActivateSpaceKey());
        }
    }

    IEnumerator ActivateSpaceKey()
    {
        if(!HasGivenKey)
        {
            yield return new WaitForSeconds(1.5f);
            HasGivenKey = true;
            Explosion.Play();
            SpaceKey.AddComponent<Rigidbody>();//have the key fall down
            StartCoroutine(KeySpin());//make it do a lil dance
        }
    }
    IEnumerator KeySpin()
    {
        float spinDuration = 3;
        totalTime = 0;
        float speed = 50f;

        while(totalTime <= spinDuration)
        {
            totalTime += Time.deltaTime;
            SpaceKey.transform.Rotate(Vector3.up, speed * Time.deltaTime);
            yield return null;
        }
        DestroySystem();
    }
    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(0.5f);
        cameraChanger.FirstPersonCam = SheepCam;
        setCamera.RotateCamera(this.transform);
    }
    #region Camera Shake Methods
    public void ShakeCamera(float duration)
    {
        StartCoroutine(ShakeTheCameraRoutine(duration));
    }
    IEnumerator ShakeTheCameraRoutine(float duration)
    {
        instance = CameraShaker.Instance.StartShake(1f, 1f, 0.25f);
        yield return new WaitForSeconds(duration);
        instance.StartFadeOut(0.5f);
    }
    #endregion
    void DestroySystem()
    {
        //manually switch camera control over
        cameraChanger.ChangeCam(1);
        setCamera.AdjustCameraForPlaying();
        //end cam switch
        canvasMessage.SetActive(false);
        //make the sheep start floating away
        parentObject.AddComponent<Rigidbody>().useGravity = false;
        parentObject.GetComponent<Rigidbody>().AddForce(0.3f, 0.4f, 0.3f, ForceMode.Impulse);
        //destroy this script
        Destroy(SpaceKey.GetComponent<Rigidbody>());
        Destroy(this);
    }
    public void Reset()
    {
        canvasMessage.SetActive(false);
        setCamera.AdjustCameraForPlaying();
        HasJumped = false;
        HasJumpedTwice = false;
        HasJumpedThrice = false;
    }
}
