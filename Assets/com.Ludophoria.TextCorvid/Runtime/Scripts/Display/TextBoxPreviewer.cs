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
            tg.Init();
            foreach(TextBox box in FindObjectsOfType<TextBox>())
            {
                box.PreviewTextBox(tg.GetTextManager(), tg.GetTextAnimator());
            }
            //b_run = false;
        }
    }
}
