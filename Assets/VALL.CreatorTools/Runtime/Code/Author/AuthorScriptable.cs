using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new Author", menuName = "VALL/Author", order = 1)]
public class AuthorScriptable : ScriptableObject {

    [SerializeField] string guid;
    public string givenName;
    public Sprite img;
    public Guid id => guid == null || guid == "" ? Guid.Empty : new Guid(guid);

    [FormerlySerializedAs("loc")]
    public LocalizedString info;
}