using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DataValidator {
    static List<Func<bool>> ValidationChain = new() {
        ValidateAuthorFile,
        ValidateExhibitionFile,
        ValidateAddressableSetup,
        ValidateAuthorGroup,
        ValidateExhibitionGroup
    };
    static List<Func<bool>> AuthorValidationChain = new() {
        ValidateAuthorFile,
        ValidateAddressableSetup,
        ValidateAuthorGroup
    };
    static List<Func<bool>> ExhibitionValidationChaind = new() {
        ValidateExhibitionFile,
        ValidateAddressableSetup,
        ValidateExhibitionGroup
    };

    public static bool ValidateAll() {

        return ValidateChain(ValidationChain);
    }

    public static bool ValidateAuthor() {
        return ValidateChain(AuthorValidationChain);
    }
    public static bool ValidateExhibition() {
        return ValidateChain(ExhibitionValidationChaind);
    }

    static bool ValidateChain(List<Func<bool>> chain) {
        foreach (var a in chain) {
            try {
                var result = a?.Invoke();
                if (result == false)
                    return false;
            } catch (Exception e) {
                Debug.LogError($"Error encountered when validating: {e.Message}");
                return false;
            }

        }
        return true;

    }

    public static bool ValidateAuthorFile() {
        var g = AssetDatabase.FindAssets($"t:{nameof(AuthorScriptable)}");
        if (g.Length == 0) {
            Debug.LogError("[VALL] No author found");
            return false;
        }
        if (g.Length > 1) {
            Debug.LogError("[VALL] More than 1 author found");
            return false;
        }
        var obj = AssetDatabase.LoadAssetAtPath<AuthorScriptable>(AssetDatabase.GUIDToAssetPath(g[0]));

        if (obj.id == Guid.Empty) {
            Debug.LogError("[VALL] Author's guid incorrect or empty");
            return false;
        }
        if (obj.givenName == string.Empty) {
            Debug.LogError("[VALL] Author's name cannot be empty");
            return false;
        }

        if (obj.img == null) {
            Debug.LogError("[VALL] Author's image cannot be empty");
            return false;
        }

        if (obj.info.IsEmpty) {
            Debug.LogError("[VALL] Author's info cannot be empty");
            return false;
        }
        return true;
    }

    public static bool ValidateExhibitionFile() {
        var g = AssetDatabase.FindAssets($"t:{nameof(ExhibitionScriptable)}");

        if (g.Length == 0) {
            Debug.LogError("[VALL] No exhibition found");
            return false;
        }

        if (g.Length > 1) {
            Debug.LogError("[VALL] More than 1 exhibition found");
            return false;
        }

        var obj = AssetDatabase.LoadAssetAtPath<ExhibitionScriptable>(AssetDatabase.GUIDToAssetPath(g[0]));

        if (obj.ID == null || obj.ID == "") {
            Debug.LogError("[VALL] Exhibition's ID cannot be empty");
            return false;
        }

        if (obj.authorID == null || obj.authorID == "") {
            Debug.LogError("[VALL] Exhibition's author cannot be empty");
            return false;
        }

        if (obj.img == null) {
            Debug.LogError("[VALL] Exhibition's <b>image</b> cannot be empty");
            return false;
        }

        if (obj.title.IsEmpty) {
            Debug.LogError("[VALL] Exhibition's title cannot be empty");
            return false;
        }

        if (obj.info.IsEmpty) {
            Debug.LogError("[VALL] Exhibition's info cannot be empty");
            return false;
        }

        if (obj.EntryScene == null) {
            Debug.LogError("[VALL] Exhibition's entry scene cannot be empty");
            return false;
        }

        return true;
    }

    public static bool ValidateAuthorGroup() {
        if (AddressablesManipulator.GetGroup(BundleBuilder.Bundle.Author) == null)
            AddressablesManipulator.CreateGroup(BundleBuilder.Bundle.Author);

        var author = HelperFunctions.GetAuthor();
        AddressablesManipulator.MoveToGroup(author, BundleBuilder.Bundle.Author, BundleBuilder.Bundle.Author.ToString());

        HelperFunctions.MoveLocales(author.info, BundleBuilder.Bundle.Author);
        return true;
    }
    public static bool ValidateExhibitionGroup() {
        if (AddressablesManipulator.GetGroup(BundleBuilder.Bundle.Exhibition) == null)
            AddressablesManipulator.CreateGroup(BundleBuilder.Bundle.Exhibition);

        var exhb = HelperFunctions.GetExhibiton();
        AddressablesManipulator.MoveToGroup(exhb, BundleBuilder.Bundle.Exhibition, BundleBuilder.Bundle.Exhibition.ToString());
        AddressablesManipulator.MoveToGroup(AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(exhb.EntryScene.AssetGUID)), BundleBuilder.Bundle.Exhibition, BundleBuilder.Bundle.Exhibition.ToString());
        HelperFunctions.MoveLocales(exhb.title, BundleBuilder.Bundle.Exhibition);
        HelperFunctions.MoveLocales(exhb.info, BundleBuilder.Bundle.Exhibition);

        return true;
    }

    private static bool ValidateAddressableSetup() {
        AddressablesManipulator.CreateProfiles();
        AddressablesManipulator.SetDefaultGroupRemote();
        return true;
    }

}
