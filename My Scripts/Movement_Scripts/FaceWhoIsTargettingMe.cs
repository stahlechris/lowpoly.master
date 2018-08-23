using UnityEngine;

//TODO: Lock x axis rotation
public class FaceWhoIsTargettingMe : MonoBehaviour 
{
    public Transform target;
    public bool doFaceTarget = true;

    //The thought is, If I am looking at the player, then my Dialogue is too.
    void FixedUpdate()
    {
        if (doFaceTarget)
        {
            //float previousYPosition = transform.eulerAngles.y;
            Vector3 targetPosition = new Vector3(target.position.x,
                                                     this.transform.position.y,
                                                     target.position.z);
            this.transform.LookAt(targetPosition);
            //float instantaneousSpeed = Mathf.Abs(transform.eulerAngles.y - previousYPosition);
        }
    }


}
