using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour 
{
	public RobotArm Arm;

	public float Acceleration = 1f;
	public float RotationSpeed = 0.1f;

	public float MaxSpeed = 3f;

	public List<ParticleSystem> TrailsParticles;

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
		else if(Arm.CurrentState != RobotArmState.Grabbing)
		{
			Arm.GotoState(RobotArmState.Idle);
		}
	}

	void FixedUpdate()
	{
		var stick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		var moving = false;
		if(Mathf.Abs(stick.y) > 0.01f)
		{
			var moveSpeed = stick.y;
			if(stick.y < 0f)
				moveSpeed *= 0.3f; //  Slower backwards
			_rigidbody.AddForce(transform.forward * moveSpeed * Acceleration / Time.deltaTime);
			moving = true;
		}

		if(Mathf.Abs(stick.x) > 0.0f)
		{	
			var rotation = transform.rotation * Quaternion.Euler(0f, stick.x * RotationSpeed / Time.deltaTime, 0f);
			rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
			_rigidbody.MoveRotation(rotation);
			moving = true;
		}
		else
		{
			// make sure we don't tilt over
			var rotation = transform.rotation;
			rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
			_rigidbody.MoveRotation(rotation);
		}

		ToggleTrailParticles(moving);

		var velocity = _rigidbody.velocity;
		var speed = velocity.magnitude;

		if(speed > MaxSpeed)
		{
			_rigidbody.velocity = velocity.normalized * MaxSpeed;
		}
	}

	private void ToggleTrailParticles(bool on)
	{
		foreach(var ps in TrailsParticles)
		{
			var emission = ps.emission;
			emission.enabled = on;			
		}
	}

}
