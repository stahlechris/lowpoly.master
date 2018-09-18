using UnityEngine;
using System.Collections;
using LowPoly.Character;

/*Earth - shield
 * Water - heal
 * Fire - AOE bomb (instakill)
 * Air - Unlimited sprint
 */
public class EyeGuard : Interactable 
{
    public AudioSource audioSource;
    public AudioClip descendingSound;
    public AudioClip gettingPokedSound;
    const string PLAYER_NAME = "Liam";
    bool hasEntered = false;
    PlayerController player;
    bool hasBlinkedUnapprovingly;
    bool hasPokedMeBeforeReading = false;
    public GameObject canvas;
    public StonehengeManager stonehengeManager;
    public SphereCollider sphereCollider;
    public BoxCollider boxCollider;
    public EyeGuard_UI my_UI;
    public Animator animator;
    public ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == PLAYER_NAME && !hasEntered)
        {
            hasEntered = true;
            if (player == null)
            {
                player = other.GetComponent<PlayerController>();
            }

            player.m_PersonRequestingToBeSpokenWith = this.gameObject;
            player.hud.OpenMessagePanel(this.gameObject, null);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == PLAYER_NAME)
        {
            hasEntered = false;
            player.hud.CloseMessagePanel();

            player.m_PersonRequestingToBeSpokenWith = null;

        }
    }

    public override void Interact()
    {
        //You may or may not interact with the eyeguard only once and he did NOT consent
        if(!hasBlinkedUnapprovingly)
        {
            //play a cutscene showing liam poke the eye
            //make the player poke him (player literally could not resist this eye is meant to be poked
            Debug.Log("Wtf u poked me??");
            hasPokedMeBeforeReading = true;
            //BlinkUnapprovingly();
            stonehengeManager.Handle_PokingTheEyeguard(gettingPokedSound, player);

        }
    }

    public void BlinkUnapprovingly()
    {
        my_UI.PrepareText();
        StartCoroutine(ShowText());
        StartCoroutine(DescendIntoGround());
    }

    IEnumerator ShowText()
    {
        canvas.SetActive(true);
        hasBlinkedUnapprovingly = true;
        yield return new WaitForSeconds(3);
        canvas.SetActive(false);
    }
    IEnumerator DescendIntoGround()
    {
        PlayRumbleSoundFX();
        yield return new WaitForSeconds(3);
        animator.SetTrigger("Descend");
        stonehengeManager.ShakeCamera(7.5f);

        PlayDustParticleFX();
    }

    void PlayDustParticleFX()
    {
        particles.Play();

    }
    void PlayRumbleSoundFX()
    {
        audioSource.PlayDelayed(2f);
    }
   

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
