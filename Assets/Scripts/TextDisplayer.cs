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
        List<Text> previousText = new List<Text>();
        private int prevLineCount;
        private int currentRow;
        private int rowCount;
        public bool b_done = true;
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
            b_done = false;
            currentRow = 0;
            prevLineCount = 0;
            rowCount = Mathf.FloorToInt(rectToDisplay.rect.height / refText.preferredHeight);
            string[] _words = textToDisplay.Split(' ');
            DeleteOldText();
            StartCoroutine(ShowNextWord(textToDisplay, rectToDisplay));
            b_done = true;
        }
        
        private void DeleteOldText()
        {
            foreach(Text killMe in previousText)
            {
                DestroyImmediate(killMe.gameObject);
            }
            previousText = new List<Text>();
        }

        private IEnumerator ShowNextWord(string nextWord, RectTransform _parent)
        {
            // Show the text character by character
            for(int i = 0; i < nextWord.Length; i++)
            {
                // Move to a new row when the next word goes over the textbox width
                if ((prevLineCount * refText.fontSize) + (refText.fontSize * nextWord.Length) > _parent.rect.width)
                {
                    currentRow++;
                    prevLineCount = 0;
                }
                Text newText = Instantiate<Text>(refText);
                newText.text = nextWord[i].ToString();
                newText.transform.parent = _parent;
                // Set the top left of the text to the top left of the box with character offsets (Not fully working yet)
                newText.rectTransform.offsetMin = new Vector2((_parent.offsetMin.x + refText.fontSize) + prevLineCount * refText.fontSize, _parent.offsetMin.y);
                newText.rectTransform.offsetMax = new Vector2(_parent.offsetMax.x, (_parent.offsetMax.y + refText.fontSize) + refText.fontSize * (currentRow));
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
        character  // Display character by character
    }

}