using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UserTeleportInteraction : MonoBehaviour {

    public static List<UserTeleportInteraction> Instances { get { if (instances == null) instances = new List<UserTeleportInteraction>(); return instances; } }
    static List<UserTeleportInteraction> instances;
    public bool snapTeleport;
    public bool teleportRotation;
    public bool movingPlatform;

    public Transform TeleportationTarget;

    public UnityEvent OnTeleportingStarted;
    public UnityEvent OnTeleportingEnded;

    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;

    public UnityEvent OnSelected;


    private void Awake() {
        Instances.Add(this);
    }
    private void OnDestroy() {
        Instances.Remove(this);
    }

#if UNITY_EDITOR
    private void OnValidate() {
        gameObject.layer = 6;
    }
#endif


}
