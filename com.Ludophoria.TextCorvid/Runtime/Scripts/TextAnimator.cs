using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Reflection;
public class TextAnimator : MonoBehaviour
{
    private Regex reg_effectCharacters = new Regex("<.*?>");
    private List<int> Li_delegateTextLengths = new List<int>();

    /// <summary>
    /// Passes text off to have effects run and removes the effect tags
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_currentTextToAnimate"></param>
    public string ParseAnimations(TMP_Text _text, string _currentTextToAnimate)
    {
        // Find all the effects
        MatchCollection effects = reg_effectCharacters.Matches(_currentTextToAnimate);
        List<string> entries = effects.Cast<Match>().Select(m => m.Value).ToList();
        foreach (string entry in entries)
            Debug.Log(entry);
        AssignTextDelegates(MatchTags(effects, _currentTextToAnimate));
        return RemoveEffects(effects, _currentTextToAnimate);
    }

    /// <summary>
    /// Search for the index of each match
    /// </summary>
    /// <param name="_matches">All matches</param>
    /// <returns>The int position of all matches.</returns>
    public List<int> GetAllMatchIndicies(MatchCollection _matches)
    {
        // I don't know if this is doing what I want it to do...
        List<int> _allIndex = new List<int>();
        foreach (Match _start in _matches)
        {
            _allIndex.Add(_start.Index);
        }
        return _allIndex;
    }

    private string RemoveEffects(MatchCollection _matches, string _originalString)
    {
        foreach (Match _match in _matches)
            _originalString = _originalString.Replace(_match.Value, "");
        return _originalString;
    }

    public List<(TextEffect _effect, string[] _args)> AssignTextDelegates(List<(Match _match, int _start, int _length)> _textDelegates)
    {
        List<(TextEffect _effect, string[] _args)> _allEffects = new List<(TextEffect _effect, string[] _args)>();
        for (int i = 0; i < _textDelegates.Count; i++)
        {
            // Get all the shit
            string[] allTheShit = _textDelegates[i]._match.Value.Split(' ');
            string effectName = allTheShit[0].Replace("<", "").Replace(">", "");
            // Initialize array
            int argLen = allTheShit.Length + 2;
            string[] _args = new string[argLen];
            // Add the initial arguments
            _args[0] = _textDelegates[i]._start.ToString();
            _args[1] = _textDelegates[i]._length.ToString();
            // Parse each argument from the match string
            for (int j = 1; j < allTheShit.Length; j++)
                _args[j + 2] = allTheShit[i];
            
            try
            {
                /*object newObj = new object();
                TextEffect newEffect = Delegate.CreateDelegate(typeof(TextEffect), newObj, effectName) as TextEffect;
                _allEffects.Add((newEffect, _args));
                Debug.Log($"{effectName} found!");*/
            }
            catch (NullReferenceException e)
            {
                Debug.LogError($"{effectName} is not a valid function.");
            }
        }
        return _allEffects;
    }

    public void Colour(params string[] _color)
    {

        List<(string argName, string argValue)> _arguments = ParseArguments(_color);
        float r = TryCast<float>(_arguments[0].argValue);
        float g = TryCast<float>(_arguments[1].argValue);
        float b = TryCast<float>(_arguments[2].argValue);
        Color textSegmentColor = new Color(r, g, b);
    }
    
    private void Wiggle(params string[] _args)
    {

    }

    private List<(string argName, string argValue)> ParseArguments(params string[] _args)
    {
        List<(string argName, string argValue)> _arguments = new List<(string argName, string argValue)>();
        for (int i = 0; i < _args.Length; i++)
        {
            if (_args[i].Contains('='))
            {
                if (_args[i].Contains('>'))
                    _args[i] = _args[i].Replace('>', '\n');
                Debug.Log($"part 1: {_args[i].Split('=')[0]} | part 2: {_args[i].Split('=')[1]}");
                _arguments.Add((_args[i].Split('=')[0], _args[i].Split('=')[1]));
            }
        }
        return _arguments;
    }

    private List<(Match, int, int)> MatchTags(MatchCollection _allTags, string _allText)
    {
        List<(Match matches, int start, int length)> _effects = new List<(Match matches, int start, int length)>();
        // Find Opening Tag
        for (int i = 0; i < _allTags.Count; i++)
        {
            // Ignore closing tags
            if (_allTags[i].Value.Contains("</"))
                continue;
            // Grab the data we need from the opening tag
            string _fullTag = _allTags[i].Value;
            int _startIndex = _allTags[i].Index; // Minus the length of any previous tags
            int _startLength = _fullTag.Length;
            string _effect = _fullTag.Split(' ')[0].Replace("<", "").Replace(">" ,"");
            // Initialize the data we need to find the closing tag
            int _totalLength = 0;
            string _endTag = "";
            int _endIndex = 0;
            int _endLength = 0;
            int _startOccurances = 0;
            for (int j = i+1; j < _allTags.Count; j++)
            {
                // Check if this is the correct end tag
                if (_allTags[j].Value.Contains("</"))
                {
                    if (_startOccurances < 1)
                    {
                        if (_allTags[j].Value.Contains(_effect))
                        {
                            // Assign all the data to the end tag
                            _endTag = _allTags[j].Value;
                            _endIndex = _allTags[j].Index;
                            _endLength = _allTags[j].Value.Length;
                            _effects.Add((_allTags[i], _startLength, _endLength));
                            break;
                        }
                    }
                    _totalLength += _allTags[j].Value.Length;
                    // Reduce the number of opening tag occurences
                    if(_allTags[j].Value.Contains(_effect))
                        _startOccurances--;
                }
                else
                {
                    _totalLength += _allTags[j].Value.Length;
                    // There is an opening tag of the same type, so ignore the next closing tag
                    if(_allTags[j].Value.Contains(_effect))
                        _startOccurances++;
                }
            }
            int totalLength = (_endIndex-1 - (_startIndex + _startLength-1)) - _totalLength;
            Debug.Log($"OTL: {_totalLength} | Effected Area: {totalLength} | Tag: {_fullTag} | Index: {_startIndex} | Length: {_startLength} | End: {_endTag} | End Index: {_endIndex} | End Length: {_endLength}");
        }
        // Count all Opening Tags that occur before Closing Tag and obtain the lengths of each tag
        return _effects;
    }

    #region Generics
    // Move this to a seperate static class
    private T TryCast<T>(string _object) where T : new()
    {
        try
        {
            return (T)Convert.ChangeType(_object, typeof(T));
        }
        catch(Exception e)
        {
            if(e is InvalidCastException || e is FormatException)
            {
                Debug.LogError($"Cannot convert {_object} to {typeof(T)}");
            }
            return new T();
        }
    }
    private List<T> TryMultiCast<T>(string _object) where T : new()
    {
        List<T> _output = new List<T>();
        try
        {
            if (_object.Contains(','))
            {
                foreach (string _value in _object.Split(','))
                {
                    Debug.Log(_value);
                    _output.Add((T)Convert.ChangeType(_object, typeof(T)));
                }
            }
            return _output;
        }
        catch(Exception e)
        {
            if(e is InvalidCastException || e is FormatException)
            {
                Debug.LogError($"Cannont convert {_object} to {typeof(T)} collection \n Specific error: {e}");
            }
            return _output;
        }
    }
    #endregion
}

public delegate void TextEffect(params string[] _strings);