using UnityEngine;

/*TODO 
Make an object manager...he turns scripts on and off to reduce Update() computations
*/

public class Levitate : MonoBehaviour 
{
    public bool doLevitate = false;
    Vector3 levitate;
    Vector3 tempLevitate;
    public float frequency = 1f;
    public float amplitude = 0.5f;

	void Start () 
    {
        levitate = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (doLevitate)
        {
            tempLevitate = levitate;
            tempLevitate.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempLevitate;
        }
	}

}
