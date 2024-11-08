using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour 
    // abstract classes can't be instantiated directly but you can have other classes inherit from them, class is generic, allows you to pass in other class type <T>
    // We are also making sure any class we pass in here as part of the generic definition is also a MonoBehaviour
{
    private static T instance;


    // Outside of the class if I want to access instance variable  I can just use the class name that is inheriting from SingletonMonoBehavior Class.Instance
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    // protected = can be accessed in inheriting classes, 
    // we want to be able to overwrite by Inheriting classes, virtual keyword allows method to be overwritten by Inheriting classes
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
