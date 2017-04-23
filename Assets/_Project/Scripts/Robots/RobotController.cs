using System;
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
	private AudioSource _audioSource;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_robot = GetComponent<Robot>();
		_audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if(Input.GetButton("Jump") || Input.GetMouseButton(0))
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

		ToggleWheelsAudio(moving);
		ToggleTrailParticles(moving);

		var velocity = _rigidbody.velocity;
		var speed = velocity.magnitude;

		if(speed > MaxSpeed)
		{
			_rigidbody.velocity = velocity.normalized * MaxSpeed;
		}
	}

	private bool _wheelAudioOn;
    private void ToggleWheelsAudio(bool moving)
    {
		if(_wheelAudioOn == moving) return;
		_wheelAudioOn = moving;

		if(moving)
		{
			StartCoroutine(QuickFade(_audioSource, 1f));
		}
		else
		{
			StartCoroutine(QuickFade(_audioSource, 0f));
		}
    }

    private IEnumerator QuickFade(AudioSource audioSource, float volume)
    {
		var startVolume = audioSource.volume;

		var duration = 0.5f;
		var current = 0f;

		while(current <= duration)
		{
			current += Time.deltaTime;
			
			var vol = Mathf.Lerp(startVolume, volume, current / duration);
			audioSource.volume = vol;
			yield return new WaitForEndOfFrame();
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
