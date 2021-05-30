using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextCorvid
{
    public class TextDisplayer : MonoBehaviour
    {
        public List<string> textKeys = new List<string>();
        public Text refText;
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
    
        private void DisplayByChar(string textToDisplay, RectTransform rectToDisplay)
        {
            int prevLineCount = 0;
            int currentRow = 0;
            int rowCount = Mathf.FloorToInt(rectToDisplay.rect.height / refText.preferredHeight);
            Debug.Log("Am running");
            foreach(string word in textToDisplay.Split(' '))
            {
                // Move to a new row when the next word goes over the textbox width
                if ((prevLineCount * refText.preferredWidth) + (refText.preferredWidth * word.Length) > rectToDisplay.rect.width)
                {
                    currentRow++;
                    prevLineCount = 0;
                }
                // Show the text character by character
                foreach (char letter in word)
                {
                    StartCoroutine(ShowNextChar(letter, new Vector3(refText.preferredWidth * prevLineCount, refText.preferredHeight * currentRow, rectToDisplay.transform.position.z), rectToDisplay));
                    prevLineCount++;
                }
            }
        }
    
        private IEnumerator ShowNextChar(char nextChar, Vector3 nextPos, RectTransform _parent)
        {
            yield return new WaitForSeconds(TextManager.x.f_textSpeed);
            Text newText = Instantiate<Text>(refText);
            newText.text = nextChar.ToString();
            newText.transform.position = nextPos;
            newText.transform.parent = _parent;
        }
    }
    
    public enum TextDisplayType
    {
        none,       // Don't display
        block,      // Display all text at once
        line,       // Display line by line
        word,       // Display word by word
        character  // Display character by character
    }

}