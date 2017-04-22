using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour 
{
	public RobotArm Arm;

	public float Acceleration = 1f;
	public float RotationSpeed = 0.1f;

	public float MaxSpeed = 3f;

	private Rigidbody _rigidbody;
	private Robot _robot;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_robot = GetComponent<Robot>();
	}

	void Update()
	{
		if(Input.GetButton("Jump"))
		{
			_robot.Claws.Open();
			Arm.GotoState(RobotArmState.Reach);
		}
		else
		{
			Arm.GotoState(RobotArmState.Idle);
		}
	}

	void FixedUpdate()
	{
		var stick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		// if(Arm.CurrentState != RobotArmState.Reach) // Freeze when grabbing
		{
			if(Mathf.Abs(stick.y) > 0.01f)
			{
				var moveSpeed = stick.y;
				if(stick.y < 0f)
					moveSpeed *= 0.3f; //  Slower backwards
				_rigidbody.AddForce(transform.forward * moveSpeed * Acceleration / Time.deltaTime);
			}

			if(Mathf.Abs(stick.x) > 0.0f)
			{	
				var rotation = transform.rotation * Quaternion.Euler(0f, stick.x * RotationSpeed / Time.deltaTime, 0f);
				_rigidbody.MoveRotation(rotation);
			}
		}

		var velocity = _rigidbody.velocity;
		var speed = velocity.magnitude;

		if(speed > MaxSpeed)
		{
			_rigidbody.velocity = velocity.normalized * MaxSpeed;
		}
	}

}
