using UnityEngine;

public class Abduct : MonoBehaviour 
{
    [SerializeField]UFO ufo;
    //This is called by an animation frame that is triggered right when
    //the deer's torso enters the UFO. It makes it dissapear
    void GetAbducted()
    {
        ufo.Inform();
        Destroy(this.gameObject);
    }


}
