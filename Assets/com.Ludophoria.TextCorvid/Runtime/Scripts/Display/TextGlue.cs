using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class TextGlue : MonoBehaviour
    {
        private TextManager tm_manager;
        private TextAnimator ta_animator;
        private CharacteManager cm_characters;
        [SerializeField] private TextSettings ts_settings;
        void Awake()
        {
            // Create necassary components
            tm_manager = new TextManager(ts_settings ? ts_settings : new TextSettings());
            ta_animator = FindObjectOfType<TextAnimator>() ?? new TextAnimator();
            cm_characters = FindObjectOfType<CharacteManager>() ?? new CharacteManager();
        }

        public TextManager GetTextManager()
        {
            return tm_manager;
        }

        public TextAnimator GetTextAnimator()
        {
            return ta_animator;
        }
    }
}
