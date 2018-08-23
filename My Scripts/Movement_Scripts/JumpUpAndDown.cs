using System.Collections;
using UnityEngine;

public class JumpUpAndDown : MonoBehaviour 
{
    
    Animator m_animator;
    int timeBetweenJumps = 5;
    public bool playerHasTalkedToMe = false;
    public bool playerIsCloseToMe = false;
    public bool isJumping = false;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        StartCoroutine(JumpUpAndDownRepeatedly());
    }

    public IEnumerator JumpUpAndDownRepeatedly()
    {
        while(!playerHasTalkedToMe && !playerIsCloseToMe)
        {
            JumpOnce();
            JumpUpAndDownRepeatedly();
            yield return new WaitForSeconds(timeBetweenJumps);
        }
    }

    private void JumpOnce()
    {
        m_animator.SetTrigger("Jump");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
