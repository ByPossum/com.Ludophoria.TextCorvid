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
        private TextAnimator ta_animator;
        List<TMP_Text> previousText = new List<TMP_Text>();
        #region TextBox Sizes
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        #endregion
        private int i_textSpeed;
        private bool b_done = true;
        private Task t_currentTask = null;
        private string s_textID;
        public string TextID { get { return s_textID; } }

        public void Init(int _textSpeed, TextAnimator _anim = null, string _textID = null)
        {
            i_textSpeed = _textSpeed;
            ta_animator = _anim;
            if (!rt_displayBox)
                rt_displayBox = GetComponent<RectTransform>();
            if (!t_displayedText)
                t_displayedText = GetComponentInChildren<TMP_Text>();
            s_textID = _textID != null ? _textID : t_displayedText.text;
        }

        /// <summary>
        /// Choose how the text will be displayed, and on which rect transform.
        /// </summary>
        /// <param name="textToDisplay">String to display (typically gotten from TextManager.x.GetText()</param>
        /// <param name="rectSize">This is contextual. It's the size of the area to display. DisplayType.line uses width. DisplayType.character uses height.</param>
        /// <param name="displayType">How the text will be displayed.</param>
        public void DisplayText(string textToDisplay, float rectSize, TextDisplayType displayType = TextDisplayType.block, CharacterDisplayer _cd = null)
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

        public void DisplayText(string textToDisplay, ResizableTextBox rs_textBox = null, TextDisplayType displayType = TextDisplayType.block, CharacterDisplayer _cd = null)
        {
            rs_textBox.Init(ta_animator.RemoveEffects(textToDisplay), t_displayedText);
            DisplayText(textToDisplay, rs_textBox.BoxHeight, displayType);
            GetComponent<SpriteRenderer>().size += new Vector2(rs_textBox.Padding.x + rs_textBox.Padding.z, rs_textBox.Padding.y + rs_textBox.Padding.w);
        }

        public void DisplayText(string textToDisplay, RectTransform rectToDisplay = null, TextDisplayType displayType = TextDisplayType.block, CharacterDisplayer _cd = null)
        {
            DisplayText(textToDisplay, rectToDisplay.rect.height, displayType);
        }

        /// <summary>
        /// Display the text character by character.
        /// </summary>
        /// <param name="textToDisplay">Characters to display.</param>
        /// <param name="rectToDisplay">Where to begin displaying those characters.</param>
        private async void DisplayByChar(string textToDisplay, float rectHeight)
        {
            b_done = false;
            currentRow = 0;
            prevLineCount = 0;
            rowCount = Mathf.FloorToInt(rectHeight / t_displayedText.rectTransform.rect.height);
            DeleteOldText();
            textToDisplay = ta_animator.ParseAnimations(t_displayedText, textToDisplay);
            t_currentTask = ShowNextCharacterByCharacter(textToDisplay, t_displayedText);
            await t_currentTask;
            b_done = true;
        }
        
        private void DisplayByBlock(string _textToDisplay)
        {
            b_done = false;
            t_displayedText.text = ta_animator.ParseAnimations(t_displayedText, _textToDisplay);
            b_done = true;
        }

        private async void DisplayTextByLine(string _textToDisplay, int _maxLength)
        {
            b_done = false;
            t_currentTask = DisplayLine(CollectTextIntoBins(_textToDisplay, _maxLength));
            await t_currentTask;
            b_done = true;
        }

        private async void DisplayByWord(string _textToDisplay)
        {
            b_done = false;
            t_currentTask = DisplayWord(_textToDisplay);
            await t_currentTask;
            b_done = true;
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
        private async Task ShowNextCharacterByCharacter(string nextString, TMP_Text _parent)
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
                await Task.Delay(i_textSpeed);
            }
        }

        private async Task DisplayLine(List<string> _lines)
        {
            t_displayedText.text = "";
            foreach(string _line in _lines)
            {
                t_displayedText.text += _line;
                await Task.Delay(i_textSpeed);
            }
        }

        private async Task DisplayWord(string _words)
        {
            t_displayedText.text = "";
            foreach (string word in _words.Split(' '))
            {
                t_displayedText.text += word + " ";
                await Task.Delay(i_textSpeed);
            }
        }

        private void OnDisable()
        {
            //t_displayedText.text = s_textID;
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