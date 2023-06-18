using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TextCorvid
{
    public class CharacterTextBox : TextBox
    {
        [SerializeField] private float f_frameSize, f_speed;
        [SerializeField] protected CharacterDisplayer cd_characterImage;
        [SerializeField] protected FrameDisplayer fd_frames;
        [SerializeField] protected SkippableAnimation[] A_objectsToAnimate;
        private int i_currentAnimatingObject = 0;
        private IEnumerator t_currentTask;
        private bool b_shouldEndAnim = true;
        public CharacterDisplayer GetCharacterDisplayer { get { return cd_characterImage; } }
        public FrameDisplayer GetFrameDisplayer { get { return fd_frames; } }

        private void OnEnable()
        {
            cas_currentState = CorvidAnimationState.idle;
        }

        private void OnDisable()
        {
            cas_currentState = CorvidAnimationState.closed;
        }

        private void Update()
        {

            if (cas_currentState == CorvidAnimationState.animating)
            {
                b_shouldEndAnim = true;
                foreach (SkippableAnimation _anim in A_objectsToAnimate)
                {
                    if (!_anim.GetAnimationEnd)
                        b_shouldEndAnim = false;
                }
                if(b_shouldEndAnim)
                    cas_currentState = CorvidAnimationState.animationEnd;
            }
                    
        }

        public void DisplayText(TextDisplayType _typeToDisplay)
        {
             td_display.AssignEndState();
             td_display.DisplayText(0f, _typeToDisplay);
        }

        public IEnumerator ToggleTextBox(bool _opening)
        {

            t_currentTask = fd_frames.AnimateFrame(_opening ? 0 : f_frameSize, _opening ? f_frameSize : 0f, f_speed);
            yield return StartCoroutine(t_currentTask);
            ToggleText();
        }

        private IEnumerator CheckForNewCharacter()
        {
            yield return null;
            //if(cd_characterImage.CheckNewCharacterTalking())
        }

        private void ToggleText()
        {
            td_display.AssignEndState();
            DisplayText(TextDisplayType.character);
        }

        public void CloseTextBox()
        {
            t_currentTask = fd_frames.AnimateFrame(f_frameSize, 0, f_speed);
            StartCoroutine(t_currentTask);
        }
        public void ChangeCharacter(string _textID)
        {
            cd_characterImage.UpdateCharacterImage(_textID);
        }

        public override IEnumerator Interact()
        {
            switch (cas_currentState)
            {
                case CorvidAnimationState.idle:
                    cas_currentState = CorvidAnimationState.animating;
                    if (!CheckTextBoxDone())
                        Animate();
                    break;
                case CorvidAnimationState.animating:
                    Interrupt();
                    break;
                case CorvidAnimationState.animationEnd:
                    i_currentAnimatingObject++;
                    cas_currentState = CorvidAnimationState.idle;
                    StartCoroutine(Interact());
                    break;
                case CorvidAnimationState.closed:
                    ToggleTextBox(false);
                    break;
                default:
                    break;
            }
            return t_currentTask;
        }

        private void Animate()
        {
            // Find the next animatable and animate it
            switch (A_objectsToAnimate[i_currentAnimatingObject])
            {
                case FrameDisplayer frame:
                    frame.AssignEndState();
                    StartCoroutine(ToggleTextBox(true));
                    break;
                case TextDisplayer dialogue:
                    DisplayText(TextDisplayType.character);
                    break;
            }
        }

        private void Interrupt()
        {
            if (fd_frames.GetAnimating)
                fd_frames.SkipToTheEnd();
            else if (td_display.GetAnimating)
                td_display.SkipToTheEnd();
        }

        private bool CheckTextBoxDone()
        {
            if (i_currentAnimatingObject >= A_objectsToAnimate.Length - 1)
            {
                ToggleTextBox(false);
                return true;
            }
            return false;
        }
    }
}