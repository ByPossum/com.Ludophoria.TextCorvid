using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{

    public abstract class SkippableAnimation : MonoBehaviour
    {
        protected CorvidAnimationState cas_currentState;
        public bool GetAnimating { get { return cas_currentState == CorvidAnimationState.animating; } }
        public CorvidAnimationState GetAnimationState { get { return cas_currentState; } }

        public abstract void SkipToTheEnd();

        public abstract void AssignEndState();
    }
}
