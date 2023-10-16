using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace TextCorvid
{
    public class ResizableTextFrame : Frame
    {
        private RectTransform rt_box;
        private SpriteRenderer sr_textBox;
        private bool b_boxSizeSatisfied = false;
        [SerializeField] private float f_minWidth, f_maxWidth;
        private float f_minHeight, f_maxHeight;
        [SerializeField, Tooltip("Left, Top, Right, Bottom")] private Vector4 v_padding;
        private float f_width, f_height;
        private float f_tolerance;
        private Dictionary<float, float> D_scores = new Dictionary<float, float>();
        public float BoxWidth { get { return f_width; } }
        public float BoxHeight { get { return f_height; } }
        public Vector4 Padding { get { return v_padding; } }
        public override void Init(string _text, TMP_Text _container)
        {
            // Never let this bish crash
            sr_textBox = sr_textBox ?? GetComponent<SpriteRenderer>();
            rt_box = rt_box ?? GetComponentInParent<RectTransform>();

            f_tolerance = _text.Length + _text.Length * 0.1f;
            Vector2 _resize = ResizeBox(f_minWidth, f_maxWidth, _text, _text.Length);
            
            if (sr_textBox)
            {
                Vector2 newSize = new Vector2(_resize.x * _container.fontSize * 0.1f, _resize.y + (_container.fontSize * 0.1f));
                sr_textBox.size = newSize;
                f_width = newSize.x;
                f_height = newSize.y;
                sr_textBox.GetComponentInChildren<RectTransform>().sizeDelta = newSize;
            }
            else if (rt_box)
            {
                Vector2 newSize = new Vector2(_resize.x * _container.fontSize, _resize.y + (_container.fontSize + _container.fontSize));
                rt_box.sizeDelta = newSize;
            }
            UpdateForPadding(new Vector2(v_padding.x + v_padding.z, v_padding.y + v_padding.w));
        }

        public void UpdateForPadding(Vector2 _pad)
        {
            if (rt_box)
                rt_box.GetComponentInParent<RectTransform>().sizeDelta += _pad;
        }
        
        private Vector2 ResizeBox(float _minX, float _maxX, string _text, float _target)
        {
            // Get our mid
            float _midX = (_minX + _maxX) * 0.5f;
            List<string> bins = CollectTextIntoBins(Mathf.CeilToInt(_midX), _text);
            int midY = bins.Count + 1;
            float area = _midX * midY;

            // Have we evaluated it already?
            if (!D_scores.ContainsKey(_midX))
                D_scores.Add(_midX, area);

            // When we've evaluated both our closest neighbours we're done
            if ((D_scores.ContainsKey((_minX + _midX) * 0.5f)) && (D_scores.ContainsKey((_midX + _maxX) * 0.5f)))
                return GetBestScore(_text);

            // Check which neighbour to try next
            if (!D_scores.ContainsKey((_minX + _midX) * 0.5f) && ((_minX + _midX) * 0.5f) > f_minWidth)
                return ResizeBox(_minX, _midX, _text, _target);
            if (!D_scores.ContainsKey((_midX + _maxX) * 0.5f) && ((_midX + _maxX) * 0.5f) < f_maxWidth)
                return ResizeBox(_midX, _maxX, _text, _target);

            // If we've fallen through here we've probably hit a min/max so return
            return GetBestScore(_text);
        }

        private Vector2 GetBestScore(string _text)
        {
            float bestScore = 10000f;
            float bestX = 100000000f;
            foreach (float key in D_scores.Keys)
                if (D_scores[key] < bestScore)
                {
                    bestScore = D_scores[key];
                    bestX = key;
                }
            return new Vector2(bestX, CollectTextIntoBins(Mathf.CeilToInt(bestX), _text).Count + 1);
        }

        private List<string> CollectTextIntoBins(int _binLength, string _text)
        {
            List<string> _binsOfText = new List<string>();
            string[] words = _text.Split(' ');
            string _nextBin = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                // Cram junk in the bin
                if (_nextBin.Length + words[i].Length + 1 > _binLength)
                {
                    _binsOfText.Add(_nextBin);
                    _nextBin = words[i];
                }
                // Add a new row to the bin
                _binsOfText.Add(_nextBin);
                _nextBin = " " + words[i];
            }
            _binsOfText.Add(_nextBin);
            return _binsOfText;
        }

        public override void Preview(string _text, TMP_Text _container)
        {
#if UNITY_EDITOR
            sr_textBox = sr_textBox ?? GetComponent<SpriteRenderer>();
            rt_box = rt_box ?? GetComponentInParent<RectTransform>();

            f_tolerance = _text.Length + _text.Length * 0.1f;
            Vector2 _resize = ResizeBox(f_minWidth, f_maxWidth, _text, _text.Length);
            if (sr_textBox)
            {
                Vector2 newSize = new Vector2(_resize.x * _container.fontSize * 0.1f, _resize.y + (_container.fontSize * 0.1f));
                sr_textBox.size = newSize;
                f_width = newSize.x;
                f_height = newSize.y;
                sr_textBox.GetComponentInChildren<RectTransform>().sizeDelta = newSize;
            }
            else if (rt_box)
            {
                Vector2 newSize = new Vector2(_resize.x * _container.fontSize, _resize.y + (_container.fontSize + _container.fontSize));
                rt_box.sizeDelta = newSize;
            }
            UpdateForPadding(new Vector2(v_padding.x + v_padding.z, v_padding.y + v_padding.w));
#endif
        }
    }

}
