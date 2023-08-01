using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TextCorvid
{
    public class TextBox : MonoBehaviour
    {
        private TMPro.TMP_Text tm_textRext;
        [SerializeField] private TextDisplayer td_text;
        [SerializeField] private Frame f_frame;
        [SerializeField] private Mover[] mA_movers;
        
        [SerializeField] private TextDisplayType td_wayToShowText;
        
        private TextGlue tg_glue;

        private string s_textID;
        private string s_textWithAnims;
        private string s_textToDisplay;

        public async Task Init()
        {
            tg_glue = FindObjectOfType<TextGlue>();
            tm_textRext = GetComponentInChildren<TMPro.TMP_Text>();
            // Wtf. A bit brutal huh?
            if (!tm_textRext)
                Destroy(this);

            // Initialize pure text
            s_textID = td_text.TextID ?? tm_textRext.text;
            s_textWithAnims = tg_glue.GetTextManager().GetText(s_textID);
            s_textToDisplay = tg_glue.GetTextAnimator().ParseAnimations(tm_textRext, s_textWithAnims);

            // Initialize frame and text displayer
            f_frame.Init(s_textToDisplay, tm_textRext);
            td_text.Init(tg_glue.GetTextManager().TextSpeed);

            await Task.Yield();
        }

        public void Display()
        {
            DisplayFrame();
            DisplayText();
            RunAllMovers();
        }

        public void DisplayFrame()
        {
            f_frame.gameObject.SetActive(true);
        }

        public void DisplayText()
        {
            td_text.DisplayText(tg_glue.GetTextManager().GetText(td_text.TextID), 0.0f, td_wayToShowText);
        }

        public void RunAllMovers()
        {
            for (int i = 0; i < mA_movers.Length; i++)
                RunSpecificMover(i);
        }

        public void RunSpecificMover(int _moverID)
        {
            mA_movers[_moverID].Begin();
        }
    }
}
