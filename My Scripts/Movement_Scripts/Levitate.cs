using UnityEngine;

/*TODO 
Make an object manager...he turns scripts on and off to reduce Update() computations
*/

public class Levitate : MonoBehaviour 
{
    Transform m_Transform;
    public bool doLevitate = false;
    Vector3 levitate;
    Vector3 tempLevitate;
    public float frequency = 1f;
    public float amplitude = 0.5f;

    void Start () 
    {
        m_Transform = transform;
        levitate = m_Transform.position;
	}
	
	// Update is called once per frame
	void Update () //has to be in update to look smooth computers r really fast at math tho, so its fine
    {
        if (doLevitate)
        {
            tempLevitate = levitate;
            tempLevitate.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            m_Transform.position = tempLevitate;
        }
	}

}
