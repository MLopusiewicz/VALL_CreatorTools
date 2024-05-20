
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ExhibitionInfo", menuName = "Vall/Exhibition", order = 1)]
public class ExhibitionScriptable : ScriptableObject {

    public string ID;
    public string authorID;
    public Sprite img;

    public string title;
    public string info;

    public string EntryScene;
}
