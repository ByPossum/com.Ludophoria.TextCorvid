using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TextCorvid
{
    [System.Serializable]
    public struct DialogueData
    {
        public string s_dialogueID;
        public UnityEvent[] ueA_events;
    }
}
