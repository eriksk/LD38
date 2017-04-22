using System;
using UnityEngine;

public class GrabHandle : MonoBehaviour
{
    public Material InRangeMaterial;
    public Material OutOfRangeMaterial;
    public bool InRange;

    private MeshRenderer _renderer;

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
}