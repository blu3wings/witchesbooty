using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour 
{
    private static Data _instance;
    public static Data instance
    {
        get{return _instance;}
    }

    public int _id;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void saveData(int id)
    {
        _id = id;
    }

    public void delete()
    {
        DestroyImmediate(this);
    }
}
