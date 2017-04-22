using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketBuilder : MonoBehaviour
{
    public List<Transform> ExpectedOrderOfObjects;
    public Transform HiddenRocketModel;

    private int _current = 0;

    public bool TryAddObject(GameObject gameObject, Vector3 dropOffPoint)
    {
        if(_current == ExpectedOrderOfObjects.Count) return false; // Already added all!

        if(ExpectedOrderOfObjects[_current].name == gameObject.name)
        {
            StartCoroutine(MoveObjectIntoPlace(gameObject, dropOffPoint));
            _current++;
            return true;
        }

        return false;
    }

    private IEnumerator MoveObjectIntoPlace(GameObject gameObject, Vector3 dropOffPoint)
    {
        var target = HiddenRocketModel
            .Children()
            .FirstOrDefault(x => x.gameObject.name == gameObject.name);

        if(target == null)
            Debug.LogError("Object '" + gameObject.name + "' was not found in hidden rocket model");
        
        Destroy(gameObject.GetComponent<Rigidbody>());
        foreach(var collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        var handles = gameObject.transform
            .Children()
            .Select(x => x.gameObject.GetComponent<GrabHandle>())
            .ToArray();
        
        for(var i = 0; i < handles.Length; i++)
            Destroy(handles[i].gameObject);


        var it = MoveObject(
            gameObject.transform,
            gameObject.transform.position, 
            gameObject.transform.rotation, 
            dropOffPoint + Vector3.up * 14f, 
            Quaternion.identity, 
            1.5f);

        while(it.MoveNext())
            yield return it.Current;

        it = MoveObject(
            gameObject.transform,
            gameObject.transform.position, 
            gameObject.transform.rotation, 
            target.transform.position + Vector3.up * 36f, 
            target.transform.rotation, 
            2f);

        while(it.MoveNext())
            yield return it.Current;

        it = MoveObject(
            gameObject.transform,
            gameObject.transform.position, 
            gameObject.transform.rotation, 
            target.transform.position, 
            target.transform.rotation, 
            3f);

        while(it.MoveNext())
            yield return it.Current;

        Debug.Log(gameObject.name + " moved into place!");
    }

    private IEnumerator MoveObject(Transform obj, Vector3 fromPosition, Quaternion fromRotation, Vector3 toPosition, Quaternion toRotation, float duration)
    {
        var current = 0f;

        while(current <= duration)
        {
            yield return new WaitForEndOfFrame();
            var progress = current / duration;

            obj.position = Vector3.Lerp(fromPosition, toPosition, Mathf.SmoothStep(0f, 1f, progress));
            obj.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, progress);

            current += Time.deltaTime;
        }
    }
}