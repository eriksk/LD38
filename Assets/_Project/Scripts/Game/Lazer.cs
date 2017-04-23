using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(LineRenderer))]
public class Lazer : MonoBehaviour
{
    public float OnInterval = 1f;
    public float OffInterval = 1f;
    public float StartDelay = 0f;

    [Header("Collision")]
    public float MaxDistance = 10f;
    public LayerMask LayerMask;

    private bool _on;
    private float _current;
    private BoxCollider _collider;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
        Turn(on: true);
    }

    void Update()
    {
        var endPosition = CastEndPosition();

        _lineRenderer.SetPositions(new Vector3[]
        {
            transform.position,
            endPosition
        });

        var length = Vector3.Distance(transform.position, endPosition);
        _collider.size = new Vector3(1f, 1f, length);
        _collider.center = new Vector3(0.5f, 0.5f, length * 0.5f);

        if(!Application.isPlaying)
        {
            return;
        }

        if(StartDelay > 0f)
        {
            StartDelay -= Time.deltaTime;
            return;
        }

        if(!_on)
        {
            if(_current >= OffInterval)
            {
                Turn(on: true);
            }
        }
        else
        {
            if(_current >= OnInterval)
            {
                Turn(on: false);
            }
        }
        _current += Time.deltaTime;
    }

    private Vector3 CastEndPosition()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, LayerMask))
        {
            return hit.point;
        }

        return transform.position + transform.forward * MaxDistance;
    }

    void Turn(bool on)
    {   
        _on = on;
        _current = 0;
        _collider.enabled = on;
        _lineRenderer.enabled = on;
    }

    public void OnTriggerEnter(Collider collider)
    {
        var robot = collider.gameObject.GetComponent<Robot>();
        if(robot == null) return;

        robot.Kill();
    }
}