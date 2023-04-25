using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextCorvid;
public class TestInputter : MonoBehaviour
{
    IInputSignal ts;
    private void Start()
    {
        ts = FindObjectOfType<TextSequencer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ts.FireInput();

    }
}
