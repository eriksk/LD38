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

    public bool TryAddObject(GameObject gameObject)
    {
        if(_current == ExpectedOrderOfObjects.Count) return false; // Already added all!

        if(ExpectedOrderOfObjects[_current].name == gameObject.name)
        {
            StartCoroutine(MoveObjectIntoPlace(gameObject));
            _current++;
            return true;
        }

        return false;
    }

    private IEnumerator MoveObjectIntoPlace(GameObject gameObject)
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
            

        var duration = 5f;
        var current = 0f;

        var startPosition = gameObject.transform.position;
        var startRotation = gameObject.transform.rotation;

        while(current <= duration)
        {
            yield return new WaitForEndOfFrame();
            var progress = current / duration;

            gameObject.transform.position = Vector3.Lerp(startPosition, target.transform.position, progress);
            gameObject.transform.rotation = Quaternion.Slerp(startRotation, target.transform.rotation, progress);

            current += Time.deltaTime;
        }
        Debug.Log(gameObject.name + " moved into place!");
    }
}