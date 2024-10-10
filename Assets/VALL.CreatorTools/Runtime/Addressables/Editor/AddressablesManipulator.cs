using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using static BundleBuilder;

public class AddressablesManipulator {

    static AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;

    public static void CreateProfiles() {

        settings.profileSettings.CreateValue("ID", GUID.Generate().ToString());
        settings.BuildRemoteCatalog = true;


        var authorProfileID = settings.profileSettings.AddProfile(Bundle.Author.ToString(), settings.activeProfileId);
        var exhibitionProfileID = settings.profileSettings.AddProfile(Bundle.Exhibition.ToString(), settings.activeProfileId);

        var e = HelperFunctions.GetExhibiton();


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

    static void ApplyPaths(AddressableAssetGroup g) {
        var schema = g.GetSchema<BundledAssetGroupSchema>();

        schema.BuildPath.SetVariableByName(settings, "Remote.BuildPath");
        schema.LoadPath.SetVariableByName(settings, "Remote.LoadPath");

        settings.RemoteCatalogBuildPath.SetVariableByName(settings, "Remote.BuildPath");
        settings.RemoteCatalogLoadPath.SetVariableByName(settings, "Remote.LoadPath");

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupSchemaModified, schema, true);
    }

    public static AddressableAssetGroup GetExhibitionGroup() {
        foreach (var a in settings.groups) {
            if (a.Name == Bundle.Exhibition.ToString()) {
                ApplyPaths(a);
                return a;
            }
        }
        var g = settings.CreateGroup(Bundle.Exhibition.ToString(), false, false, false, new System.Collections.Generic.List<AddressableAssetGroupSchema>());
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


    public static AddressableAssetGroup GetGroup(Bundle bundle) {
        foreach (var a in settings.groups) {
            if (a.Name == bundle.ToString()) {
                ApplyPaths(a);
                return a;
            }
        }
        return null;
    }

    public static AddressableAssetGroup CreateGroup(Bundle bundle) {
        var schemas = new List<AddressableAssetGroupSchema>() { new BundledAssetGroupSchema(), new ContentUpdateGroupSchema() };

        var g = settings.CreateGroup(bundle.ToString(), false, false, false, schemas);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, g, true);

        ApplyPaths(g);
        return g;
    }



    public static void MoveToGroup(Object o, Bundle b, params string[] labels) {

        var g = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
        AddressableAssetEntry entry;
        entry = settings.CreateOrMoveEntry(g, GetGroup(b));

        if (labels != null)
            foreach (var a in labels)
                entry.SetLabel(a, true, true);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

    }


}