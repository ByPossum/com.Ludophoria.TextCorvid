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
        private int i_currentDialogue = -1;
        private TextGlue tg;
        private IEnumerator ie_currentEvent;
        void Start()
        {
            tg = FindObjectOfType<TextGlue>();
            ctb_characterTextBox.Init(tg.GetTextManager().TextSpeed, tg.GetTextAnimator(), dA_sequencedText[0].s_dialogueID);
        }

        public void Update()
        {
            
        }

        public void FireInput()
        {
            if (!ctb_characterTextBox.gameObject.activeInHierarchy)
                ctb_characterTextBox.gameObject.SetActive(true);
            ie_currentEvent = SequenceText();
            if(ie_currentEvent != null)
            {
                StartCoroutine(ie_currentEvent);
            }
            else
            {
                // Interrupt here
            }
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
            if (disp.CheckNewCharacterTalking(_nextDialogue.s_dialogueID))
            {
                fd_frames.AnimateFrame(0, 1, 1, 1);
                while (fd_frames.GetAnimating)
                    yield return null;
            }
            ctb.Interact();
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

        public IEnumerator EnumeratorTest()
        {
            yield return new WaitForSeconds(10);
        }
    }
}