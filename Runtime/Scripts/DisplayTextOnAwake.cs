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
                await System.Threading.Tasks.Task.Delay(1);
            }

            td_displayer = GetComponentInChildren<TextDisplayer>(true);
            td_displayer.Init(tg.GetTextManager().GetSettings(), tg.GetTextAnimator());
            // NB: Change text display type to some variable?
            td_displayer.DisplayText(tg.GetTextManager().GetText(td_displayer.GetComponentInChildren<TMPro.TMP_Text>(true).text), GetComponentInChildren<ResizableTextBox>().GetComponent<RectTransform>(), TextDisplayType.character);
        }
    }
}
