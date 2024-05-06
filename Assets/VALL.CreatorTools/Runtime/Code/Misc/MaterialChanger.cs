using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChanger : MonoBehaviour {
    public Material From, To;
    void Start() {
        if (From == null || To == null)
            return;
        foreach (var a in GetComponentsInChildren<MeshRenderer>()) {
            if (a.sharedMaterials.Length == 1)
                for (int i = 0; i < a.sharedMaterials.Length; i++) {
                    if (a.sharedMaterials[i] == From) {
                        var z = a.sharedMaterials;
                        z[i] = To;
                        a.sharedMaterials = z;
                        break;
                    }
                }
        }
    }


}
