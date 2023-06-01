using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TextCorvid
{
    public class FrameDisplayer : SkippableAnimation
    {
        [SerializeField] private Image i_frame;
        float f_previousTarget = 0f;

        public void UpdateFrame(Sprite _newFrame)
        {
            i_frame.sprite = _newFrame;
        }

        public IEnumerator AnimateFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            i_frame.transform.localScale = new Vector3(i_frame.transform.localScale.x, _to, i_frame.transform.localScale.z);
            f_previousTarget = _to;
            return ScaleFrame(_from, _to, _speed, _duration);
        }

        public IEnumerator ScaleFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            float start = Time.time;
            Vector3 _startScale = new Vector3(i_frame.transform.localScale.x, _from, i_frame.transform.localScale.z);

            while (_from < _to ? _startScale.y < _to : _startScale.y > _to)
            {
                float size = (Time.time - start) * _speed;
                float proportionalSize = size / _duration;

                i_frame.transform.localScale = new Vector3(_startScale.x, proportionalSize, _startScale.y);
                yield return null;
            }
        }

        public override void SkipToTheEnd()
        {
            i_frame.transform.localScale = Vector3.one;
        }

        public override void AssignEndState()
        {
            throw new System.NotImplementedException();
        }
    }
}