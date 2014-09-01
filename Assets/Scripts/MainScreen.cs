using UnityEngine;
using System.Collections;

public class MainScreen : MonoBehaviour 
{
    private Ray _ray;
    private RaycastHit2D _2dHit;

    public GameObject comicPrefab;
    public AudioSource audio;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _2dHit = Physics2D.Raycast(_ray.origin, _ray.direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("platform"));
            if (_2dHit != null && _2dHit.collider != null)
            {
                audio.Play();
                comicPrefab.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}