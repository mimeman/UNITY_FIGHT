using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomAnimationEvent : UnityEvent<string>
{
}

public class CustomAnimationEvents : MonoBehaviour
{
    public CustomAnimationEvent AnimationEvent = new CustomAnimationEvent();

    public void OnAnimationEvent(string eventName)
    {
        AnimationEvent.Invoke(eventName);
    }
}
