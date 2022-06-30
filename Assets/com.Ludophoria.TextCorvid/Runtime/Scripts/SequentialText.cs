using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TextCorvid
{
    public class SequentialText : MonoBehaviour, IInputSignal
    {
        [SerializeField] private GameObject[] go_textBoxes;
        [SerializeField] private int i_memory;
        private float f_targetY;
        private int i_currentIndex;
        private List<float> fL_heights = new List<float>();
        private bool b_takingInput = false;
        private bool b_done = false;
        public void FireInput()
        {
            if (b_takingInput)
            {
                f_targetY = transform.position.y;
                IncrementTextBox();
            }
        }

        public bool GetDone()
        {
            return b_done;
        }

        public void ToggleInput()
        {
            b_takingInput = ! b_takingInput;
        }

        private void IncrementTextBox()
        {
            b_takingInput = false;
            if (i_currentIndex >= go_textBoxes.Length)
                foreach (GameObject box in go_textBoxes)
                {
                    box.SetActive(false);
                    b_done = true;
                }
            // The current index of the text box we're looking at
            i_currentIndex++;
            for (int i = go_textBoxes.Length-1; i >= 0; i--)
            {
                if (i < i_currentIndex && i >= i_currentIndex - i_memory)
                {
                    go_textBoxes[i].SetActive(true);
                    StartCoroutine(MoveTheDangTextBox(go_textBoxes[i]));
                }
                else
                    go_textBoxes[i].SetActive(false);
            }
            b_takingInput = true;
        }

        private IEnumerator MoveTheDangTextBox(GameObject _textBox)
        {
            Vector3 _startPos = _textBox.transform.position;
            float _start = _startPos.y;
            float _height = _textBox.GetComponentInChildren<ResizableTextBox>() ? _textBox.GetComponentInChildren<ResizableTextBox>().BoxHeight :
                _textBox.GetComponentInChildren<RectTransform>() ? _textBox.GetComponentInChildren<RectTransform>().rect.height : 1f;
            float totalHeight = 0;
            fL_heights.Add(_height);
            foreach (float _h in fL_heights)
            {
                totalHeight += _h;
            }
            for (float i = 0; i < 1; i+=Time.deltaTime)
            {
                Vector3 _endPos = _startPos;
                _endPos.y = _endPos.y + totalHeight;
                Vector3 _pos = Vector3.Lerp(_startPos, _endPos, i);
                _textBox.transform.position = _pos;
                yield return new WaitForEndOfFrame();
            }
            fL_heights.Clear();
        }
    }
}
