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
        private Vector3 v_endState;

        public void UpdateFrame(Sprite _newFrame)
        {
            i_frame.sprite = _newFrame;
        }

        public IEnumerator AnimateFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            v_endState = new Vector3(1f, _to, 1f);
            cas_currentState = CorvidAnimationState.animating;
            i_frame.transform.localScale = new Vector3(i_frame.transform.localScale.x, _from, i_frame.transform.localScale.z);
            return ScaleFrame(_from, _to, _speed, _duration);
        }

        public IEnumerator ScaleFrame(float _from, float _to, float _speed, float _duration = 1f)
        {
            float start = Time.time;
            Vector3 _startScale = new Vector3(i_frame.transform.localScale.x, _from, i_frame.transform.localScale.z);
            float _startSize = _from < _to ? _from : _to;
            float _endSize = _from < _to ? _to : _from;

            while (_from < _to ? i_frame.transform.localScale.y < _endSize : i_frame.transform.localScale.y >= _endSize)
            {
                float size = (Time.time - start) * _speed;
                float proportionalSize = size / _duration;
                float ySize = _from < _to ? proportionalSize * _to : _from - (proportionalSize * _from);
                Debug.Log($"{ySize}");
                i_frame.transform.localScale = new Vector3(_startScale.x, ySize, _startScale.y);
                yield return null;
            }

            i_frame.transform.localScale = v_endState;
            cas_currentState = i_frame.transform.localScale.y > 0.9f ? CorvidAnimationState.animationEnd : CorvidAnimationState.closed;
        }

        public override void SkipToTheEnd()
        {
            StopAllCoroutines();
            i_frame.transform.localScale = v_endState;
            cas_currentState = CorvidAnimationState.animationEnd;
        }

        public override void AssignEndState()
        {
            
        }

        public override void ResetAnimation()
        {
            cas_currentState = CorvidAnimationState.idle;
        }
    }
}