using System;
using UnityEngine;
using UnityEngine.Events;


public class GrabItem : MonoBehaviour {
    public static Action<GrabItem> OnRegistered;
    public enum GrabType { snap, free }
    public GrabType grabType;
    public Mesh outlineMesh;

    public UnityEvent OnGrabbed, OnReleased, OnHoverStarted, OnHoverEnded;

    private void Start() {
        OnRegistered?.Invoke(this);
    }
}
