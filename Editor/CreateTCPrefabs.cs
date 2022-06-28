using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTCPrefabs
{
    [MenuItem("GameObject/TextCorvid/Resizable Textbox", false, 4)]
    static void CreateResizableTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/ResizableTextBox.prefab");
    }

    [MenuItem("GameObject/TextCorvid/World Space Textbox", false, 2)]
    static void CreateWSTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/WSCanvasTextBox.prefab");
    }

    [MenuItem("GameObject/TextCorvid/Text Managers", false, 1)]
    static void CreateTextManagers()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TextManager.prefab");
    }
    [MenuItem("GameObject/TextCorvid/Trigger Text Box", false, 3)]
    static void CreateTriggerTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TriggerTextBox.prefab");
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
