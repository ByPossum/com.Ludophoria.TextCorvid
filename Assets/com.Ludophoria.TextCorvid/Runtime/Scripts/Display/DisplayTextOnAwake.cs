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
            await tb_textBox.Init();
            for (int i = 0; i < 4000; i++)
                await System.Threading.Tasks.Task.Yield();
            tb_textBox.Display();
        }
    }
}
