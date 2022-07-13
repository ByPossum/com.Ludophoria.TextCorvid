using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TextCorvid
{
    public class ResizableTextBox : MonoBehaviour
    {
        [SerializeField] private int i_minCharactersPerLine;
        [SerializeField] private int i_maxCharactersPerLine;
        [SerializeField] private int i_linesPerBox;
        [SerializeField] private int i_maxHeight;
        private int i_currentWidth, i_currentHeight;
        private float f_currentBoxWidth, f_currentBoxHeight;
        private RectTransform rt_box;
        private SpriteRenderer sr_textBox;
        private bool b_boxSizeSatisfied = false;
        private float f_minWidth, f_maxWidth, f_minHeight, f_maxHeight;

        public int Width { get { return i_currentWidth; } }
        public int Height { get { return i_currentHeight; } }
        public float BoxWidth { get { return f_currentBoxWidth; } }
        public float BoxHeight { get { return f_currentBoxHeight; } }
    
        public void Init(string _text, float _textSize)
        {
            sr_textBox = GetComponent<SpriteRenderer>();
            rt_box = GetComponent<RectTransform>();
            List<string> _bins = CollectTextIntoBins(i_minCharactersPerLine, _text);
            Vector2 _resize = ResizeBox(_text.Length * _textSize, _bins.Count - 1, _textSize, _text);
            f_currentBoxWidth = _resize.x;
            f_currentBoxHeight = _resize.y;
            if (sr_textBox)
            {
                Vector2 newSize = new Vector2(_resize.x * 0.1f, _resize.y * 0.1f);
                sr_textBox.size = newSize;
                sr_textBox.GetComponentInChildren<RectTransform>().sizeDelta = newSize;
                return;
            }
            rt_box.sizeDelta = _resize.x < i_maxCharactersPerLine*_textSize && _resize.y < i_maxHeight * _textSize ? _resize : new Vector2(i_maxCharactersPerLine * _textSize, i_maxHeight * _textSize);
        }

        public void Init(string _text, TMP_Text _container)
        {
            sr_textBox = GetComponent<SpriteRenderer>();
            rt_box = GetComponent<RectTransform>();
            List<string> _bins = CollectTextIntoBins(i_minCharactersPerLine, _text);
            List<string> _dummy = CollectTextIntoBins(i_minCharactersPerLine, _text, _container);
            Vector2 _resize = ResizeBoxV2(_text, _container, _bins);
            f_currentBoxWidth = _resize.x;
            f_currentBoxHeight = _resize.y;
            if (sr_textBox)
            {
                Vector2 newSize = new Vector2(_resize.x * 0.1f, _resize.y * 0.1f);
                sr_textBox.size = newSize;
                sr_textBox.GetComponentInChildren<RectTransform>().sizeDelta = newSize;
                return;
            }
        }
        
        private Vector2 ResizeBoxV2(string _text, TMP_Text _container, List<string> _bins)
        {
            // ============ INITIAL VALUES ============
            // Set Lowest width     (low)   to largest word
            // Set Largest width    (law)   to all words summed
            // Set Lowest height    (loh)   to Number of bins
            // Set Largest height   (lah)   to Total # of words
            // ============ SOLVING VALUES ============
            // Set Width guess      (wg)    to Average Width (low + law / 2)
            // Set Height guess     (hg)    to Average Height (loh + lah / 2)
            // return previous guess when both hg and wg are too small
            // if width can go lower
            //      Set Largest Width to Width Guess
            // if width is too low
            //      Set Lowest Width to Width Guess
            // if height can go lower
            //      Set Largest Height to Height Guess
            // if height is too low
            //      Set Lowest Height to Height Guess
            // Solve again
            // Until you can't go smaller

            return Vector2.zero;
        }

        //private float SolveMaxWidth(TMP_Text _container, List<string> _bins)
        //{

        //}

        private Vector2 ResizeBox(float _previousWidth, int _previousHeight, float _fontSize, string _text)
        {
            float _currentWidth = _previousWidth * 0.5f;
            int _binSize = Mathf.CeilToInt(_currentWidth / _fontSize);
            List<string> _bins = CollectTextIntoBins(_binSize, _text);

            if (_currentWidth / _fontSize > i_minCharactersPerLine && b_boxSizeSatisfied)
                return ResizeBox(_currentWidth, _bins.Count, _fontSize, _text);
            else if (b_boxSizeSatisfied)
                return ResizeBox(_currentWidth + _currentWidth * 0.5f, _bins.Count, _fontSize, _text);
            return new Vector2(_previousWidth, _previousHeight * _fontSize);
        }
    
        private List<string> CollectTextIntoBins(int _binLength, string _text, TMP_Text _box)
        {
            _box.text = _text;
            int linierIter = 0;
            List<string> _binsOfText = new List<string>();
            string[] words = _text.Split(' ');
            string _nextBin = string.Empty;
            foreach (string word in words)
            {
                int tmp = 0;
                int j = 0;
                for(int i = linierIter; i < linierIter + word.Length; i++)
                {
                    int _characterIndex = GetCharacterIndex(word[j], _box.textInfo.characterInfo);
                    Debug.Log($"Word Length {word.Length} | Character {_characterIndex} Width {_box.textInfo.characterInfo[_characterIndex].character}");
                    j++;
                    tmp = linierIter + i;
                }
                linierIter = tmp + 1;
            }
            return _binsOfText;
        }

        private List<string> CollectTextIntoBins(int _binLength, string _text)
        {
            b_boxSizeSatisfied = true;
            List<string> _binsOfText = new List<string>();
            string[] words = _text.Split(' ');
            string _nextBin = string.Empty;
            foreach (string word in words)
            {
                // Cram junk in the bin
                if (_nextBin.Length + word.Length + 1 < _binLength)
                {
                    _nextBin += word + " ";
                    continue;
                }
                // Unsatisfy the box if a word doesn't fit into it
                if(word.Length > _binLength)
                    b_boxSizeSatisfied = false;
                // Add a new row to the bin
                _binsOfText.Add(_nextBin);
                _nextBin = word + " ";
            }
            if (_nextBin.Length > 0)
                _binsOfText.Add(_nextBin.Remove(_nextBin.Length - 1));
            return _binsOfText;
        }
        private int GetCharacterIndex(char _character, TMP_CharacterInfo[] _info)
        {
            // All characters in this Character info are empty...
            // Unity throw me a bone here please for the love of god.
            for (int i = 0; i < _info.Length; i++)
                if (_character == _info[i].character)
                    return i;
            return 0;
        }
    }

}
