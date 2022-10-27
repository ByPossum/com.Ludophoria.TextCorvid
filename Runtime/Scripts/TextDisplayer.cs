using System.Collections;
using System.Collections.Generic;
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
        private TMP_Text t_testBox;
        List<TMP_Text> previousText = new List<TMP_Text>();
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        private int i_textSpeed;
        private float f_resizeTolerance;
        private bool b_done = true;

        public void Init(int _textSpeed, TextAnimator _anim = null)
        {
            i_textSpeed = _textSpeed;
            ta_animator = _anim;

            if (!rt_displayBox)
                rt_displayBox = GetComponent<RectTransform>();
            if (!t_displayedText)
                t_displayedText = GetComponentInChildren<TMP_Text>();
            

        }

        /// <summary>
        /// Choose how the text will be displayed, and on which rect transform.
        /// </summary>
        /// <param name="textToDisplay">String to display (typically gotten from TextManager.x.GetText()</param>
        /// <param name="rectToDisplay">Rect Transform used to display on. Will display on the top left of the rect transform.</param>
        /// <param name="displayType">How the text will be displayed.</param>
        public void DisplayText(string textToDisplay, float rectHeight, TextDisplayType displayType = TextDisplayType.block)
        {
            switch (displayType)
            {
                case TextDisplayType.block:
                    break;
                case TextDisplayType.line:
                    break;
                case TextDisplayType.word:
                    break;
                case TextDisplayType.character:
                    try
                    {
                        DisplayByChar(textToDisplay, rectHeight);
                    }
                    catch (System.NullReferenceException nre)
                    {
                        Debug.LogException(nre);
                    }
                    break;
                default:
                    break;
            }
        }

        public void DisplayText(string textToDisplay, ResizableTextBox rs_textBox = null, TextDisplayType displayType = TextDisplayType.block)
        {
            rs_textBox.Init(ta_animator.RemoveEffects(textToDisplay), t_displayedText);
            DisplayText(textToDisplay, rs_textBox.BoxHeight, displayType);
            GetComponent<SpriteRenderer>().size += new Vector2(rs_textBox.Padding.x + rs_textBox.Padding.z, rs_textBox.Padding.y + rs_textBox.Padding.w);
        }

        public void DisplayText(string textToDisplay, RectTransform rectToDisplay = null, TextDisplayType displayType = TextDisplayType.block)
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
            await ShowNextCharacterByCharacter(textToDisplay, t_displayedText);
            b_done = true;
        }
        
        private void DisplayByBlock(string _textToDisplay, RectTransform _rectToDisplay)
        {

        }

        /// <summary>
        /// Remove old text from the rect.
        /// </summary>
        private void DeleteOldText()
        {
            t_displayedText.text = "";
            previousText = new List<TMP_Text>();
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