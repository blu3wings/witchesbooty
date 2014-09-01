using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
    public GameObject startChain;
    public LineRenderer _lineRenderer;

    private HingeJoint2D _hinge;
    private Ray _ray;
    private RaycastHit _hit;

    private RaycastHit2D _2dHit;
    private Ray2D _2dRay;

    private float _defaultDistance;
    private float _distance;

    public GameObject[] chains;

    private GameObject _obj;

    private void Start()
    {
        if(startChain != null)
        {
            _hinge = startChain.GetComponent<HingeJoint2D>();
            _lineRenderer = gameObject.GetComponent<LineRenderer>();

            _lineRenderer.SetPosition(0, gameObject.transform.position);

            _defaultDistance = Vector2.Distance(_hinge.connectedAnchor, gameObject.transform.position) + 0.2f;
        }
    }

    private void Update()
    {       
        _lineRenderer.SetPosition(0, gameObject.transform.position);

        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Down");
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _2dHit = Physics2D.Raycast(_ray.origin,_ray.direction,Mathf.Infinity,1 << LayerMask.NameToLayer("platform"));
            if (_2dHit != null && _2dHit.collider != null)
            {
                if (_obj != null)
                    Destroy(_obj);

                _obj = new GameObject();

                _hinge.enabled = true;
                //foreach (GameObject g in chains)
                //{
                //    g.SetActive(true);
                //}

                _lineRenderer.enabled = true;
                //Vector2 dir = new Vector2(gameObject.transform.position.x,
                //    gameObject.transform.position.y) - _2dHit.point;

                //dir.Normalize();

                //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                //foreach(GameObject g in chains)
                //{
                //    g.transform.position = gameObject.transform.position;
                //    g.transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
                //}

                //Debug.Log(_2dHit.collider.name + " | " + _2dHit.point.x + " | " + _2dHit.point.y);

                //Debug.Log(_2dHit.point.y - _hinge.connectedAnchor.y);

                Vector2 test = Camera.main.transform.InverseTransformPoint(_2dHit.point.x, _2dHit.point.y,0);
                Vector2 test2 = _2dHit.transform.InverseTransformPoint(_2dHit.point);

                _hinge.connectedBody = _2dHit.rigidbody;

                _hinge.connectedAnchor = new Vector2(test2.x, _2dHit.transform.position.y - test.y);

                

                _obj.transform.position = new Vector2(_2dHit.point.x, _2dHit.point.y);
                _obj.transform.parent = _2dHit.collider.transform;
                
                //_lineRenderer.SetPosition(1, new Vector3(_2dHit.point.x, _2dHit.point.y, 0));
                //_lineRenderer.SetPosition(1, _obj.transform.position);
            }
            else
            {
                //foreach (GameObject g in chains)
                //{
                //    g.SetActive(false);
                //}

                _hinge.enabled = false;
                if(_obj != null)
                    _obj.transform.parent = null;

                _lineRenderer.enabled = false;
            }

            

            if(_obj != null)
            {
                Debug.Log(_obj.transform.position);
                _lineRenderer.SetPosition(1, _obj.transform.position);
            }
                
        }

        //_distance = Vector2.Distance(_hinge.connectedAnchor, gameObject.transform.position);
        //if ((_distance - 1f) > _defaultDistance)
        //{
        //    _lineRenderer.enabled = true;
        //}
        //else
        //{
        //    _lineRenderer.enabled = false;
        //}
    }
}
