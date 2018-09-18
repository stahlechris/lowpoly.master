using UnityEngine;

public class BouncyStep : MonoBehaviour 
{
    public BouncyStepManager manager;
    public Rigidbody rb;
    /*Even if GameObjects are turned off, they still check for collisions - 
     * So add a bool to control if they run or not
    */
    public bool m_doOnTrigger = true;
    public float forwardThrust = 1f;
    public float upwardThrust = 1f;

    void OnTriggerEnter(Collider other)
    {
        if(m_doOnTrigger)
        {
            if(other.CompareTag("Player")) 
            {
                //If you are bouncing different objects, get the component:
                //rb = other.GetComponent<RigidBody>()
                Bounce();
            }
        }
    }
    void Bounce()
    {
        //Zero out the RigidBody so we have a consistant jump
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //Debug.Log(rb.velocity);
        //Based on player's forward, apply a forward & upward force to his rb
        rb.AddRelativeForce(new Vector3(0, upwardThrust, 0) + ((Vector3.forward) * forwardThrust), ForceMode.Impulse);
        //Tell manager to play a bounce sound from the singular AudioSource
        manager.PlayAudio();
    }
}
