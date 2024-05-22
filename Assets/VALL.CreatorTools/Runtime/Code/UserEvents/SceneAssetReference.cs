using UnityEngine.AddressableAssets;

[System.Serializable]
public class SceneAssetReference : AssetReference {

    public SceneAssetReference(string guid) : base(guid) { }

    public override bool ValidateAsset(string path) {

        return path.EndsWith(".unity");
    }
}
