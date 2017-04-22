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
        _originalParent = Root.parent;
        Root.parent = transform;
        Rigidbody.isKinematic = true;
    }

    public void Detach()
    {
        Root.parent = _originalParent;
        Rigidbody.isKinematic = false;
    }
}