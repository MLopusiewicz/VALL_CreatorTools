using NUnit.Framework;
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using static UnityEngine.EventSystems.EventTrigger;

public class CreatorToolsBuildWindow : EditorWindow {

    enum Bundle { Author, Exhibition };
    public WindowData data;
    public AddressableAssetGroupTemplate template;
    [MenuItem("VALL/Data Builder")]
    public static CreatorToolsBuildWindow GetWindow() {
        return EditorWindow.GetWindow<CreatorToolsBuildWindow>();
    }
    SerializedObject serializedData;

    AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;


    private void OnGUI() {
        serializedData = new SerializedObject(data);
        EditorGUILayout.PropertyField(serializedData.FindProperty("targetPath"));
        if (GUILayout.Button("SetupPackage")) {
            GetExhibitionGroup();
            GetAuthorGroup();
            var def = settings.DefaultGroup;
            ApplyPaths(def);
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


        if (GUILayout.Button("MoveLocalization")) {
            MoveLocales();
        }
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
        settings.DefaultGroup.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = true;
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
                ApplyPaths(a);
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

        ApplyPaths(g);
        return g;
    }

    AddressableAssetGroup GetExhibitionGroup() {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        foreach (var a in settings.groups) {
            if (a.Name == Bundle.Exhibition.ToString()) {
                ApplyPaths(a);
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
        ApplyPaths(g);
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


    void MoveLocales() {
        var exhb = GetExhibiton();
        MoveLocales(exhb.info, Bundle.Exhibition);

        var author = GetAuthor();
        MoveLocales(author.info, Bundle.Author);
    }

    private AuthorScriptable GetAuthor() {
        var authorAssetGUID = AssetDatabase.FindAssets($"t:{nameof(AuthorScriptable)}")[0];
        return AssetDatabase.LoadAssetAtPath<AuthorScriptable>(AssetDatabase.GUIDToAssetPath(authorAssetGUID));

    }

    ExhibitionScriptable GetExhibiton() {
        var authorAssetGUID = AssetDatabase.FindAssets($"t:{nameof(ExhibitionScriptable)}")[0];
        return AssetDatabase.LoadAssetAtPath<ExhibitionScriptable>(AssetDatabase.GUIDToAssetPath(authorAssetGUID));
    }


    void MoveLocales(LocalizedString localization, Bundle bundle) {
        var collection = LocalizationEditorSettings.GetStringTableCollection(localization.TableReference);
        MoveToGroup(collection, bundle);
        foreach (var a in collection.Tables) {
            MoveToGroup(a.asset, bundle, $"Locale-{a.asset.LocaleIdentifier.Code}");
        }
        MoveToGroup(collection.SharedData, bundle);

    }

    void MoveToGroup(Object o, Bundle b, params string[] labels) {

        var g = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
        AddressableAssetEntry entry;
        if (b == Bundle.Author)
            entry = settings.CreateOrMoveEntry(g, GetAuthorGroup());
        else
            entry = settings.CreateOrMoveEntry(g, GetExhibitionGroup());

        foreach (var a in labels)
            entry.SetLabel(a, true, true);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

    }


    void ApplyPaths(AddressableAssetGroup g) {
        var schema = g.GetSchema<BundledAssetGroupSchema>();
        schema.BuildPath.SetVariableByName(settings, "Remote.BuildPath");
        schema.LoadPath.SetVariableByName(settings, "Remote.LoadPath");
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupSchemaModified, schema, true);

        settings.RemoteCatalogBuildPath.SetVariableByName(settings, "Remote.BuildPath");
        settings.RemoteCatalogLoadPath.SetVariableByName(settings, "Remote.LoadPath");
    }

}
