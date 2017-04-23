
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RobotClaws : MonoBehaviour 
{
    public RobotArm Arm;
	public Transform LeftClaw;
	public Transform RightClaw;

    [Range(0f, 1f)]
    public float Opened = 0f;
    public float RotationRange = 45f;
    public float OpenCloseSpeed = 0.1f;
    
    private bool _opening;
    private float _cooldown;

    void Start()
    {
        Arm.StateChanged += ((oldState, state) => 
        {
            if(oldState == RobotArmState.Grabbing)
            {
                ReleaseConnectedObject();
                _cooldown = 1f; // Can't grab anything else atm
            }

            if(state == RobotArmState.Idle && ObjectHandleInRangeOfGrabbing != null)
            {
                // Grab it!
                Arm.GotoState(RobotArmState.Grabbing);
                GrabConnectedObject();
            }
            else if(state != RobotArmState.Grabbing && state != RobotArmState.Reach)
            {
                DetachInRangeHandle();
            }
        });
    }

    void Update () 
	{
        _cooldown -= Time.deltaTime;
	}

    void LateUpdate()
    {
        if(LeftClaw == null || RightClaw == null) return;

        if(Application.isPlaying)
        {
            if(_opening)
            {
                Opened += OpenCloseSpeed * Time.deltaTime;
                Opened = Mathf.Clamp01(Opened);
            }
            else
            {
                // Closing is faster
                Opened -= OpenCloseSpeed * 2f * Time.deltaTime;     
                Opened = Mathf.Clamp01(Opened);       
            }
        }

        LeftClaw.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, -RotationRange, Opened), 0f);
        RightClaw.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, RotationRange, Opened), 180f);
        _opening = false;
    }

    public void Open()
    {
        _opening = true;
    }

    [NonSerialized]
    public GrabHandle ObjectHandleInRangeOfGrabbing;

    private void DetachInRangeHandle()
    {
        if(ObjectHandleInRangeOfGrabbing != null)
        {
            ObjectHandleInRangeOfGrabbing.MarkAsOutOfRange();
        }
        ObjectHandleInRangeOfGrabbing = null;
    }

    private void ConnectHandleInRange(GrabHandle handle)
    {
        // Detach previous if existing
        DetachInRangeHandle();
        ObjectHandleInRangeOfGrabbing = handle;
        ObjectHandleInRangeOfGrabbing.MarkAsInRange();
    }

    private void GrabConnectedObject()
    {
        ObjectHandleInRangeOfGrabbing.Attach(transform);
    }

    private void ReleaseConnectedObject()
    {
        var handles = transform.GetComponentsInChildren<GrabHandle>();
        foreach(var handle in handles)
        {
            handle.Detach();
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(_cooldown > 0f) return;
        
        // Only when reaching
        if(Arm.CurrentState != RobotArmState.Reach) 
        {
            // If no longer reaching, detach.
            DetachInRangeHandle();
            return;
        }

        if(ObjectHandleInRangeOfGrabbing != null) return;

        var handle = collider.gameObject.GetComponent<GrabHandle>();
        if(handle == null) return;

        ConnectHandleInRange(handle);
    }

    public void OnTriggerStay(Collider collider)
    {
        OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        var handle = collider.gameObject.GetComponent<GrabHandle>();
        if(handle == null) return;
        if(handle == ObjectHandleInRangeOfGrabbing)
        {
            DetachInRangeHandle();
        }
    }
}
