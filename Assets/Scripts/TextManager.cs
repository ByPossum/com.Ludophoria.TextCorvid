//#define USING_UNITY_FUNCTIONS
#define ALLOW_SINGLETON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace TextCrow
{
    public class TextManager : MonoBehaviour
    {
        #if ALLOW_SINGLETON
        public static TextManager x;
        #endif
        [SerializeField] string s_filePath;
        public Dictionary<string, string> D_allText = new Dictionary<string, string>();
        [SerializeField] public Languages[] A_supportedLanguages;
        public Languages l_currentLanguage;

        // Uncomment the Define if you wish to do things in start as opposed to init
        #if USING_UNITY_FUNCTIONS
        // Start is called before the first frame update
        void Start()
        {
            MakeThisObjectSingleton();
        }
        #else
        public void Init()
        {
            s_filePath = Application.dataPath + "/" + s_filePath;
            MakeThisObjectSingleton();
            MakeTestJsonData();
            D_allText = ReadJsonText();
        }
        #endif

        public string GetText(string _textID)
        {
            _textID = _textID + l_currentLanguage.ToString();
            Dictionary<string, string>.KeyCollection keys = D_allText.Keys;
            foreach (string key in D_allText.Keys)
                if (key.Contains(_textID))
                    return D_allText[key];
            return "Unable To Get Text";
        }

        /// <summary>
        /// If singleton use is allowed, make this object a singleton
        /// </summary>
        private void MakeThisObjectSingleton()
        {
            #if ALLOW_SINGLETON
            if (x != null)
            {
                Destroy(this);
            }
            else
                x = this;
            DontDestroyOnLoad(this);
            #endif
        }

        private Dictionary<string, string> ReadJsonText()
        {
            CrowTextCollection ctc = JsonUtility.FromJson<CrowTextCollection>(File.ReadAllText(s_filePath + ".JSON"));
            Dictionary<string, string> textData = new Dictionary<string, string>();
            foreach(CrowText crow in ctc.crowText)
            {
                textData.Add(crow.ID + crow.Country, crow.TextToDisplay);
            }
            return textData;
        }

        public void MakeTestJsonData()
        {
            CrowText[] ct = new CrowText[4];
            ct[0] = new CrowText { ID = "tut01", Country = "isl", TextToDisplay = "Hæ ég er Text Corvid." };
            ct[1] = new CrowText { ID = "tut01", Country = "eng", TextToDisplay = "Hi, I'm Text Corvid." };
            ct[2] = new CrowText { ID = "tut02", Country = "isl", TextToDisplay = "Mér likar tölva leiknir." };
            ct[3] = new CrowText { ID = "tut02", Country = "eng", TextToDisplay = "I like video games." };
            CrowTextCollection ctc = new CrowTextCollection();
            ctc.crowText = ct;
            string jsonData = JsonUtility.ToJson(ctc);
            if (!File.Exists(s_filePath + ".JSON"))
            {                    
                FileStream sr = File.Create(s_filePath + ".JSON");
                sr.Close();
            }
            File.WriteAllText(s_filePath + ".JSON", jsonData);
        }
    }

    [System.Serializable]
    public struct CrowTextCollection
    {
        public CrowText[] crowText;
    }

    [System.Serializable]
    public struct CrowText
    {
        public string ID;
        public string Country;
        public string TextToDisplay;
    }
}