using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextCorvid;
using System.Threading.Tasks;

public class TestInputter : MonoBehaviour
{
    IInputSignal ts;
    private async void Start()
    {
        while (ts == null)
        {
            ts = FindObjectOfType<TextSequencer>();
            await Task.Yield();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ts.FireInput();
        }

    }
}
