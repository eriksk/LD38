using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PanelButtonWeight : MonoBehaviour
{
    public Material Invalid, Success;
    public string ExpectedObjectName;
    public Transform Door;

    private bool _granted;

    void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial = Invalid;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(_granted) return;

        var handle = collider.gameObject.GetComponent<GrabHandle>();
        if(handle == null) return;

        if(handle.Root.gameObject.name != ExpectedObjectName) return;
        var allHandles = handle.Root.GetComponentsInChildren<GrabHandle>();
        if(allHandles.Any(x => x.Attached)) return;

        _granted = true;
        GetComponent<MeshRenderer>().sharedMaterial = Success;
        StartCoroutine(OpenDoor());
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    private IEnumerator OpenDoor()
    {
        var current = 0f;
        var duration = 3f;
        // -10.4
        
        var start = Door.position;
        var target = new Vector3(start.x, start.y - 20, start.z);

        while(current <= duration)
        {
            current += Time.deltaTime;

            var progress = current / duration;

            Door.transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, progress));

            yield return new WaitForEndOfFrame();
        }
    }
}