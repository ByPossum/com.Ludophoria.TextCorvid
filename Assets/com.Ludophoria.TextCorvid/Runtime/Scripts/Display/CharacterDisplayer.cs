using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextCorvid
{
    public class CharacterDisplayer : MonoBehaviour
    {
        [SerializeField] private Image i_characterImage;
        [SerializeField] private CharacteManager cm_characters;
        public void UpdateCharacterImage(string _id)
        {
            i_characterImage.sprite = cm_characters.GetSpriteOfCurrentTalkingCharacter(_id);
        }

        public bool CheckNewCharacterTalking(string _id)
        {
            return cm_characters.CheckNewCharacterTalking(_id);
        }
    }
}
