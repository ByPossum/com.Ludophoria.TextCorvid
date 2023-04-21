using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace TextCorvid
{
    // Generic Casting system
    public static class GCaster
    {
        #region Generics
        public static T TryCast<T>(string _object) where T : new()
        {
            try
            {
                return (T)Convert.ChangeType(_object, typeof(T));
            }
            catch (Exception e)
            {
                if (e is InvalidCastException || e is FormatException)
                {
                    Debug.LogError($"Cannot convert {_object} to {typeof(T)}");
                }
                return new T();
            }
        }
        public static List<T> TryMultiCast<T>(string _object) where T : new()
        {
            List<T> _output = new List<T>();
            try
            {
                if (_object.Contains(","))
                {
                    foreach (string _value in _object.Split(','))
                    {
                        Debug.Log(_value);
                        _output.Add((T)Convert.ChangeType(_object, typeof(T)));
                    }
                }
                return _output;
            }
            catch (Exception e)
            {
                if (e is InvalidCastException || e is FormatException)
                {
                    Debug.LogError($"Cannont convert {_object} to {typeof(T)} collection \n Specific error: {e}");
                }
                return _output;
            }
        }
        #endregion
    }
}