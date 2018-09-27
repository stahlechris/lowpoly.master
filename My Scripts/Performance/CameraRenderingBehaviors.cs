using UnityEngine;

public class CameraRenderingBehaviors : MonoBehaviour 
{
    ParticleSystem me;
    //sheep wandering script
    //lookAt scripts
    //enemyDistance checking scripts 
    //dialogue scripts 

    //make sure to add things to OnEnable and Disable so they work when off/oned

    void Start()
    {
        me = GetComponent<ParticleSystem>();
    }
    //TODO...figure out why these are not working as expected.
    //Ex - it says its visible when THE PLAYER can't see it 



    //TODO Get rid of this shit. It works like shit.. Make a distance checker.
    void OnBecameVisible()
    {
        //Debug.Log(this + "started playing");
        me.Play();
    }
    void OnBecameInvisible()
    {
        //Debug.Log(this + "stopped playing");
        me.Stop();
    }

}
