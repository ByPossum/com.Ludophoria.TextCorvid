using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{

    public class TextBoxPreviewer : MonoBehaviour
    {
        [SerializeField] TextGlue tg;
        [SerializeField] private bool b_run;
        public void OnValidate()
        {
            try
            {
                tg.Init();
            }
            catch(System.NullReferenceException e)
            {
                Debug.LogError("Text Manager unable to initialize. Check your text settings");
            }
            foreach(TextBox box in FindObjectsOfType<TextBox>())
            {
                box.PreviewTextBox(tg.GetTextManager(), tg.GetTextAnimator());
            }
            foreach (DisplayUIOnAwake ui in FindObjectsOfType<DisplayUIOnAwake>())
                ui.DisplayText();
            //b_run = false;
        }
    }
}
