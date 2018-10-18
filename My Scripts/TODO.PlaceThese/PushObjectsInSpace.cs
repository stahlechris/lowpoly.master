using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectsInSpace : MonoBehaviour 
{
    public Vector3 force;
	// Use this for initialization
	void Start ()
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}
	
}
