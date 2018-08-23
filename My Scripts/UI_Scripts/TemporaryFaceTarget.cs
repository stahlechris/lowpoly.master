using UnityEngine;

public class TemporaryFaceTarget : MonoBehaviour 
{
    public Transform target;
    public bool doFaceTarget = true;

    void FixedUpdate()
    {
        if (doFaceTarget)
        {
            Vector3 targetPosition = new Vector3(target.position.x,
                                                     this.transform.position.y,
                                                     target.position.z);
            this.transform.LookAt(targetPosition);
        }
    }
}
