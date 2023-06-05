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

        public void UpdateFrame(Sprite _newFrame)
        {
            i_frame.sprite = _newFrame;
        }

        public IEnumerator AnimateFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            cas_currentState = CorvidAnimationState.animating;
            i_frame.transform.localScale = new Vector3(i_frame.transform.localScale.x, _from, i_frame.transform.localScale.z);
            return ScaleFrame(_from, _to, _speed, _duration);
        }

        public IEnumerator ScaleFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            float start = Time.time;
            Vector3 _startScale = new Vector3(i_frame.transform.localScale.x, _from, i_frame.transform.localScale.z);

            while (_from < _to ? i_frame.transform.localScale.y < _to : i_frame.transform.localScale.y > _to)
            {
                float size = (Time.time - start) * _speed;
                float proportionalSize = size / _duration;

                i_frame.transform.localScale = new Vector3(_startScale.x, proportionalSize, _startScale.y);
                yield return null;
            }
            cas_currentState = CorvidAnimationState.animationEnd;
        }

        public override void SkipToTheEnd()
        {
            StopAllCoroutines();
            i_frame.transform.localScale = Vector3.one;
            cas_currentState = CorvidAnimationState.idle;
        }

        public override void AssignEndState()
        {
            
        }
    }
}