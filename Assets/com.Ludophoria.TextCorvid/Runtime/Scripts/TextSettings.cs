using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    [CreateAssetMenu(fileName ="TextSettings", menuName ="TextCorvid/New Text Settings", order = 69)]
    public class TextSettings : ScriptableObject
    {
        public string s_filePath;
        public string s_folderPaths;
        public Languages[] A_supportedLanguages;
        public int i_textSpeed;
        public bool b_autoDetectFile;
        public float f_resizingTolerance;
    }
}
