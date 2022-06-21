using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTCPrefabs
{
    [MenuItem("GameObject/TextCorvid/Resizable Textbox", false, 1)]
    static void CreateResizableTextBox()
    {
        GameObject _obj = (GameObject)AssetDatabase.LoadAssetAtPath("Packages/Text Corvid/Runtime/Prefabs/ResizableTextBox.prefab", typeof(GameObject));

        if(!_obj)
            _obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/com.Ludophoria.TextCorvid/Runtime/Prefabs/ResizableTextBox.prefab", typeof(GameObject));
        else
            Debug.LogError("ResizableTextBox prefab Not Found");

        GameObject go = PrefabUtility.InstantiatePrefab(_obj, Selection.activeTransform) as GameObject;
    }
}
