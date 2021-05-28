using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextCorvid;

public class Runner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TextManager>().Init();
        StartCoroutine(GetTextDelayed());
    }

    private IEnumerator GetTextDelayed()
    {
        yield return new WaitForSeconds(0.0f);
        Debug.Log(TextManager.x.GetText("tut01"));
    }
}
