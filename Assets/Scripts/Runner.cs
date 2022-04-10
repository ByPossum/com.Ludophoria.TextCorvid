using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextCorvid;
using UnityEngine.UI;

public class Runner : MonoBehaviour
{
    [SerializeField] Text textRef;
    [SerializeField] Text langRef;
    [SerializeField] RectTransform rectToDrawTo;
    [SerializeField] TextDisplayer td;
    private Text t_textToDisplay;
    private string s_dialogue = "";
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TextManager>().Init();
        StartCoroutine(GetTextDelayed());
        t_textToDisplay = rectToDrawTo.gameObject.GetComponent<Text>();
        s_dialogue = t_textToDisplay?.text;
        td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TextManager.x.l_currentLanguage = Languages.eng;
            td.DisplayText(TextManager.x.GetText(s_dialogue), rectToDrawTo, TextDisplayType.character);
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

    private IEnumerator GetTextDelayed()
    {
        yield return new WaitForSeconds(0.0f);
        Debug.Log(TextManager.x.GetText(s_dialogue));
    }
}
