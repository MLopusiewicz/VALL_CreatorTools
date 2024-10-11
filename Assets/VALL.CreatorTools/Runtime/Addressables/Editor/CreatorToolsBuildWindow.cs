using System;
using UnityEditor;
using UnityEngine;
using static BundleBuilder;

public class CreatorToolsBuildWindow : EditorWindow {

    public WindowData data;
    bool authorValidationStatus;
    bool exhibitionValidationStatus;

    const string ERROR_ICON = "d_console.erroricon";
    const string OK_ICON = "d_FilterSelectedOnly@2x";

    bool authorToggle, exhbToggle;
    [MenuItem("VALL/Builder")]
    public static CreatorToolsBuildWindow GetWindow() {
        return EditorWindow.GetWindow<CreatorToolsBuildWindow>("VALL Builder");
    }
    SerializedObject serializedData;



    private void OnGUI() {
        GUILayout.Space(10);
        PathGUI();
        GUILayout.Space(10);

        authorToggle = EditorGUILayout.BeginFoldoutHeaderGroup(authorToggle, "Author");

        if (authorToggle) {
            ValidtionGUI(ref authorValidationStatus, DataValidator.ValidateAuthor);
            GUI.enabled = authorValidationStatus;
            if (GUILayout.Button("Build Author")) {
                authorValidationStatus = DataValidator.ValidateAuthor();
                if (authorValidationStatus)
                    BundleBuilder.BuildBundle(Bundle.Author, data.targetPath);
            }
            GUILayout.Space(20);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        GUI.enabled = true;
        exhbToggle = EditorGUILayout.BeginFoldoutHeaderGroup(exhbToggle, "Exhibition");
        if (exhbToggle) {
            EditorGUILayout.LabelField("Exhibition", EditorStyles.boldLabel);
            ValidtionGUI(ref exhibitionValidationStatus, DataValidator.ValidateExhibition);
            GUI.enabled = exhibitionValidationStatus;
            if (GUILayout.Button("Build Exhibition")) {
                exhibitionValidationStatus = DataValidator.ValidateExhibition();
                if (exhibitionValidationStatus)
                    BundleBuilder.BuildBundle(Bundle.Exhibition, data.targetPath);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUI.enabled = true;


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
    void ValidtionGUI(ref bool b, Func<bool> validationFunc) {
        using (new UnityEditor.EditorGUILayout.HorizontalScope()) {
            int size = 48;
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            if (b)
                EditorGUILayout.LabelField(EditorGUIUtility.IconContent(OK_ICON), style, GUILayout.Width(size), GUILayout.Height(size));
            else
                EditorGUILayout.LabelField(EditorGUIUtility.IconContent(ERROR_ICON), style, GUILayout.Width(size), GUILayout.Height(size));

            using (new UnityEditor.EditorGUILayout.VerticalScope()) {

                if (b)
                    GUILayout.Label("Setup complete");
                else
                    GUILayout.Label("Errors found. Press validate and check console");

                if (GUILayout.Button("Validate")) {
                    b = validationFunc();
                }
            }

        }
    }

}


