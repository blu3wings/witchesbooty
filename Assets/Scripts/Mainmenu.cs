using UnityEngine;
using System.Collections;

public class Mainmenu : MonoBehaviour 
{
    private Ray _ray;
    private RaycastHit2D _2dHit;

    public Sprite[] comicPages;
    public AudioSource audio;

    private int _count = 0;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _2dHit = Physics2D.Raycast(_ray.origin,_ray.direction,Mathf.Infinity,1 << LayerMask.NameToLayer("platform"));
            if (_2dHit != null && _2dHit.collider != null)
            {
                _2dHit.collider.GetComponent<SpriteRenderer>().sprite = comicPages[_count];
                _count++;

                audio.Play();

                if(_count == comicPages.Length)
                {
                    Application.LoadLevel(1);
                }
            }
        }
    }
}
