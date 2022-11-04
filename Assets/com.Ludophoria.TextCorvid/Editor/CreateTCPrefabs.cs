using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTCPrefabs
{
    [MenuItem("GameObject/TextCorvid/Resizable Textbox", false,2)]
    static void CreateResizableTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/ResizableTextBox.prefab");
    }

    [MenuItem("GameObject/TextCorvid/Canvas Textbox", false, 3)]
    static void CreateWSTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/CanvasTextBox.prefab");
    }

    [MenuItem("GameObject/TextCorvid/Text Managers", false, 1)]
    static void CreateTextManagers()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TextManager.prefab");
    }
    [MenuItem("GameObject/TextCorvid/Trigger Text Box", false, 14)]
    static void CreateTriggerTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TriggerTextBox.prefab");
    }
    [MenuItem("GameObject/TextCorvid/Text Box", false, 15)]
    static void CreateTextBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/Text.prefab");
    }
    [MenuItem("GameObject/TextCorvid/Text Sequencer", false, 16)]
    static void CreateSequencer()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/TextSequencer.prefab");
    }
    [MenuItem("GameObject/TextCorvid/Sprite Text Box", false, 17)]
    static void CreateSpriteBox()
    {
        CreateNewObjectFromString("com.Ludophoria.TextCorvid/Runtime/Prefabs/SpriteTextBox.prefab");
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
