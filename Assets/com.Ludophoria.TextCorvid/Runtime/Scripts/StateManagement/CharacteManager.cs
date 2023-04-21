using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class CharacteManager : MonoBehaviour
    {
        [SerializeField] private string[] A_characterName = new string[0];
        [SerializeField] private Sprite[] A_characterImages = new Sprite[0];
    
    
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
        }
    
        public Sprite GetCharacterSprite(string _name)
        {
            for (int i = 0; i < A_characterName.Length; i++)
                if (_name == A_characterName[i])
                    return A_characterImages[i];
            Debug.LogError("Character Name Not Found");
            return null;
        }
    }
}