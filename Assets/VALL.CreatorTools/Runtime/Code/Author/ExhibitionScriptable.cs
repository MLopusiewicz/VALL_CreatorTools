
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ExhibitionInfo", menuName = "VALL/Exhibition", order = 1)]
public class ExhibitionScriptable : ScriptableObject {

    public string ID;
    public string authorID;
    public Sprite img;

    public LocalizedString title;
    public LocalizedString info;

    public SceneAssetReference EntryScene;
}
