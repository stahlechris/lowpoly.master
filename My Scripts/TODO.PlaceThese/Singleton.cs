using UnityEngine;

/*This is a Singleton template. 
 * 
 * Define Singleton: A Singleton is a software design pattern that restricts 
 * the instantiation of a class to one object.
 * 
 * 

 * abstract - We need an implementation of this class to be able to instantiate it.
 * 
 *  <T> We want to only be able to generate an implementation of this type (<T>)
 *  if the Monobehavior implementation is also of this type (<T>). This is "typesafe" now.
 */
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return Instance;
        }
    }

    /*   Because we potentially want to do something in Awake() in the implementations of this class, 
     *   we cannot make it private, but protected, so implementations have access to it
     *   make it virtual so implementating classes can have access to create their own version of the method.
     */
    protected virtual void Awake()
    {
        if(instance != null)
        {
            //It has been instantiated already...
            //Prevent a new instantiation being created by Destroying it.
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}