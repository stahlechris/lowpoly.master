using UnityEngine;

public class FaceWhoIsTargettingMe : MonoBehaviour 
{
    Transform m_Transform;
    public Transform target;
    public bool doFaceTarget = true;

    void Awake()
    {
        m_Transform = transform;
    }
    //The thought is, If I am looking at the player, then my Dialogue is too.
    void FixedUpdate()
    {
        if (doFaceTarget)
        {
            //float previousYPosition = transform.eulerAngles.y;
            Vector3 targetPosition = new Vector3(target.position.x,
                                                 m_Transform.position.y,
                                                     target.position.z);
           m_Transform.LookAt(targetPosition);
            //float instantaneousSpeed = Mathf.Abs(transform.eulerAngles.y - previousYPosition);
            //Above is used to determine how fast target is circling this.
        }
    }

    /*TODO: If target is outside of X meters, every x seconds readjust to face target.
     * If target is within X meters, adjust every fixedUpdate frame.
     * 
     */
}
