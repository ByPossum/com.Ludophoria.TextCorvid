using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class DisplayTextOnAwake : MonoBehaviour
    {

        TextGlue tg;
        TextDisplayer td_displayer;
        public async void Awake()
        {
            // This is a really horrid way of trying to avoid race conditions but I'm not above programming like a dingus so deal with it.
            while(tg == null)
            {
                tg = FindObjectOfType<TextGlue>();
                await System.Threading.Tasks.Task.Yield();
            }

            td_displayer = GetComponentInChildren<TextDisplayer>(true);
            td_displayer.Init(tg.GetTextManager().GetSettings(), tg.GetTextAnimator());
            // NB: Change text display type to some variable?
            ResizableTextBox tb = GetComponentInChildren<ResizableTextBox>();
            if (tb)
                td_displayer.DisplayText(tg.GetTextManager().GetText(td_displayer.GetComponentInChildren<TMPro.TMP_Text>(true).text), tb, TextDisplayType.character);
            else
                td_displayer.DisplayText(tg.GetTextManager().GetText(td_displayer.GetComponentInChildren<TMPro.TMP_Text>(true).text), GetComponent<RectTransform>().rect.height, TextDisplayType.character);
        }
    }
}
