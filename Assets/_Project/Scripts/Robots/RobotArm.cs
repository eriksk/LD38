

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotArm : MonoBehaviour
{   
    public Robot Robot;
    public RobotClaws Claws;
    public RobotArmStateTransformRotations States;

    public bool IsTransitioning;
    public RobotArmState CurrentState;

    public float StateTransitionDuration = 1f;
    private RobotArmStateTransformRotation _previousState;
    private RobotArmStateTransformRotation _state;
    private bool _transitioningState = false;
    private float _currentTransition;

    public event Action<RobotArmState, RobotArmState> StateChanged;

    void Start()
    {
        GotoState(RobotArmState.Idle);
    }

    private RobotArmStateTransformRotation GetState(RobotArmState state)
    {
        return States.States.FirstOrDefault(x => x.State == state);
    }

    void Update()
    {
        if(_transitioningState)
        {
            _currentTransition += Time.deltaTime;
            if(_currentTransition >= StateTransitionDuration)
            {
                _currentTransition = StateTransitionDuration;
                _transitioningState = false;
            }
            var progress = _currentTransition / StateTransitionDuration;

            ApplyTransition(_previousState, _state, progress);
        }

        IsTransitioning = _transitioningState;
        CurrentState = _state.State;
    }

    private void ApplyTransition(RobotArmStateTransformRotation previousState, RobotArmStateTransformRotation state, float progress)
    {
        for(var i = 0; i < previousState.Rotations.Count; i++)
        {
            var prev = previousState.Rotations[i].Rotation;
            var target = state.Rotations[i].Rotation;

            var transform = previousState.Rotations[i].Transform;
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(prev), Quaternion.Euler(target), progress);
        }
    }

    public bool GotoState(RobotArmState state)
    {
        if(_state != null && _state.State == state) return false;

        _previousState = _state;
        if(_state == null)
            _previousState = GetState(RobotArmState.Idle);
        else
        {
            // Snapshot current pose
            var s = new RobotArmStateTransformRotation()
            {
                State = _previousState.State,
                Rotations = _previousState.Rotations.Select(x => new TransformRotation()
                {
                    Transform = x.Transform,
                    Rotation = x.Transform.localRotation.eulerAngles
                }).ToList()
            };
            _previousState = s;
        }
        _state = GetState(state);
        _transitioningState = true;
        _currentTransition = 0f;

        if(StateChanged != null)
        {
            StateChanged(_previousState.State, _state.State);
        }
        return true;
    }
}

public enum RobotArmState
{
    Idle,
    Reach,
    Grabbing
}

[Serializable]
public class RobotArmStateTransformRotations
{
    public List<RobotArmStateTransformRotation> States = new List<RobotArmStateTransformRotation>();
}

[Serializable]
public class RobotArmStateTransformRotation
{
    public RobotArmState State;
    public List<TransformRotation> Rotations = new List<TransformRotation>();
}

[Serializable]
public class TransformRotation
{
    public Transform Transform;
    public Vector3 Rotation;
}