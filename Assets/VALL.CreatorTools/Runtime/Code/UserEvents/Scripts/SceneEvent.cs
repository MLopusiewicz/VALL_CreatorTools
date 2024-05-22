using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vall.Tools.Events;

[CreateAssetMenu(fileName = "MyEvent", menuName = "VALL/Events/SceneChangeEvent", order = 0)]
public class SceneEvent : UserEventBase {
    public SceneAssetReference scene;
    public override object[] GetParams() {
        return new object[] { scene };
    }

    protected override List<string> GetEvents() {
        return new List<string>() { "LoadScene" };
    }
}
