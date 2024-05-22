using System.IO;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class CreatorToolsBuildWindow : EditorWindow {

    enum Bundle { Author, Exhibition };
    public WindowData data;
    public AddressableAssetGroupTemplate template;
    [MenuItem("VALL/Builder")]
    public static CreatorToolsBuildWindow GetWindow() {
        return EditorWindow.GetWindow<CreatorToolsBuildWindow>();
    }
    SerializedObject serializedData;

    AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;


    private void OnGUI() {
        serializedData = new SerializedObject(data);
        EditorGUILayout.PropertyField(serializedData.FindProperty("targetPath"));
        if (GUILayout.Button("SetupPackage")) {
            GetAuthorGroup();
            GetExhibitionGroup();
        }
        if (GUILayout.Button("Build Author")) {
            BuildBundle(Bundle.Author);
        }
        if (GUILayout.Button("Build Exhibition")) {
            BuildBundle(Bundle.Exhibition);
        }

        if (GUILayout.Button("Setup")) {
            SettingsSetup();
        }

        serializedData.ApplyModifiedProperties();

    }

    private void BuildBundle(Bundle b) {

        string folderName = $"{b.ToString().ToLower()[0]}_{settings.profileSettings.GetValueByName(settings.activeProfileId, "ID")}";
        var buildPath = Application.streamingAssetsPath + $"/{folderName}";
        var movePath = data.targetPath + $"/{folderName}";

        RemoveOldData(buildPath);

        foreach (var a in settings.groups) {
            var z = a.GetSchema<BundledAssetGroupSchema>();
            if (z != null)
                z.IncludeInBuild = false;
        }
        AddGroupToBuildByName(b.ToString());

        settings.activeProfileId = settings.profileSettings.GetProfileId(b.ToString());
        AddressableAssetSettings.BuildPlayerContent();

        //bool success = string.IsNullOrEmpty(result.Error);

        //if (!success) {
        //    Debug.LogError("Addressables build error encountered: " + result.Error);
        //} else
        MoveData(buildPath, movePath);


    }
    void RemoveOldData(string buildPath) {
        if (Directory.Exists(buildPath)) {
            Directory.Delete(buildPath, true);
        }
        AssetDatabase.Refresh();
    }
    void MoveData(string buildPath, string movePath) {
        if (Directory.Exists(movePath)) {
            Directory.Delete(movePath, true);
        }

        Directory.Move(buildPath, movePath);

        AssetDatabase.Refresh();
    }
    AddressableAssetGroup GetAuthorGroup() {
        foreach (var a in settings.groups) {
            if (a.Name == Bundle.Author.ToString()) {
                return a;
            }
        }

        var g = settings.CreateGroup(Bundle.Author.ToString(), false, false, false, template.SchemaObjects);
        string authorAssetGUID = null;
        try {
            authorAssetGUID = AssetDatabase.FindAssets("t:AuthorScriptable")[0];
        } catch {
            Debug.Log("Create the author first");
            return null;
        }
        var entry = settings.CreateOrMoveEntry(authorAssetGUID, g, false, false);
        entry.SetLabel(Bundle.Author.ToString(), true, true, true);
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        return g;
    }

    AddressableAssetGroup GetExhibitionGroup() {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        foreach (var a in settings.groups) {
            if (a.Name == Bundle.Exhibition.ToString()) {
                return a;
            }
        }
        var g = settings.CreateGroup(Bundle.Exhibition.ToString(), false, false, false, template.SchemaObjects);
        string exhbAssetGUID = null;
        try {
            exhbAssetGUID = AssetDatabase.FindAssets($"t:{nameof(ExhibitionScriptable)}")[0];
        } catch {
            Debug.Log("Create the author first");
            return null;
        }
        var entry = settings.CreateOrMoveEntry(exhbAssetGUID, g, false, false);
        entry.SetLabel(Bundle.Exhibition.ToString(), true, true, true);
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        return g;
    }

    void AddGroupToBuildByName(string name) {
        var g = settings.FindGroup(name);
        if (g == null)
            throw new System.Exception($"Group not found: {name}");
        var s = g.GetSchema<BundledAssetGroupSchema>();
        if (s != null) {
            s.IncludeInBuild = true;
        } else {
            Debug.LogError("Schema not found");
        }


    }
    void SettingsSetup() {

        settings.profileSettings.CreateValue("ID", GUID.Generate().ToString());
        settings.BuildRemoteCatalog = true;


        var authorProfileID = settings.profileSettings.AddProfile(Bundle.Author.ToString(), settings.activeProfileId);
        var exhibitionProfileID = settings.profileSettings.AddProfile(Bundle.Exhibition.ToString(), settings.activeProfileId);

        var e = GetExhibiton();


        settings.profileSettings.SetValue(authorProfileID, "Remote.BuildPath", "[UnityEngine.Application.streamingAssetsPath]/a_[ID]");
        settings.profileSettings.SetValue(authorProfileID, "Remote.LoadPath", "{UnityEngine.Application.streamingAssetsPath}/a_[ID]");
        settings.profileSettings.SetValue(authorProfileID, "ID", e.authorID);


        settings.profileSettings.SetValue(exhibitionProfileID, "Remote.BuildPath", "[UnityEngine.Application.streamingAssetsPath]/e_[ID]");
        settings.profileSettings.SetValue(exhibitionProfileID, "Remote.LoadPath", "{UnityEngine.Application.streamingAssetsPath}/e_[ID]");
        settings.profileSettings.SetValue(exhibitionProfileID, "ID", e.ID);

        settings.ShaderBundleNaming = ShaderBundleNaming.Custom;
        settings.ShaderBundleCustomNaming = e.ID;
        settings.MonoScriptBundleNaming = MonoScriptBundleNaming.Disabled;
    }

    ExhibitionScriptable GetExhibiton() {
        var authorAssetGUID = AssetDatabase.FindAssets($"t:{nameof(ExhibitionScriptable)}")[0];
        return AssetDatabase.LoadAssetAtPath<ExhibitionScriptable>(AssetDatabase.GUIDToAssetPath(authorAssetGUID));
    }

}
