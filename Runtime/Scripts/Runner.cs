using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextCorvid;
using UnityEngine.UI;
using TMPro;

public class Runner : MonoBehaviour
{
    [SerializeField] TMP_Text textRef;
    [SerializeField] TMP_Text langRef;
    [SerializeField] RectTransform rectToDrawTo;
    [SerializeField] TextDisplayer td;
    private TextManager tm_managerRef;
    private TMP_Text t_textToDisplay;
    private string s_dialogue = "";
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TextManager>().Init();
        t_textToDisplay = rectToDrawTo.gameObject.GetComponent<TMP_Text>();
        s_dialogue = t_textToDisplay?.text;
#if ALLOW_SINGLETON
        tm_managerRef = TextManager.x;
#else
        tm_managerRef = FindObjectOfType<TextManager>();
#endif
        td.Init(tm_managerRef.f_textSpeed);
        td.DisplayText(tm_managerRef.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
        tm_managerRef.l_currentLanguage = Languages.eng;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            td.DisplayText(tm_managerRef.GetText("tut01"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            td.DisplayText(tm_managerRef.GetText("test02"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            td.DisplayText(tm_managerRef.GetText("test03"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            td.DisplayText(tm_managerRef.GetText("test04"), rectToDrawTo, TextDisplayType.character);
        }
        langRef.text = tm_managerRef.l_currentLanguage.ToString();
    }
}
