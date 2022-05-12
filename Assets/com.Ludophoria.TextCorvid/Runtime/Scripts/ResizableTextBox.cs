using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResizableTextBox : MonoBehaviour
{
    [SerializeField] private int i_maxWidth, i_maxHeight;
    private int i_currentWidth, i_currentHeight;
    private RectTransform rt_box;

    public int Width { get { return i_currentWidth; } }
    public int Height { get { return i_currentHeight; } }

    public void Init(string _text, float _textSize)
    {
        rt_box = GetComponent<RectTransform>();
        float maxBins = i_maxHeight / _textSize;
        float maxSize = (_text.Length * _textSize) / maxBins;
        float height = maxBins * _textSize;
        rt_box.sizeDelta = new Vector2(maxSize, height);
    }
}
