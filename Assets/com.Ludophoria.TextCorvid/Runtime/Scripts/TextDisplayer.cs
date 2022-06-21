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
        [SerializeField, InspectorName("Parent Rect Transform")] private RectTransform rt_displayBox;
        [SerializeField, InspectorName("Displayed Text")] private TMP_Text t_displayedText;
        private TextAnimator ta_animator;
        List<TMP_Text> previousText = new List<TMP_Text>();
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        private int i_textSpeed;
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
        public void DisplayText(string textToDisplay, RectTransform rectToDisplay = null, TextDisplayType displayType = TextDisplayType.block)
        {
            if (rectToDisplay == null)
                rectToDisplay = rt_displayBox;
            if (rectToDisplay.GetComponent<ResizableTextBox>() != null)
                rectToDisplay.GetComponent<ResizableTextBox>()?.Init(ta_animator.RemoveEffects(textToDisplay), t_displayedText.fontSize);

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
                        DisplayByChar(textToDisplay, rectToDisplay);
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

        /// <summary>
        /// Display the text character by character.
        /// </summary>
        /// <param name="textToDisplay">Characters to display.</param>
        /// <param name="rectToDisplay">Where to begin displaying those characters.</param>
        private async void DisplayByChar(string textToDisplay, RectTransform rectToDisplay)
        {
            b_done = false;
            currentRow = 0;
            prevLineCount = 0;
            rowCount = Mathf.FloorToInt(rectToDisplay.rect.height / t_displayedText.rectTransform.rect.height);
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
                _parent.text += nextString[i].ToString();
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