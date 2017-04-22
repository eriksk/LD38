
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

    void Start()
    {
        Arm.StateChanged += ((state) => 
        {
            if(state != RobotArmState.Grabbing)
            {
                if(ObjectHandleInRangeOfGrabbing != null)
                {
                    ObjectHandleInRangeOfGrabbing.MarkAsOutOfRange();
                    ObjectHandleInRangeOfGrabbing = null;
                }
            }
        });
    }

	void Update () 
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
	}

    void LateUpdate()
    {
        _opening = false;
    }

    public void Open()
    {
        _opening = true;
    }

    public GrabHandle ObjectHandleInRangeOfGrabbing;
    public void OnTriggerEnter(Collider collider)
    {
        if(ObjectHandleInRangeOfGrabbing != null) return; // Already have something in range

        // Only when reaching
        if(Arm.CurrentState != RobotArmState.Reach) return;

        var handle = collider.gameObject.GetComponent<GrabHandle>();
        if(handle == null) return;

        Debug.Log("Can grab");
        ObjectHandleInRangeOfGrabbing = handle;
        ObjectHandleInRangeOfGrabbing.MarkAsInRange();
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
            Debug.Log("Clearing obj");
            ObjectHandleInRangeOfGrabbing.MarkAsOutOfRange();
            ObjectHandleInRangeOfGrabbing = null;
        }
    }
}
