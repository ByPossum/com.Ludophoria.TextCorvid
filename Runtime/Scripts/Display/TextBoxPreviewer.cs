#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TextCorvid
{

    public class TextBoxPreviewer : MonoBehaviour
    {
        [SerializeField] TextGlue tg;
        [SerializeField] private bool b_run;
        public void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            try
            {
                tg.Init();
            }
            catch(NullReferenceException e)
            {
                return;
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
#endif