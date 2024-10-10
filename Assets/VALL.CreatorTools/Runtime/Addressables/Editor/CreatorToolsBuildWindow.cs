using UnityEditor;
using UnityEngine;
using static BundleBuilder;

public class CreatorToolsBuildWindow : EditorWindow {

    public WindowData data;
    bool validationStatus;

    const string ERROR_ICON = "d_console.erroricon";
    const string OK_ICON = "d_FilterSelectedOnly@2x";


    [MenuItem("VALL/Data Builder")]
    public static CreatorToolsBuildWindow GetWindow() {
        return EditorWindow.GetWindow<CreatorToolsBuildWindow>();
    }
    SerializedObject serializedData;



    private void OnGUI() {
        ValidtionGUI();

        PathGUI();
        GUI.enabled = validationStatus;

        if (GUILayout.Button("Build Author")) {
            validationStatus = DataValidator.ValidateAll();
            if (validationStatus)
                BundleBuilder.BuildBundle(Bundle.Author, data.targetPath);
        }
        if (GUILayout.Button("Build Exhibition")) {
            validationStatus = DataValidator.ValidateAll();
            if (validationStatus)
                BundleBuilder.BuildBundle(Bundle.Exhibition, data.targetPath);
        }


    }
    void PathGUI() {

        serializedData = new SerializedObject(data);
        using (new EditorGUILayout.HorizontalScope()) {
            GUILayout.Label("Folder", GUILayout.Width(50));

            EditorGUILayout.PropertyField(serializedData.FindProperty("targetPath"), GUIContent.none);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Project"), GUILayout.Width(32))) {
                var p = EditorUtility.OpenFolderPanel("", serializedData.FindProperty("targetPath").stringValue, "StreamingAssets");
                if (p != null && p != "")
                    serializedData.FindProperty("targetPath").stringValue = p;
            }
        }

        serializedData.ApplyModifiedProperties();
    }
    void ValidtionGUI() {
        using (new UnityEditor.EditorGUILayout.HorizontalScope()) {
            int size = 48;
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            if (validationStatus)
                EditorGUILayout.LabelField(EditorGUIUtility.IconContent(OK_ICON), style, GUILayout.Width(size), GUILayout.Height(size));
            else
                EditorGUILayout.LabelField(EditorGUIUtility.IconContent(ERROR_ICON), style, GUILayout.Width(size), GUILayout.Height(size));

            using (new UnityEditor.EditorGUILayout.VerticalScope()) {

                if (validationStatus)
                    GUILayout.Label("Setup complete");
                else
                    GUILayout.Label("Errors found. Press validate and check console");

                if (GUILayout.Button("Validate")) {
                    validationStatus = DataValidator.ValidateAll();
                }
            }

        }
    }

}


