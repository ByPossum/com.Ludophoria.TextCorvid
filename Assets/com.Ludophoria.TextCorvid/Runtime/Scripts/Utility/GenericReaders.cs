using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace LudoReaders
{
    public class GenericReaders : MonoBehaviour
    {
        public static T ReadXML<T>(string _fileName)
        {
            return ReadXML<T>(new FileStream(_fileName, FileMode.Open));
        }
        public static T ReadXML<T>(FileStream fs)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T readData = (T)serializer.Deserialize(fs);
            fs.Close();
            return readData;
        }
        public static T ReadXML<T>(TextAsset _ass)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader tr = new StringReader(_ass.text);
            T readData = (T)serializer.Deserialize(tr);
            tr.Close();
            return readData;
        }

        public static void WriteXML<T>(T _objectToSerialize, string _path, string _fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string path = Application.persistentDataPath + "/" + _path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream fs = new FileStream(path + "/" + _fileName, FileMode.Create);
            serializer.Serialize(fs, _objectToSerialize);
            fs.Close();
        }
    }
}