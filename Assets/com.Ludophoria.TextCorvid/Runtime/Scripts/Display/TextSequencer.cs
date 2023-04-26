using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class TextSequencer : MonoBehaviour, IInputSignal
    {
        [SerializeField] TextDisplayType td_wayToDisplayText;
        [SerializeField] private DialogueData[] dA_sequencedText;
        [SerializeField] private CharacterDisplayer cd_characterImage;
        [SerializeField] private FrameDisplayer fd_frames;
        [SerializeField] private TextDisplayer td_display;
        private int i_currentDialogue = -1;
        private TextGlue tg;

        void Start()
        {
            tg = FindObjectOfType<TextGlue>();
            td_display.Init(tg.GetTextManager().TextSpeed, tg.GetTextAnimator());
        }

        public void FireInput()
        {
            StopAllCoroutines();
            StartCoroutine(SequenceText());
        }

        private IEnumerator SequenceText()
        {
            if (i_currentDialogue >= 0)
                dA_sequencedText[i_currentDialogue].ueA_events.Invoke();
            DialogueData _nextDialogue = AdvanceDialogue();
            if (cd_characterImage.CheckNewCharacterTalking(_nextDialogue.s_dialogueID))
            {
                fd_frames.AnimateFrame(0, 1, 1, 1);

            }
            td_display?.DisplayText(tg.GetTextManager().GetText(_nextDialogue.s_dialogueID), 0f, td_wayToDisplayText);
            cd_characterImage.UpdateCharacterImage(_nextDialogue.s_dialogueID);
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