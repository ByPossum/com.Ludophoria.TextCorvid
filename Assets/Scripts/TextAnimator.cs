using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class TextAnimator : MonoBehaviour
{
    private Regex reg_effectCharacters = new Regex("<.*?>");
    private Regex reg_start = new Regex("<");
    private Regex reg_end = new Regex(">");
    private List<int> Li_delegateTextLengths = new List<int>();
    public void ParseAnimations(TMP_Text _text, string _currentTextToAnimate)
    {
        // Find all the effects
        MatchCollection effects = reg_effectCharacters.Matches(_currentTextToAnimate);
        List<string> entries = effects.Cast<Match>().Select(m => m.Value).ToList();
        foreach (string entry in entries)
            Debug.Log(entry);
        // Get all start and end positions of the text
        List<int> _starts = GetAllMatchIndicies(reg_start.Matches(_currentTextToAnimate));
        List<int> _ends = GetAllMatchIndicies(reg_end.Matches(_currentTextToAnimate));
        AssignTextDelegates(effects);

    }

    /// <summary>
    /// Search for the index of each match
    /// </summary>
    /// <param name="_matches">All matches</param>
    /// <returns>The int position of all matches.</returns>
    public List<int> GetAllMatchIndicies(MatchCollection _matches)
    {
        List<int> _allIndex = new List<int>();
        foreach (Match _start in _matches)
            _allIndex.Add(_start.Index);
        return _allIndex;
    }

    public List<(TextEffect _effect, string[] _args)> AssignTextDelegates(MatchCollection _textDelegates)
    {
        List<(TextEffect _effect, string[] _args)> _allEffects = new List<(TextEffect _effect, string[] _args)>();
        int previousDelegateLength = 0;
        foreach(Match _newDelegate in _textDelegates)
        {
            // Split the delegate into Name and Arguments
            string[] delegateAndArgs = _newDelegate.ToString().Split(' ');
            if (!delegateAndArgs[0].Contains('/'))
            {
                // NB: Add switch case with enum parse or some kind of reflection here to get the correct text effect
                TextEffect _newEffect = ChangeTextColour;
                // This is just to test the function
                _newEffect(delegateAndArgs);
                // Store the effect to apply
                _allEffects.Add((_newEffect, delegateAndArgs));
                // NEXT STEP: RUN THE FUNCTIONS ASSUMIDLY BY LOOPING THROUGH START/ENDS ALONGSIDE ALLEFFECTS
            }
            // This might actually not matter? I'm getting the total length of all the delegates???
            Li_delegateTextLengths.Add(previousDelegateLength += _newDelegate.ToString().Length);
        }
        return _allEffects;
    }

    private void ChangeTextColour(params string[] _color)
    {

        List<(string argName, string argValue)> _arguments = ParseArguments(_color);
        float r = TryCast<float>(_arguments[0].argValue);
        float g = TryCast<float>(_arguments[1].argValue);
        float b = TryCast<float>(_arguments[2].argValue);
        Color textSegmentColor = new Color(r, g, b);
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
}

public delegate void TextEffect(params string[] _strings);