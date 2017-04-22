using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public Transform Target;
	public float Damping = 15f;
	public float MovementSpeed = 10f;

	public float Distance = 15f;
	public float Height = 10f;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(Target == null) return;

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
}
