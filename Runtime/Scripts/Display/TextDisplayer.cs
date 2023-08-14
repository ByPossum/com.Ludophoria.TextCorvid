﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TextCorvid
{
    public class TextDisplayer : MonoBehaviour
    {
        [SerializeField] private RectTransform rt_displayBox;
        [SerializeField] private TMP_Text t_displayedText;
        private string s_textToBeDisplayed = "";
        List<TMP_Text> previousText = new List<TMP_Text>();
        #region TextBox Sizes
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        #endregion
        private int i_textSpeed;
        private IEnumerator t_currentTask = null;
        [SerializeField] private string s_textID;
        public string TextID { get { return s_textID; } }
        public TMP_Text GetTextObject { get { return t_displayedText; } }

        public void Init(int _textSpeed, string _textID = null)
        {
            i_textSpeed = _textSpeed;
            if (!rt_displayBox)
                rt_displayBox = GetComponent<RectTransform>();
            if (!t_displayedText)
                t_displayedText = GetComponentInChildren<TMP_Text>();
        }

        public void CacheID(string _textID)
        {
            s_textID = _textID;
        }

        public void CacheText(string _textToDisplay)
        {
            s_textToBeDisplayed = _textToDisplay;
            t_displayedText.text = "";
        }

        /// <summary>
        /// Choose how the text will be displayed, and on which rect transform.
        /// </summary>
        /// <param name="textToDisplay">String to display (typically gotten from TextManager.x.GetText()</param>
        /// <param name="rectSize">This is contextual. It's the size of the area to display. DisplayType.line uses width. DisplayType.character uses height.</param>
        /// <param name="displayType">How the text will be displayed.</param>
        public void DisplayText(string textToDisplay, float rectSize, TextDisplayType displayType = TextDisplayType.block)
        {
            switch (displayType)
            {
                case TextDisplayType.block:
                    DisplayByBlock(textToDisplay);
                    return;
                case TextDisplayType.line:
                    DisplayTextByLine(textToDisplay, Mathf.FloorToInt(rectSize));
                    return;
                case TextDisplayType.word:
                    DisplayByWord(textToDisplay);
                    break;
                case TextDisplayType.character:
                    try{ DisplayByChar(textToDisplay, rectSize); }
                    catch (System.NullReferenceException nre){ Debug.LogException(nre); }
                    return;
                default:
                    break;
            }
        }

        public void DisplayText(string textToDisplay, RectTransform rectToDisplay = null, TextDisplayType displayType = TextDisplayType.block)
        {
            DisplayText(textToDisplay, rectToDisplay.rect.height, displayType);
        }

        public void DisplayText(float _rectSize, TextDisplayType _displayType)
        {
            DisplayText(s_textToBeDisplayed, _rectSize, _displayType);
        }

        /// <summary>
        /// Display the text character by character.
        /// </summary>
        /// <param name="textToDisplay">Characters to display.</param>
        /// <param name="rectToDisplay">Where to begin displaying those characters.</param>
        private void DisplayByChar(string textToDisplay, float rectHeight)
        {
            currentRow = 0;
            prevLineCount = 0;
            rowCount = Mathf.FloorToInt(rectHeight / t_displayedText.rectTransform.rect.height);
            DeleteOldText();
            t_currentTask = ShowNextCharacterByCharacter(textToDisplay, t_displayedText);
            StartCoroutine(t_currentTask);
        }
        
        private void DisplayByBlock(string _textToDisplay)
        {
            t_displayedText.text = _textToDisplay;
        }

        private IEnumerator DisplayTextByLine(string _textToDisplay, int _maxLength)
        {
            t_currentTask = DisplayLine(CollectTextIntoBins(_textToDisplay, _maxLength));
            yield return StartCoroutine(t_currentTask);
        }

        private IEnumerator DisplayByWord(string _textToDisplay)
        {
            t_currentTask = DisplayWord(_textToDisplay);
            yield return StartCoroutine(t_currentTask);
        }

        /// <summary>
        /// Remove old text from the rect.
        /// </summary>
        private void DeleteOldText()
        {
            t_displayedText.text = "";
            previousText = new List<TMP_Text>();
        }

        private List<string> CollectTextIntoBins(string _text, int _width)
        {
            string[] words = _text.Split(' ');
            List<string> bins = new List<string>();
            string temp = string.Empty;
            foreach (string word in words)
            {
                if (word.Length + temp.Length + 1 > _width)
                {
                    bins.Add(temp);
                    temp = string.Empty;
                }
                temp += word == string.Empty ? word : " " + word;
            }
            if(temp != string.Empty)
                bins.Add(temp);
            return bins;
        }

        /// <summary>
        /// Displays a character, then waits for "text speed" until all characters are displayed.
        /// </summary>
        /// <param name="nextString">The string to display. Will accomidate for the text being larger than the rect.</param>
        /// <param name="_parent">The area to display the text.</param>
        /// <returns>Waits for text speed.</returns>
        private IEnumerator ShowNextCharacterByCharacter(string nextString, TMP_Text _parent)
        {
            // Show the text character by character
            for(int i = 0; i < nextString.Length; i++)
            {
                // Move to a new row when the next word goes over the textbox width
                if ((prevLineCount * t_displayedText.fontSize) + (t_displayedText.fontSize * nextString.Length) > _parent.rectTransform.rect.width)
                {
                    currentRow++;
                    prevLineCount = 0;
                }
                int iteratorChanger = 0;
                string seg = string.Empty;
                // if there's an effect from TMP just add it all at once
                if (nextString[i] == '<')
                {
                    for (int j = i; j < nextString.Length; j++)
                    {
                        if (nextString[j] == '>')
                        {
                            iteratorChanger = j+1;
                            break;
                        }
                    }
                    int charLen = iteratorChanger - i;
                    seg = nextString.Substring(i, charLen);
                    i = iteratorChanger;
                }
                else
                    seg = nextString[i].ToString();
                _parent.text += seg;
                previousText.Add(_parent);
                prevLineCount++;
                yield return new WaitForSeconds(i_textSpeed*0.01f);
            }
        }

        private IEnumerator DisplayLine(List<string> _lines)
        {
            t_displayedText.text = "";
            foreach(string _line in _lines)
            {
                t_displayedText.text += _line;
                yield return new WaitForSeconds(i_textSpeed);
            }
        }

        private IEnumerator DisplayWord(string _words)
        {
            t_displayedText.text = "";
            foreach (string word in _words.Split(' '))
            {
                t_displayedText.text += word + " ";
                yield return new WaitForSeconds(i_textSpeed);
            }
        }

        private void OnDisable()
        {
            //t_displayedText.text = s_textID;
        }

        public void ClearDisplayedText()
        {
            t_displayedText.text = "";
        }

        public void PreviewText(string _ttd)
        {
            t_displayedText.text = _ttd;
        }
    }
    
    public enum TextDisplayType
    {
        none,       // Don't display
        block,      // Display all text at once
        line,       // Display line by line
        word,       // Display word by word
        character   // Display character by character
    }

}