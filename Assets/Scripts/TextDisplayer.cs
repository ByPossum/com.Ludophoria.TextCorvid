using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextCorvid
{
    public class TextDisplayer : MonoBehaviour
    {
        [SerializeField] private RectTransform rt_displayBox;
        public List<string> textKeys = new List<string>();
        public Text refText;
        List<Text> previousText = new List<Text>();
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        public bool b_done = true;

        /// <summary>
        /// Choose how the text will be displayed, and on which rect transform.
        /// </summary>
        /// <param name="textToDisplay">String to display (typically gotten from TextManager.x.GetText()</param>
        /// <param name="rectToDisplay">Rect Transform used to display on. Will display on the top left of the rect transform.</param>
        /// <param name="displayType">How the text will be displayed.</param>
        public void DisplayText(string textToDisplay, RectTransform rectToDisplay, TextDisplayType displayType)
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
                    DisplayByChar(textToDisplay, rectToDisplay);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Choose how the text will be displayed. Will be displayed on serialized rect transform.
        /// </summary>
        /// <param name="textToDisplay">String to display (typically gotten from TextManager.x.GetText()</param>
        /// <param name="displayType">How the text will be displayed</param>
        public void DisplayText(string textToDisplay, TextDisplayType displayType)
        {
            switch (displayType)
            {
                case TextDisplayType.none:
                    break;
                case TextDisplayType.block:
                    break;
                case TextDisplayType.line:
                    break;
                case TextDisplayType.word:
                    break;
                case TextDisplayType.character:
                    try
                    {
                        DisplayByChar(textToDisplay, rt_displayBox);
                    }
                    catch(System.NullReferenceException nre)
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
        private void DisplayByChar(string textToDisplay, RectTransform rectToDisplay)
        {
            b_done = false;
            currentRow = 0;
            prevLineCount = 0;
            rowCount = Mathf.FloorToInt(rectToDisplay.rect.height / refText.preferredHeight);
            string[] _words = textToDisplay.Split(' ');
            DeleteOldText();
            StartCoroutine(ShowNextCharacterByCharacter(textToDisplay, rectToDisplay));
            b_done = true;
        }
        
        /// <summary>
        /// Remove old text from the rect.
        /// </summary>
        private void DeleteOldText()
        {
            foreach(Text killMe in previousText)
            {
                DestroyImmediate(killMe.gameObject);
            }
            previousText = new List<Text>();
        }

        /// <summary>
        /// Displays a character, then waits for "text speed" until all characters are displayed.
        /// </summary>
        /// <param name="nextString">The string to display. Will accomidate for the text being larger than the rect.</param>
        /// <param name="_parent">The area to display the text.</param>
        /// <returns>Waits for text speed.</returns>
        private IEnumerator ShowNextCharacterByCharacter(string nextString, RectTransform _parent)
        {
            // Show the text character by character
            for(int i = 0; i < nextString.Length; i++)
            {
                // Move to a new row when the next word goes over the textbox width
                if ((prevLineCount * refText.fontSize) + (refText.fontSize * nextString.Length) > _parent.rect.width)
                {
                    currentRow++;
                    prevLineCount = 0;
                }
                Text newText = Instantiate<Text>(refText);
                newText.text = nextString[i].ToString();
                newText.transform.parent = _parent;
                // Set the top left of the text to the top left of the box with character offsets (Not fully working yet)
                newText.rectTransform.offsetMin = new Vector2((_parent.offsetMin.x + refText.fontSize * 3) + prevLineCount * refText.fontSize, ((_parent.offsetMax.x - _parent.offsetMax.x) - refText.fontSize * 3) - refText.fontSize * (currentRow));
                //newText.rectTransform.offsetMax = new Vector2(_parent.offsetMax.x, (_parent.offsetMax.y + refText.fontSize) + refText.fontSize * (currentRow));
                newText.rectTransform.sizeDelta = new Vector2(100, 100);
                previousText.Add(newText);
                prevLineCount++;
                yield return new WaitForSeconds(TextManager.x.f_textSpeed);
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