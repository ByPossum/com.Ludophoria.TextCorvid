using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public abstract class TextBox : MonoBehaviour
    {
        [SerializeField] protected TextDisplayer td_display;
        protected string s_textID;
        [SerializeField]protected CorvidAnimationState cas_currentState = CorvidAnimationState.closed;
        public bool Animating { get { return cas_currentState == CorvidAnimationState.animating; } }
        public void Init(int _textSpeed, TextAnimator _animator = null, string _initialID = null)
        {
            s_textID = GetComponentInChildren<TMPro.TMP_Text>().text;
            td_display.Init(_textSpeed, _animator, _initialID);
        }

        public abstract IEnumerator Interact();
    }
}
