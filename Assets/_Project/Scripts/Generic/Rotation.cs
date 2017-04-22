using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour 
{

	public Vector3 Axis;
	public float Speed = 1f;
	public Space Space = Space.World;

	void Update () 
	{
		transform.Rotate(Axis * Speed * Time.deltaTime, Space);
	}
}
