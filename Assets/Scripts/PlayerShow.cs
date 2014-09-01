using UnityEngine;
using System.Collections;

public class PlayerShow : MonoBehaviour 
{
    public GameObject archi;
    public GameObject sashi;
    public GameObject galiver;

    private bool _isDatafound;

    private void Update()
    {
        if(!_isDatafound)
        {
            if(Data.instance != null)
            {
                archi.SetActive(false);
                sashi.SetActive(false);
                galiver.SetActive(false);

                if(Data.instance._id == 0)
                {
                    archi.SetActive(true);
                }
                else if(Data.instance._id == 1)
                {
                    sashi.SetActive(true);
                }
                else if (Data.instance._id == 2)
                {
                    galiver.SetActive(true);
                }

                _isDatafound = true;

                DestroyImmediate(GameObject.Find("GarbageToClear"));
            }
        }
    }

    public void replay()
    {
        Application.LoadLevel(1);
    }

    public void quit()
    {
        Application.LoadLevel(0);
        Data.instance.delete();
        DestroyImmediate(this);
    }
}
