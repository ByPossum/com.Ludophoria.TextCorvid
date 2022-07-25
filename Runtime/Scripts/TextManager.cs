using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;
using LudoReaders;


namespace TextCorvid
{
    public class TextManager
    {
        public Languages l_currentLanguage;
        private TextSettings ts_settings = new TextSettings();
        private Dictionary<string, string> D_allText = new Dictionary<string, string>();
        private int i_currentLanguageIndex = 0;
        private Dictionary<string, Text> D_currentTextOnScreen;
        private string s_filePath;
        /// TODO:
        /// - Add "currently in use" text to be updated if language changes
        // Set in Player settings
        public TextManager(TextSettings _newSettings)
        {
            ts_settings = _newSettings;
            l_currentLanguage = ts_settings.A_supportedLanguages[0];
            s_filePath = ts_settings.s_filePath;
            //MakeTestJsonData();

            // Load text from file
            D_allText = LoadFileExtention(true) switch
            {
                ".csv" => ReadCSVText(),
                ".json" => ReadJsonText(),
                ".xml" => ReadXMLData(),
                _ => null
            };
        }

        /// <summary>
        /// Check if any of the supported filetypes are available
        /// </summary>
        /// <returns>The file extension that got loaded</returns>
        private string LoadFileExtention(bool _fromResources)
        {
            FileStream fs = null;
            string _filePath = Application.dataPath + (_fromResources ? "/Resources/" : "") + s_filePath;
            if (File.Exists(_filePath + ".csv"))
                fs = File.Open(_filePath + ".csv", FileMode.Open);
            else if (File.Exists(_filePath + ".json"))
                fs = File.Open(_filePath + ".json", FileMode.Open);
            else if (File.Exists(_filePath + ".xml"))
                fs = File.Open(_filePath + ".xml", FileMode.Open);
            else
            {
                fs?.Close();
                if (_fromResources)
                    return LoadFileExtention(false);
                return "Unable to load file extension.";
            }
            string extention = Path.GetExtension(fs.Name);
            fs.Close();
            return extention;
        }
        public string GetText(string _textID)
        {
            _textID = _textID + l_currentLanguage.ToString();
            _textID = _textID.ToLower();
            Dictionary<string, string>.KeyCollection keys = D_allText.Keys;
            foreach (string key in D_allText.Keys)
                if (key.Contains(_textID))
                    return D_allText[key];
            return "Unable To Get Text";
        }

        private Dictionary<string, string> ReadCSVText()
        {
            Dictionary<string, string> readText = new Dictionary<string, string>();
            TextAsset _ass = GetTextAssetFromFile(".csv");
            string allTextFromFile = _ass.text;
            string[] textRows = allTextFromFile.Split("\n"[0]);
            int rows = textRows.Length;
            for(int i = 0; i < rows; i++)
            {
                string id = textRows[i].Split(',')[0] + textRows[i].Split(',')[1] + textRows[i].Split(',')[2];
                string[] splitText = textRows[i].Split(',');
                string text = "";
                for (int j = 3; j < splitText.Length; j++)
                    text += splitText[j] + (j != splitText.Length -1 ? ',' : '\0');
                readText.Add(id, text);
            }
            return readText;
        }

        private Dictionary<string, string> ReadJsonText()
        {
            TextAsset _ass = GetTextAssetFromFile(".JSON");
            CrowTextCollection ctc = JsonUtility.FromJson<CrowTextCollection>(_ass.text);
            Dictionary<string, string> textData = new Dictionary<string, string>();
            foreach(CrowText crow in ctc.crowText)
            {
                textData.Add(crow.ID + crow.Event + crow.Country, crow.TextToDisplay);
            }
            return textData;
        }

        private Dictionary<string, string> ReadXMLData()
        {
            TextAsset _ass = GetTextAssetFromFile(".xml");
            CrowXml _allText = GenericReaders.ReadXML<CrowXml>(_ass);
            Dictionary<string, string> textData = new Dictionary<string, string>();
            foreach (CrowText _text in _allText.L_crowText)
                textData.Add(_text.ID + _text.Event + _text.Country, _text.TextToDisplay);
            return textData;
        }

        private TextAsset GetTextAssetFromFile(string _ext)
        {
            TextAsset _ass = Resources.Load<TextAsset>(s_filePath);
            if (!_ass)
                _ass = new TextAsset(File.ReadAllText(Application.dataPath + s_filePath + _ext));
            return _ass;
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

        private int UpdateLanguageIter(int _step)
        {
            return (int)Mathf.Repeat(i_currentLanguageIndex+_step, ts_settings.A_supportedLanguages.Length);
        }
        private int UpdateLanguageIter(Languages _lang)
        {
            for (int i = 0; i < ts_settings.A_supportedLanguages.Length; i++)
            {
                if (ts_settings.A_supportedLanguages[i] == _lang)
                    return i;
            }
            Debug.LogError("Attempt to change to unsupported language.");
            return -1;
        }

        /// <summary>
        /// Iterate by one language
        /// </summary>
        public void ChangeLanguage()
        {
            i_currentLanguageIndex = UpdateLanguageIter(1);
            l_currentLanguage = ts_settings.A_supportedLanguages[i_currentLanguageIndex];
        }

        public void ChangeLanguage(Languages _lang)
        {
            i_currentLanguageIndex = UpdateLanguageIter(_lang);
            l_currentLanguage = _lang;
        }

        // Dis gross try not to use this
        public void ChangeLanguage(string _languageToChangeTo)
        {
            // Ew. God- jeez what? Gross
            Languages _nextLang = (Languages)((int?)System.Enum.Parse(typeof(Languages), _languageToChangeTo) ?? 0);
            // Does this work? No idea
            i_currentLanguageIndex = UpdateLanguageIter((int)_nextLang - (int)i_currentLanguageIndex);
            l_currentLanguage = _nextLang;
        }
        public int GetSettings()
        {
            return ts_settings.i_textSpeed;
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
        [XmlElement("Event")]
        public string Event;
        [XmlElement("Country")]
        public string Country;
        [XmlElement("TextToDisplay")]
        public string TextToDisplay;
    }
}