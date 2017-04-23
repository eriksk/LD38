

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    // TODO: ontriggerenter for grabhandles, when they are tedatched
    public RocketBuilder RocketBuilder;

    public GameObject SuccessPrefab;
    public Transform RocketPartsContainer;

    public Material BaseMaterial, ErrorMaterial;
    private HashSet<int> _handled;

    void Start()
    {
        _handled = new HashSet<int>();
    }

    public void CheatTeleportNextExpectedToLandingPad()
    {
        if(!Application.isPlaying) return; // Don't want to screw up in edit mode

        var next = RocketBuilder.NextExpected;
        if(next == null) return;

        var name = next.name;

        var obj = RocketPartsContainer.Children().FirstOrDefault(x => x.name == name);
        if(obj == null) return;
        obj.transform.position = transform.position + Vector3.up * 3f;

    }

    public void OnTriggerEnter(Collider collider)
    {
        Handle(collider.gameObject);
    }

    public void OnTriggerStay(Collider collider)
    {
        Handle(collider.gameObject);
    }

    private void Handle(GameObject gameObject)
    {
        var handle = gameObject.GetComponent<GrabHandle>();
        if(handle == null) return;

        var allHandlesOnObject = handle.Root.GetComponentsInChildren<GrabHandle>();

        if(allHandlesOnObject.Any(x => x.Attached)) return;
        if(_handled.Contains(handle.Root.GetInstanceID())) return;

        if(RocketBuilder.TryAddObject(handle.Root.gameObject, transform.position))
        {
            Debug.Log("Added object to rocket builder: " + handle.Root.name);
            _handled.Add(handle.Root.GetInstanceID());

            Instantiate(SuccessPrefab, transform.position, transform.rotation);

            GetComponent<MeshRenderer>().sharedMaterial = BaseMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().sharedMaterial = ErrorMaterial;
        }
    }
}