using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{

    public class TextBoxPreviewer : MonoBehaviour
    {
        [SerializeField] TextGlue tg;
        [SerializeField] private bool b_run;
        public async void OnValidate()
        {
            tg.Init();
            foreach(TextBox box in FindObjectsOfType<TextBox>())
            {
                await box.Init();
                box.Display();
            }
            b_run = false;
        }
    }
}
