using Codice.Utils;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using static BundleBuilder;


public static class BundleBuilder {

    public enum Bundle { Author, Exhibition };

    static AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;


    public static void BuildBundle(Bundle b, string targetPath) {

        string folderName = $"{b.ToString().ToLower()[0]}_{settings.profileSettings.GetValueByName(settings.activeProfileId, "ID")}";
        var buildPath = Application.streamingAssetsPath + $"/{folderName}";
        var movePath = targetPath + $"/{folderName}";

        RemoveOldData(buildPath);
        ClearAllGroupsFromBuild();
        settings.DefaultGroup.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = true;
        AddGroupToBuild(b.ToString());

        settings.activeProfileId = settings.profileSettings.GetProfileId(b.ToString());
        AddressableAssetSettings.BuildPlayerContent();

        MoveData(buildPath, movePath);

    }

    static void AddGroupToBuild(string name) {
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

    static void RemoveOldData(string buildPath) {
        if (Directory.Exists(buildPath)) {
            Directory.Delete(buildPath, true);
        }
        AssetDatabase.Refresh();
    }

    static void MoveData(string buildPath, string movePath) {
        if (Directory.Exists(movePath)) {
            Directory.Delete(movePath, true);
        }

        Directory.Move(buildPath, movePath);

        AssetDatabase.Refresh();
    }


    static void ClearAllGroupsFromBuild() {

        foreach (var a in settings.groups) {
            var z = a.GetSchema<BundledAssetGroupSchema>();
            if (z != null)
                z.IncludeInBuild = false;
        }
    }
}

