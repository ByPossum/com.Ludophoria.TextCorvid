using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextCorvid
{
    public class DisplayUIOnAwake : MonoBehaviour
    {
        [SerializeField] private TextDisplayer td_displayer;
        [SerializeField] private TextDisplayType td_wayToShowText;
        private void Start()
        {
            DisplayText();
        }

        public void DisplayText()
        {
            TextGlue tg = FindObjectOfType<TextGlue>();
            TMP_Text tm_text = td_displayer.GetComponent<TMP_Text>();
            td_displayer.Init(tg.GetTextManager().TextSpeed);
            td_displayer.DisplayText(tg.GetTextAnimator().ParseAnimations(tm_text, tg.GetTextManager().GetText(td_displayer.TextID)), 0.0f,
                td_wayToShowText);
        }
    }
}
