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

        void Update()
        {
            if (ctb_characterTextBox.CheckIfEnded())
            {
                ctb_characterTextBox.ResetAnimations();
            }
        }

        public void FireInput()
        {
            if (!ctb_characterTextBox.gameObject.activeInHierarchy && cas_currentState != CorvidAnimationState.closed)
                ctb_characterTextBox.gameObject.SetActive(true);

            switch (cas_currentState)
            {
                case CorvidAnimationState.idle:
                    QueueNextData();
                    StartCoroutine(SequenceText());
                    break;
                case CorvidAnimationState.animating:
                    // Display the next data if the boxes aren't animating
                    if(!(ctb_characterTextBox as CharacterTextBox).Animating)
                    {
                        CharacterDialogue(SequenceDialogueData());
                        QueueNextData();
                    }
                    // Run the text
                    if (ie_currentEvent != null)
                        StopCoroutine(ie_currentEvent);
                    StartCoroutine(SequenceText());
                    break;
                case CorvidAnimationState.animationEnd:
                    // Close the text box here.
                    ctb_characterTextBox.gameObject.SetActive(false);
                    cas_currentState = CorvidAnimationState.closed;
                    break;
                case CorvidAnimationState.closed:
                    gameObject.SetActive(false);
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

        private void QueueNextData()
        {
            ChangeFrameAndPortrait((CharacterTextBox)ctb_characterTextBox);
            TransitionToAnimating();
        }

        private IEnumerator SequenceText()
        {
            ie_currentEvent = ctb_characterTextBox.Interact();
            yield return ie_currentEvent;
        }

        private DialogueData SequenceDialogueData()
        {
            if (i_currentDialogue >= 0)
                dA_sequencedText[i_currentDialogue].ueA_events.Invoke();
            
            return AdvanceDialogue();
        }

        private void CharacterDialogue(DialogueData _nextDialogue)
        {
            CharacterTextBox ctb = (CharacterTextBox)ctb_characterTextBox;
            ctb.UpdateTextForDisplay(dA_sequencedText[i_currentDialogue].s_dialogueID);
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
                i_currentDialogue = dA_sequencedText.Length-1;
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