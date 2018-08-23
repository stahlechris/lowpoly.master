using UnityEngine;

public class Teleporter : MonoBehaviour 
{

    public Vector3 destination;

    void OnTriggerEnter(Collider other)
    {
        Teleport(other.transform);
    }

    void Teleport(Transform target )
    {
        target.position = destination;
    }
}
