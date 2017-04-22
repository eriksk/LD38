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

    public void Attach(Transform owner)
    {
        // TODO: move so handle is in position of the claw (owner)... dunno how!
        // var clawTargetPosition = owner.InverseTransformPoint(transform.position);
        // Root.position = clawTargetPosition;
        _originalParent = Root.parent;
        Root.parent = owner;
        Rigidbody.isKinematic = true;
        _attached = true;
    }

    public void Detach()
    {
        Root.parent = _originalParent;
        Rigidbody.isKinematic = false;
        _attached = false;
    }
}