using System;
using UnityEngine;

public class GrabHandle : MonoBehaviour
{
    public Transform Root;
    public Rigidbody Rigidbody;

    public Material InRangeMaterial;
    public Material OutOfRangeMaterial;
    public bool InRange;

    private MeshRenderer _renderer;

    [NonSerialized]
    private Transform _originalParent;

    private bool _attached;

    public bool Attached
    {
        get { return _attached; }
    }

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        MarkAsOutOfRange();
    }

    public void MarkAsInRange()
    {
        InRange = true;
        _renderer.sharedMaterial = InRangeMaterial;
    }

    public void MarkAsOutOfRange()
    {
        InRange = false;
        _renderer.sharedMaterial = OutOfRangeMaterial;
    }

    public void Attach(Transform transform)
    {
        Debug.Log("Attaching");
        _originalParent = Root.parent;
        Root.parent = transform;
        Rigidbody.isKinematic = true;
        _attached = true;
    }

    public void Detach()
    {
        Debug.Log("Detaching");
        Root.parent = _originalParent;
        Rigidbody.isKinematic = false;
        _attached = false;
    }
}