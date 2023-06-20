using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public abstract class TextBox : MonoBehaviour
    {
        [SerializeField] protected TextDisplayer td_display;
        protected string s_textID;
        [SerializeField] protected CorvidAnimationState cas_currentState;
        public virtual bool Animating { get { return cas_currentState == CorvidAnimationState.animating; } }
        public CorvidAnimationState GetAnimationState { get { return cas_currentState; } }
        public void Init(int _textSpeed, TextAnimator _animator = null, string _initialID = null, string _initialText = null)
        {
            s_textID = GetComponentInChildren<TMPro.TMP_Text>().text;
            td_display.Init(_textSpeed, _animator, _initialID);
            if (_initialText != null)
                td_display.CacheText(_initialText);
        }

        public abstract IEnumerator Interact();
        public abstract bool CheckIfEnded();
        public abstract void ResetAnimations();
    }
}
