using UnityEngine;
using LowPoly.Character;
using System.Collections;
using EZCameraShake;
public class StonehengeManager : MonoBehaviour 
{
    public bool HasCircledMe { get; set; }
    bool IsWalkingBackwardsToAvoidMyGaze { get; set; }
    bool HasWalkedBackwardsToAvoidMyGaze { get; set; }
    bool HasHitMeWithWeapon { get; set; }
    bool IsUsingShieldToBlockMyLazer { get; set; }
    bool HasUsedShieldToBlockMyLazer { get; set; }
    bool HasAttemptedToLootATome { get; set; }

    bool HasLegendarySword { get; set; }

    public AudioSource stonehenge_audiosource;
    public Levitate tomesLevitation;
    public Levitate rocksLevitation;
    public Transform floatingRocks;
    public Animator spaceCubeAnimator;
    public EyeGuard eyeGuard;
    public SpaceCube spaceCube;
    public GameObject stonehengeCam;
    public GameObject eyeguardPokeCam;
    public GameObject MAIN_CAMERA;
    public Quest stonehengeQuest;
    CameraShakeInstance instance;
    public QuestList playersQuestLog;


    //TODO: Seperate the two cutscenes into different classes, only call from here
    public void Handle_ReadingWithoutPermission(PlayerController player)
    {
        Debug.Log("Situation received. Grabbing hold of this player");
        PlayEyeguardCutsceneNoPoke(player);
        //play sound fx

        //tell the eyeguard to act
        // play "cutscene" showing the eyeguard and turning his text on 
    }
    void PlayEyeguardCutsceneNoPoke(PlayerController player)
    {
        DisablePlayer(player, 16.5f);
        RotatePlayerTowardsTheEyeguard(player);
        EnableStonehengeCam(true);
        eyeGuard.BlinkUnapprovingly();
        ActivateDefenses(player);
    }

    public void Handle_PokingTheEyeguard(AudioClip gettingPokeSound, PlayerController player)
    {
        Debug.Log("Eyeguard told me you poked him??");
        PlayPokingEyeCutscene(gettingPokeSound, player);
    }
    public void PlayPokingEyeCutscene(AudioClip audio, PlayerController player)
    {
        DisablePlayer_eyeguard(player, 16.5f);
        EnableEyeguardCam(true);
        SetPlayerPositioning_eyeguardPoke(player);
        RotatePlayerTowardsTheEyeguard(player);
        PokeTheEyeguard(player, audio);
        eyeGuard.BlinkUnapprovingly();
        ActivateDefenses(player);
    }
    void EnableEyeguardCam(bool active)
    {//will give control to the stonehengecam on false arg
        if (active)
        {
            MAIN_CAMERA.SetActive(false);
            eyeguardPokeCam.SetActive(true);
        }
        else
        {
            stonehengeCam.SetActive(true);
            eyeguardPokeCam.SetActive(false);
        }
        
    }
    void SetPlayerPositioning_eyeguardPoke(PlayerController player)
    {
        //253.5, 31.3, 195.7
        player.transform.position = new Vector3(253.5f, 31.3f, 195.7f);
    }
    void DisablePlayer_eyeguard(PlayerController player, float duration)
    {
        Global.USER_INPUT_ENABLED = false;
        player.animator.SetFloat("Forward", 0.0f);
        player.animator.SetFloat("JumpLeg", 0.0f);

        Rigidbody playersRb = player.GetComponent<Rigidbody>();
        playersRb.constraints = RigidbodyConstraints.FreezePosition;

        playersRb.constraints = RigidbodyConstraints.FreezeRotation;
        StartCoroutine(StartEnablingPlayer(player, duration));
    }
    void PokeTheEyeguard(PlayerController player, AudioClip audioclip)
    {
        player.animator.SetTrigger("ReachForward");
        stonehenge_audiosource.PlayOneShot(audioclip,1.5f);
    }
    void RotatePlayerTowardsTheEyeguard(PlayerController player)
    {
        Transform playersTransform = player.GetComponent<Transform>();
        Transform target = eyeGuard.GetComponent<Transform>();
        playersTransform.LookAt(target); //im the captain now
    }
    void DisablePlayer(PlayerController player, float duration)
    {
        Global.USER_INPUT_ENABLED = false;
        player.animator.SetFloat("Forward", 0.0f);
        player.animator.SetFloat("JumpLeg", 0.0f);

        Rigidbody playersRb = player.GetComponent<Rigidbody>();
        playersRb.constraints = RigidbodyConstraints.FreezePosition;

        playersRb.constraints = RigidbodyConstraints.FreezeRotation;
        StartCoroutine(StartEnablingPlayer(player, duration));
        player.animator.SetBool("IsBlocking", true);
    }
    IEnumerator StartEnablingPlayer(PlayerController player, float duration)
    {
        yield return new WaitForSeconds(duration);
        EnablePlayer(player);
    }
    void EnablePlayer(PlayerController player)
    {
        Global.USER_INPUT_ENABLED = true;
        player.transform.rotation = Quaternion.identity;
        Rigidbody playersRigidBody = player.GetComponent<Rigidbody>();
        playersRigidBody.constraints = RigidbodyConstraints.None;//unfreeze
        playersRigidBody.constraints = RigidbodyConstraints.FreezeRotation;//refreeze to ground
        player.animator.SetBool("IsBlocking", false);
        AssignStonehengeQuest();
    }
    void ActivateDefenses(PlayerController player)
    {
        spaceCube.AcceptPlayerReference(player);
        StartCoroutine(AriseSpaceCube());
    }
    IEnumerator AriseSpaceCube()
    {
        yield return new WaitForSeconds(10);
        //There's an animation event that enables all of the space cube's behaviors
        spaceCubeAnimator.SetTrigger("Activate");
    }
    public void EnableStonehengeCam(bool active)
    {
        //Sets active Main cam or stonehenge cam
        if(active)
        {
            MAIN_CAMERA.SetActive(false);
            stonehengeCam.SetActive(true);
        }
        else
        {
            MAIN_CAMERA.SetActive(true);
            stonehengeCam.SetActive(false);
        }
    }

    void AssignStonehengeQuest()
    {
        StartCoroutine(AssignQuest());
    }
    IEnumerator AssignQuest()
    {
        yield return new WaitForSeconds(3);
        stonehengeQuest.enabled = true;
        playersQuestLog.AddQuestItem(stonehengeQuest);
    }

    public void ShakeCamera(float duration)
    {
        StartCoroutine(ShakeTheCameraRoutine(duration));
    }
    IEnumerator ShakeTheCameraRoutine(float duration)
    {
        instance = CameraShaker.Instance.StartShake(3f, 3f, 0.5f);
        yield return new WaitForSeconds(duration);
        instance.StartFadeOut(0.5f);
    }


}
