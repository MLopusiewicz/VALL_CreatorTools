using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;
using static BundleBuilder;

public class HelperFunctions {

    public static AuthorScriptable GetAuthor() {
        var authorAssetGUID = AssetDatabase.FindAssets($"t:{nameof(AuthorScriptable)}")[0];
        return AssetDatabase.LoadAssetAtPath<AuthorScriptable>(AssetDatabase.GUIDToAssetPath(authorAssetGUID));
    }

    public static ExhibitionScriptable GetExhibiton() {
        var authorAssetGUID = AssetDatabase.FindAssets($"t:{nameof(ExhibitionScriptable)}")[0];
        return AssetDatabase.LoadAssetAtPath<ExhibitionScriptable>(AssetDatabase.GUIDToAssetPath(authorAssetGUID));
    }
    public static void MoveLocales(LocalizedString localization, Bundle bundle) {
        var collection = LocalizationEditorSettings.GetStringTableCollection(localization.TableReference);
        AddressablesManipulator.MoveToGroup(collection, bundle);
        foreach (var a in collection.Tables) {
            AddressablesManipulator.MoveToGroup(a.asset, bundle, $"Locale-{a.asset.LocaleIdentifier.Code}");
        }
        AddressablesManipulator.MoveToGroup(collection.SharedData, bundle);

    }
}
