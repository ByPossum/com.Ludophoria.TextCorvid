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
    private TMP_Text t_textToDisplay;
    private string s_dialogue = "";
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TextManager>().Init();
        t_textToDisplay = rectToDrawTo.gameObject.GetComponent<TMP_Text>();
        s_dialogue = t_textToDisplay?.text;
        td.Init(TextManager.x.f_textSpeed);
        td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TextManager.x.l_currentLanguage = Languages.eng;
            td.DisplayText(TextManager.x.GetText("test01"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TextManager.x.l_currentLanguage = Languages.isl;
            td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TextManager.x.l_currentLanguage = Languages.eng;
            td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TextManager.x.l_currentLanguage = Languages.isl;
            td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
        }
        langRef.text = TextManager.x.l_currentLanguage.ToString();
    }
}
