using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.CullingGroup;

[RequireComponent(typeof(AudioSource))]
public class AudioEvents : MonoBehaviour {
    public UnityEvent OnAudioStart, OnAudioStop;
    AudioSource source;
    bool lastState;
    private void Awake() {
        source = GetComponent<AudioSource>();
    }
    private void Update() {
        if (lastState != source.isPlaying) {
            StateChanged(source.isPlaying);
            lastState = source.isPlaying;
        }
    }
    void StateChanged(bool newState) {
        if (newState) {
            Debug.Log($"[AudioEvents] Started {source.clip} on {gameObject.name}");
            OnAudioStart?.Invoke();
        } else {
            Debug.Log($"[AudioEvents] Finished {source.clip} on {gameObject.name}");
            OnAudioStop?.Invoke();
        }
    }
}
