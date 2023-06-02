using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class TextSequencer : MonoBehaviour, IInputSignal
    {
        [SerializeField] TextDisplayType td_wayToDisplayText;
        [SerializeField] private DialogueData[] dA_sequencedText;
        [SerializeField] private TextBox ctb_characterTextBox;
        private CorvidAnimationState cas_currentState;
        private int i_currentDialogue = -1;
        private TextGlue tg;
        private IEnumerator ie_currentEvent;
        void Start()
        {
            tg = FindObjectOfType<TextGlue>();
            ctb_characterTextBox.Init(tg.GetTextManager().TextSpeed, tg.GetTextAnimator(), dA_sequencedText[0].s_dialogueID);
            cas_currentState = CorvidAnimationState.idle;
        }

        public void FireInput()
        {
            if (!ctb_characterTextBox.gameObject.activeInHierarchy)
                ctb_characterTextBox.gameObject.SetActive(true);
            switch (cas_currentState)
            {
                case CorvidAnimationState.idle:
                    TransitionToAnimating();
                    break;
                case CorvidAnimationState.animating:
                    break;
                case CorvidAnimationState.animationEnd:
                    break;
                case CorvidAnimationState.closed:
                    break;
                default:
                    break;
            }
        }

        private void TransitionToAnimating()
        {
            ie_currentEvent = SequenceText();
            cas_currentState = CorvidAnimationState.animating;
            StartCoroutine(ie_currentEvent);
        }

        private IEnumerator SequenceText()
        {
            if (i_currentDialogue >= 0)
                dA_sequencedText[i_currentDialogue].ueA_events.Invoke();
            DialogueData _nextDialogue = AdvanceDialogue();
            switch (ctb_characterTextBox)
            {
                case CharacterTextBox ctb:
                    yield return CharacterDialogue(_nextDialogue);
                    break;
                /// TODO: Implement Resizable Text Boxes and others
            }
            ie_currentEvent = null;
            yield return null;
        }

        private IEnumerator CharacterDialogue(DialogueData _nextDialogue)
        {
            CharacterTextBox ctb = (CharacterTextBox)ctb_characterTextBox;
            CharacterDisplayer disp = ctb.GetCharacterDisplayer;
            FrameDisplayer fd_frames = ctb.GetFrameDisplayer;
            disp.UpdateCharacterImage(_nextDialogue.s_dialogueID);
            ctb.Interact();
            yield return null;
        }

        public bool GetDone()
        {
            return false;
        }

        public void ToggleInput() {}

        private DialogueData AdvanceDialogue()
        {
            i_currentDialogue++;
            if (i_currentDialogue >= dA_sequencedText.Length)
                i_currentDialogue = dA_sequencedText.Length - 1;
            return dA_sequencedText[i_currentDialogue];
        }
    
        private DialogueData RegressDialogue()
        {
            i_currentDialogue--;
            if(i_currentDialogue <= 0)
                i_currentDialogue = 0;
            return dA_sequencedText[i_currentDialogue];
        }
    }
}