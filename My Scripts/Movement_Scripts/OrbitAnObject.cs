using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAnObject : MonoBehaviour 
{
    public GameObject sphere;
    public Transform center;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 30f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = -20f;

	void Start () 
    {
        //sphere = transform.Find("RotationPoint").GetComponent<GameObject>();
        center = sphere.transform;
        transform.position = (transform.position - center.position).normalized * radius + center.position;
	}

    private void FixedUpdate()
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed );

    }


}
