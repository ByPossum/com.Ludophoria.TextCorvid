using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class TextSequencer : MonoBehaviour
    {
        [SerializeField] private DialogueData[] dA_sequencedText;
        [SerializeField] private CharacterDisplayer cd_characterImage;
        [SerializeField] private TextDisplayer td_display;
        private int i_currentDialogue;
    
        public DialogueData AdvanceDialogue()
        {
            i_currentDialogue++;
            if (i_currentDialogue > dA_sequencedText.Length)
                return new DialogueData();
            return dA_sequencedText[i_currentDialogue];
        }
    
        public DialogueData RegressDialogue()
        {
            i_currentDialogue--;
            if(i_currentDialogue <= 0)
                i_currentDialogue = 0;
            return dA_sequencedText[i_currentDialogue];
        }
    }
}