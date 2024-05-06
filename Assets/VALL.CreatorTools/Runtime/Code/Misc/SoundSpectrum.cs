using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundSpectrum : MonoBehaviour {
    AudioSource audioSource;
    public List<Freq> frequencies;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    void Update() {
        float[] spectrum = new float[64];

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        foreach (var a in frequencies) {
            a.Set(spectrum[a.spectrum]);
        }
    }
}

[Serializable]
public class Freq {
    [Range(0, 64)]
    public int spectrum;
    public UnityEvent<float> OnValueChanged;
    float lastVal;
    public float lerp;
    public float scale;
    public void Set(float val) {
        if (lastVal < val)
            lastVal = val;
        else
            lastVal = Mathf.Lerp(lastVal, val, lerp) * scale;
        OnValueChanged?.Invoke(lastVal);
    }
}
