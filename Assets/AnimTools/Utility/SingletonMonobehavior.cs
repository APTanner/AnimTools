using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;
    private static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<T>();
                if (m_instance == null) 
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    m_instance = singleton.AddComponent<T>();
                }
            }
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
