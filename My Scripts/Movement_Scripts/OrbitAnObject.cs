using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAnObject : MonoBehaviour 
{
    Transform m_Transform;
    public GameObject sphere;
    public Transform center;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 30f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = -20f;

	void Start () 
    {
        m_Transform = transform;
        //sphere = transform.Find("RotationPoint").GetComponent<GameObject>();
        center = sphere.transform;
        m_Transform.position = (m_Transform.position - center.position).normalized * radius + center.position;
	}

    private void FixedUpdate()
    {
        m_Transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (m_Transform.position - center.position).normalized * radius + center.position;
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, desiredPosition, Time.deltaTime * radiusSpeed );

    }


}
