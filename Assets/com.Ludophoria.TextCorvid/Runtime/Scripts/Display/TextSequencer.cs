using System;
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
        private int i_currentDialogue = 0;
        private TextGlue tg;
        private IEnumerator ie_currentEvent;
        public CorvidAnimationState GetAnimationState { get { return cas_currentState; } }
        void Start()
        {
            tg = FindObjectOfType<TextGlue>();
            ctb_characterTextBox.Init(tg.GetTextManager().TextSpeed, tg.GetTextAnimator(), null, tg.GetTextManager().GetText(dA_sequencedText[0].s_dialogueID));
            cas_currentState = CorvidAnimationState.idle;
        }

        public void FireInput()
        {
            if (!ctb_characterTextBox.gameObject.activeInHierarchy && cas_currentState != CorvidAnimationState.closed)
                ctb_characterTextBox.gameObject.SetActive(true);

            switch (cas_currentState)
            {
                case CorvidAnimationState.idle:
                    ChangeFrameAndPortrait((CharacterTextBox)ctb_characterTextBox);
                    TransitionToAnimating();
                    ie_currentEvent = ctb_characterTextBox.Interact();
                    break;
                case CorvidAnimationState.animating:
                    StartCoroutine(SequenceText());
                    break;
                case CorvidAnimationState.animationEnd:
                    ctb_characterTextBox.gameObject.SetActive(false);
                    break;
                case CorvidAnimationState.closed:
                    break;
                default:
                    break;
            }
        }

        private void ChangeFrameAndPortrait(CharacterTextBox _tb)
        {
            _tb.GetCharacterDisplayer.UpdateCharacterImage(dA_sequencedText[i_currentDialogue].s_dialogueID);
            //_tb.GetFrameDisplayer.UpdateFrame();
        }

        private void TransitionToAnimating()
        {
            cas_currentState = CorvidAnimationState.animating;
            //StartCoroutine(SequenceText());
        }

        private IEnumerator SequenceText()
        {
            if (ctb_characterTextBox.GetAnimationState == CorvidAnimationState.animationEnd)
            {
                ie_currentEvent = SequenceDialogueData();
                StartCoroutine(ie_currentEvent);
                yield return ie_currentEvent;

            }
            else
            {
                ie_currentEvent = ctb_characterTextBox.Interact();
                StartCoroutine(ie_currentEvent);
                yield return ie_currentEvent;
            }
            ie_currentEvent = null;
            yield return null;
        }

        private IEnumerator SequenceDialogueData()
        {
            if (i_currentDialogue >= 0)
                dA_sequencedText[i_currentDialogue].ueA_events.Invoke();
            
            DialogueData _nextDialogue = AdvanceDialogue();
            switch (ctb_characterTextBox)
            {
                case CharacterTextBox ctb:
                    yield return CharacterDialogue(_nextDialogue);
                    break;
                default:
                    break;
                    /// TODO: Implement Resizable Text Boxes and others
            }
            yield return null;
        }

        private IEnumerator CharacterDialogue(DialogueData _nextDialogue)
        {
            CharacterTextBox ctb = (CharacterTextBox)ctb_characterTextBox;
            CharacterDisplayer disp = ctb.GetCharacterDisplayer;
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