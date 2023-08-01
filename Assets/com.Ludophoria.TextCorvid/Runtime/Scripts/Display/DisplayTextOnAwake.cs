using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextCorvid
{
    public class DisplayTextOnAwake : MonoBehaviour
    {
        [SerializeField] private TextBox tb_textBox;
        public async void Start()
        {
            //tb_textBox = GetComponent<TextBox>();
            await tb_textBox.Init();
            tb_textBox.Display();
        }
    }
}
