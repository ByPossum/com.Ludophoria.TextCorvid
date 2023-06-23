using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextCorvid
{

    public class StateDebugger : MonoBehaviour
    {
        [SerializeField] TMP_Text t_sequencer;
        [SerializeField] TMP_Text t_box;
        [SerializeField] TMP_Text t_frame;
        [SerializeField] TMP_Text t_text;
        [SerializeField] TextSequencer ts_sequencer;
        [SerializeField] CharacterTextBox ctb_box;
        [SerializeField] FrameDisplayer fd_displayer;
        [SerializeField] TextDisplayer td_displayer;

        public void Update()
        {
            t_sequencer.text = ts_sequencer.GetAnimationState.ToString();
            t_box.text = ctb_box.GetAnimationState.ToString();
            t_frame.text = fd_displayer.GetAnimationState.ToString();
            t_text.text = td_displayer.GetAnimationState.ToString();
        }

    }
}
