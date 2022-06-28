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
        private RectTransform rt_box;
        private bool b_boxSizeSatisfied = false;
    
        public int Width { get { return i_currentWidth; } }
        public int Height { get { return i_currentHeight; } }
    
        public void Init(string _text, float _textSize)
        {
            rt_box = GetComponent<RectTransform>();
            List<string> _bins = CollectTextIntoBins(i_minCharactersPerLine, _text);
            Vector2 _resize = ResizeBoxV2(_text.Length * _textSize, _bins.Count - 1, _textSize, _text);
            rt_box.sizeDelta = _resize.x < i_maxCharactersPerLine*_textSize && _resize.y < i_maxHeight * _textSize ? _resize : new Vector2(i_maxCharactersPerLine * _textSize, i_maxHeight * _textSize);
            //rt_box.sizeDelta = ResizeBox(i_maxWidth, i_maxHeight, _textSize, _text.Length, _text);
        }
    
        private Vector2 ResizeBoxV2(float _previousWidth, int _previousHeight, float _fontSize, string _text)
        {
            float _currentWidth = _previousWidth * 0.5f;
            int _binSize = Mathf.CeilToInt(_currentWidth / _fontSize);
            List<string> _bins = CollectTextIntoBins(_binSize, _text);
            if (_currentWidth / _fontSize > i_minCharactersPerLine && _currentWidth / _fontSize < i_maxCharactersPerLine && b_boxSizeSatisfied)
                return ResizeBoxV2(_currentWidth, _bins.Count, _fontSize, _text);
            return new Vector2(_previousWidth, _previousHeight * _fontSize);
        }
    
        private Vector2 ResizeBox(float _previousWidth, float _previousHeight, float _textSize, int _textLength, string _text)
        {
            // Half the previous width
            float _currentWidth = _previousWidth * 0.5f;
            int _currentBins = Mathf.CeilToInt(_currentWidth / _textSize);
    
            List<string> _binsOfText = CollectTextIntoBins(_currentBins, _text);
            
            Debug.Log($"Width: {_currentWidth} | Bins: {_currentWidth / _textSize} (Int: {_currentBins}) | Text Size: {_textSize} | What's in our bin: {_binsOfText.Count}");
            // Generate height from the bin count
            float _height = _binsOfText.Count * _textSize;
    
            // Try and go smaller
            if (_height < _previousHeight)
                return ResizeBox(_currentWidth, _height, _textSize, _textLength, _text);
    
            // Go a bit bigger
            float _widthTimesAQuarter = _currentWidth * 1.25f;
            int _largerBinSize = Mathf.CeilToInt(_widthTimesAQuarter / _textSize);
            List<string> _largerBins = CollectTextIntoBins(_largerBinSize, _text);
            float _newHeight = _largerBins.Count * _textSize;
            Vector2 _resizeTest = ResizeBox(_currentWidth * 1.25f, _newHeight, _textSize, _textLength, _text);
            // Try again to go smaller
            if (_resizeTest.y < _previousHeight)
                return ResizeBox(_widthTimesAQuarter, _newHeight, _textSize, _textLength, _text);
            // Return smallest Value
            return new Vector2(_currentWidth, i_currentHeight);
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
    }
}
