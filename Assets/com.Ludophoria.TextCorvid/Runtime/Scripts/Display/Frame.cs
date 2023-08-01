using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Frame : MonoBehaviour
{
    public abstract void Init(string _text, TMP_Text _container);
}
