//#define USING_UNITY_FUNCTIONS
#define ALLOW_SINGLETON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TextCorvid
{
    public class TextManager : MonoBehaviour
    {
        #if ALLOW_SINGLETON
        public static TextManager x;
        #endif
        [SerializeField] string s_filePath;
        public Dictionary<string, string> D_allText = new Dictionary<string, string>();
        [SerializeField] public Languages[] A_supportedLanguages;
        [SerializeField] public float f_textSpeed;
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
            s_filePath = Application.dataPath + "/"+ s_filePath;
            MakeThisObjectSingleton();
            //MakeTestJsonData();


            switch (LoadFileExtention())
            {
                case ".csv":
                    D_allText = ReadCSVText();
                    break;
                case ".json":
                    D_allText = ReadJsonText();
                    break;
                case ".xml":
                    D_allText = ReadXMLData();
                    break;
                default:
                    Debug.LogError("Unable to get file extension");
                    break;
            }
        }
        #endif
        private string LoadFileExtention()
        {
            FileStream fs = null;
            if (File.Exists(s_filePath + ".csv"))
                fs = File.Open(s_filePath + ".csv", FileMode.Open);
            else if (File.Exists(s_filePath + ".json"))
                fs = File.Open(s_filePath + ".json", FileMode.Open);
            else if (File.Exists(s_filePath + ".xml"))
                fs = File.Open(s_filePath + ".xml", FileMode.Open);
            else return "Unable to load file extension.";
            string extention = Path.GetExtension(fs.Name);
            fs.Close();
            return extention;
        }
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

        private Dictionary<string, string> ReadCSVText()
        {
            Dictionary<string, string> readText = new Dictionary<string, string>();
            string allTextFromFile = File.ReadAllText(s_filePath + ".csv");
            string[] textRows = allTextFromFile.Split("\n"[0]);
            int rows = textRows.Length;
            for(int i = 0; i < rows; i++)
            {
                string id = textRows[i].Split(',')[0] + textRows[i].Split(',')[1];
                string[] splitText = textRows[i].Split(',');
                string text = "";
                for (int j = 2; j < splitText.Length; j++)
                    text += splitText[j] + (j != splitText.Length -1 ? ',' : '\0');
                readText.Add(id, text);
            }
            return readText;
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

        private Dictionary<string, string> ReadXMLData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CrowXml));
            FileStream fs = new FileStream(s_filePath + ".xml", FileMode.Open);
            CrowXml crow = serializer.Deserialize(fs) as CrowXml; 
            Dictionary<string, string> textData = new Dictionary<string, string>();
            foreach(CrowText text in crow.L_crowText)
            {
                textData.Add(text.ID + text.Country, text.TextToDisplay);
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
    public class CrowTextCollection
    {
        public CrowText[] crowText;
    }
    [System.Serializable, XmlRoot("CrowXml")]
    public class CrowXml
    {
        [XmlArray("CrowTextCollection")]
        [XmlArrayItem("CrowText")]
        public List<CrowText> L_crowText = new List<CrowText>();
    }

    [System.Serializable]
    public struct CrowText
    {
        [XmlElement("ID")]
        public string ID;
        [XmlElement("Country")]
        public string Country;
        [XmlElement("TextToDisplay")]
        public string TextToDisplay;
    }
}