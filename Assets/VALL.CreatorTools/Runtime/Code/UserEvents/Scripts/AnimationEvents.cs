using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour {
    public List<AnimationEvent> animEvents;
    public void TriggerEvent(string val) {
        foreach (var a in animEvents) {
            if (a.key == val)
                a.animationEvent?.Invoke();
        }
    }
    
}

[System.Serializable]
public struct AnimationEvent {
    public string key;
    public UnityEvent animationEvent;
}
