using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class TextSequencer : MonoBehaviour, IInputSignal
    {
        [SerializeField] private DialogueData[] dA_sequencedText;
        [SerializeField] private CharacterDisplayer cd_characterImage;
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
            if(i_currentDialogue >= 0)
                dA_sequencedText[i_currentDialogue].ueA_events.Invoke();
            DialogueData _nextDialogue = AdvanceDialogue();
            td_display.DisplayText(tg.GetTextManager().GetText(_nextDialogue.s_dialogueID), 0f, TextDisplayType.block);
            cd_characterImage.UpdateCharacterImage(_nextDialogue.s_dialogueID);
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