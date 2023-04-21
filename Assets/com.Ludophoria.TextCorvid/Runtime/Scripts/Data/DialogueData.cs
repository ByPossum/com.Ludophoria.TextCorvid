using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogueData
{
    public string s_dialogueID;
    public Animator a_characterToAnimate;
    public string s_triggerName;
    public UnityEvent[] ueA_events;
}
