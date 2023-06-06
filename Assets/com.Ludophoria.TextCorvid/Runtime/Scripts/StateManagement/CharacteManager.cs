using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class CharacteManager : MonoBehaviour
    {
        [SerializeField] private string[] A_characterName = new string[0];
        [SerializeField] private Sprite[] A_characterImages = new Sprite[0];
        [SerializeField] private Sprite[] A_frames = new Sprite[0];
        private string s_currentCharacterTalking;
        private int i_currentTalkingIndex;
        public string GetTalkingCharacter { get { return s_currentCharacterTalking; } }
    
        private int i_previousSize;
    
        private void OnValidate()
        {
            if (A_characterName.Length == A_characterImages.Length)
                return;
    
            if (A_characterImages.Length < i_previousSize)
                A_characterName = Utility.ResizeArray(A_characterName, A_characterImages.Length);
    
            if (A_characterName.Length < i_previousSize)
                A_characterImages = Utility.ResizeArray(A_characterImages, A_characterName.Length);
    
            if (A_characterImages.Length > A_characterName.Length)
                A_characterName = Utility.ResizeArray(A_characterName, A_characterImages.Length);
    
            if (A_characterName.Length > A_characterImages.Length)
                A_characterImages = Utility.ResizeArray(A_characterImages, A_characterName.Length);
    
            i_previousSize = A_characterImages.Length;
            A_frames = Utility.ResizeArray(A_frames, i_previousSize);
        }
    
        public Sprite GetSpriteOfCurrentTalkingCharacter(string _name)
        {
            int newCharacter = GetTalkingCharacterIndex(_name);

            // Return character if found
            if (newCharacter >= 0)
            {
                // Update current talking character
                i_currentTalkingIndex = newCharacter;
                // Return Character
                s_currentCharacterTalking = A_characterName[newCharacter];
                return A_characterImages[newCharacter];
            }

            // Return null if not found
            Debug.LogError("Character Name Not Found");
            s_currentCharacterTalking = string.Empty;
            return null;
        }

        private int GetTalkingCharacterIndex(string _name)
        {
            for (int i = 0; i < A_characterName.Length; i++)
                if (_name != string.Empty && _name != null)
                    if (_name.Contains(A_characterName[i]))
                    {
                        return i;
                    }
            return -1;
        }

        public Sprite GetFrameOfCurrentTalkingCharacter(string _name)
        {
            return A_frames[i_currentTalkingIndex];
        }

        public bool CheckNewCharacterTalking(string _name)
        {
            int newCharacter = GetTalkingCharacterIndex(_name);
            return i_currentTalkingIndex == newCharacter;
        }
    }
}