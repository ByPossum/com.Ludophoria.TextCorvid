using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTCPrefabs
{
    [MenuItem("GameObject/TextCorvid/Resizable Textbox", false, 1)]
    static void CreateRSTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/CanvasResizableTextBox.prefab");
    }

    [MenuItem("GameObject/TextCorvid/UI Text", false, 2)]
    static void CreateUIText()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/UIText.prefab");
    }

    [MenuItem("GameObject/TextCorvid/Text Managers", false, 3)]
    static void CreateTextManagers()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TextManager.prefab");
    }

    static void CreateNewObjectFromString(string filePath)
    {
        GameObject _obj = (GameObject)AssetDatabase.LoadAssetAtPath("Packages/" + filePath, typeof(GameObject));

        if (!_obj)
        {
            _obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/" + filePath, typeof(GameObject));
            if (!_obj)
                Debug.LogError("Object failed to create");
        }
        GameObject go = null;
        if (_obj)
            go = PrefabUtility.InstantiatePrefab(_obj, Selection.activeTransform) as GameObject;
    }
}
