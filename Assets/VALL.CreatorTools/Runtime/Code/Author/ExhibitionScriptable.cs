
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ExhibitionInfo", menuName = "Vall/Exhibition", order = 1)]
public class ExhibitionScriptable : ScriptableObject {

    public string ID;
    public string authorID;
    public string sceneFileName;
    public Sprite img;

    public LocalizedString title;
    public LocalizedString info;

    public AssetReference EntryScene;
}
