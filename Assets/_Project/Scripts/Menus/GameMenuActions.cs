

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMenuActions : MonoBehaviour
{
    public List<KeyAction> Actions;

    void Update()
    {
        foreach(var ac in Actions)
        {
            if(Input.GetButtonDown(ac.InputButton))
            {
                ac.Event.Invoke();
            }
        }
    }
}

[Serializable]
public class KeyAction
{
    public string InputButton;
    public UnityEvent Event;
}