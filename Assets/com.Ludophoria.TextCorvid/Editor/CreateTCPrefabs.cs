using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTCPrefabs : MonoBehaviour
{
    [MenuItem("TextCorvid/Resizable Textbox"), MenuItem("GameObject/TextCorvid/Resizable Textbox", false, 69)]
    static void CreateResizableTextBox()
    {
        GameObject _obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/com.Ludophoria.TextCorvid/Runtime/Prefabs/ResizableTextBox.prefab", typeof(GameObject));
        
        if(!_obj)
            Debug.LogError("Object Not Found");
        
        GameObject go = PrefabUtility.InstantiatePrefab(_obj, Selection.activeTransform) as GameObject;
    }
}
