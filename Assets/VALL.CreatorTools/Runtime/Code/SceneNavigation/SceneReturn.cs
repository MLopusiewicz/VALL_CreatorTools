using UnityEngine;

public class SceneReturn : MonoBehaviour {
    public SceneAssetReference previousScene;
    public static SceneAssetReference PreviousScene;

    private void Awake() {
        PreviousScene = previousScene;
    }
    private void OnDestroy() {
        PreviousScene = null;
    }

}
