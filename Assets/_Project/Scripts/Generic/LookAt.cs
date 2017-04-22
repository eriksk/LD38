using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour 
{
	public Transform Target;
	public float Damping = 0.15f;

	void Update () 
	{
		if(Target == null) return;

		var targetRotation = Quaternion.LookRotation((Target.position - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Damping * Time.deltaTime);
	}
}
