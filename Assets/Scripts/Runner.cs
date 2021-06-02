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
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TextManager>().Init();
        StartCoroutine(GetTextDelayed());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TextManager.x.l_currentLanguage = Languages.eng;
            td.DisplayText(TextManager.x.GetText("tut01"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TextManager.x.l_currentLanguage = Languages.isl;
            td.DisplayText(TextManager.x.GetText("tut01"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TextManager.x.l_currentLanguage = Languages.eng;
            td.DisplayText(TextManager.x.GetText("tut02"), rectToDrawTo, TextDisplayType.character);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TextManager.x.l_currentLanguage = Languages.isl;
            td.DisplayText(TextManager.x.GetText("tut02"), rectToDrawTo, TextDisplayType.character);
        }
        langRef.text = TextManager.x.l_currentLanguage.ToString();
    }

    private IEnumerator GetTextDelayed()
    {
        yield return new WaitForSeconds(0.0f);
        Debug.Log(TextManager.x.GetText("tut01"));
    }
}
