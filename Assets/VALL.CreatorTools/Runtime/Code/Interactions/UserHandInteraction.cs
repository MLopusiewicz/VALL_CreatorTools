using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UserHandInteraction : MonoBehaviour {
	public UnityEvent OnPress, OnEnter, OnExit;
	public Action<UserHandInteraction> OnTriggerDestroy;
	Collider c;

#if UNITY_EDITOR
	private void OnValidate() {
		this.gameObject.layer = 8;
		GetComponent<Collider>().isTrigger = true;
	}
#endif
	private void OnDestroy() {
		OnTriggerDestroy?.Invoke(this);
	}
}
