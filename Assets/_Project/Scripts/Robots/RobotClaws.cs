
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RobotClaws : MonoBehaviour 
{
	public Transform LeftClaw;
	public Transform RightClaw;

    [Range(0f, 1f)]
    public float Opened = 0f;
    public float RotationRange = 45f;
    public float OpenCloseSpeed = 0.1f;
    
    private bool _opening;

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
                Opened -= OpenCloseSpeed * Time.deltaTime;     
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
}
