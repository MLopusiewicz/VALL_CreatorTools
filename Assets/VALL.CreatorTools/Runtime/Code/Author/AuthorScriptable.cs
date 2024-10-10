using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new Author", menuName = "VALL/Author", order = 1)]
public class AuthorScriptable : ScriptableObject {

    [SerializeField] string guid;
    public string givenName;
    public Sprite img;
    public Guid id => GetId();

    [FormerlySerializedAs("loc")]
    public LocalizedString info;

    public Guid GetId() {
        if (guid == null || guid == "")
            return Guid.Empty;
        if (Guid.TryParse(guid, out var gg)) { return gg; }

        return Guid.Empty;
    }
}