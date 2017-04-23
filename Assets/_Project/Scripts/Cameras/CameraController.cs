using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public Transform Target;

	[Header("Follow")]
	public float Damping = 15f;
	public float MovementSpeed = 10f;
	public float Distance = 15f;
	public float Height = 10f;

	[Header("Mouse Orbit")]
	public bool MouseLook = false;
	[Range(0f, 1f)]
	public float MouseSmooth = 0.3f;
	public float MouseSpeed = 50f;
	public float ZoomSpeed = 10f;
	public Vector2 OrbitDistance = new Vector2(0.5f, 40f);
	private float _currentOrbitDistance;

	void Start () 
	{
		_currentOrbitDistance = Mathf.Lerp(OrbitDistance.x, OrbitDistance.y, 0.5f);
	}
	
	void Update () 
	{
		if(Target == null) return;

		if(MouseLook)
		{
			if(Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			UpdateMouseLook();
			return;
		}

		if(Cursor.lockState != CursorLockMode.None)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		transform.rotation = 
			Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(Target.position - transform.position, Vector3.up),
				Damping / Time.deltaTime
			);

		var directionFromTarget = -Target.forward;

		var targetPosition = Target.position + ((directionFromTarget * Distance) + new Vector3(0f, Height, 0f));
		transform.position = Vector3.Lerp(transform.position, targetPosition, MovementSpeed * Time.fixedDeltaTime);
	}

	private float _x, _y;

    private void UpdateMouseLook()
    {
		var mouseDelta = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

		_x += mouseDelta.x * MouseSpeed * Time.fixedDeltaTime;
		_y -= mouseDelta.y * MouseSpeed * Time.fixedDeltaTime;

		_x = Mathf.Clamp(_x, 50f, 80f);

		var direction = Quaternion.Euler(_x, _y, 0f);

		_currentOrbitDistance += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.fixedDeltaTime;
		_currentOrbitDistance = Mathf.Clamp(_currentOrbitDistance, OrbitDistance.x, OrbitDistance.y);

		var finalPosition = Target.position + (direction * Vector3.up * _currentOrbitDistance);
		transform.position = Vector3.Lerp(transform.position, finalPosition, 1f - MouseSmooth); // TODO: lerp

		transform.LookAt(Target, Vector3.up);
    }
}
