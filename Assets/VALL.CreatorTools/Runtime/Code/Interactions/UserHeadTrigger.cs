using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UserHeadTrigger : MonoBehaviour {

    public UnityEvent OnHeadEnter;
    public UnityEvent OnHeadLeave;

    private void OnTriggerEnter(Collider other) {
        if (other.transform == Camera.main.transform) {
            Debug.Log("[User] Entered: " + gameObject.name);
            OnHeadEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.transform == Camera.main.transform) {
            Debug.Log("[User] Exited: " + gameObject.name);
            OnHeadLeave?.Invoke();
        }
    }
#if UNITY_EDITOR
    private void OnValidate() {
        gameObject.layer = 7;
    }
#endif
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void FireHeadEnter() {
        OnHeadEnter?.Invoke();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void FireHeadLeave() {
        OnHeadLeave?.Invoke();
    }

}
