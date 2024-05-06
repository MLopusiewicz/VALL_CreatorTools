using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialPropertySetter : MonoBehaviour {
    public string propertyName;
    Renderer r;
    private void Awake() {
        r = GetComponent<Renderer>();
    }

    public void SetPropertyValue(float f) {
        r.sharedMaterial.SetFloat(propertyName, f);
    }
}
