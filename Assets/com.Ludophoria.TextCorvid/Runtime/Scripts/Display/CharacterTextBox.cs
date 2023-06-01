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
        private TextGlue tg;
        private int i_currentAnimatingObject = 0;
        private IEnumerator t_currentTask;
        public CharacterDisplayer GetCharacterDisplayer { get { return cd_characterImage; } }
        public FrameDisplayer GetFrameDisplayer { get { return fd_frames; } }

        private void OnEnable()
        {
            tg = FindObjectOfType<TextGlue>();
            cas_currentState = CorvidAnimationState.idle;
            Interact();
        }

        private void OnDisable()
        {
            cas_currentState = CorvidAnimationState.closed;
        }

        private void Update()
        {
            if(cas_currentState == CorvidAnimationState.animationEnd)
            {
                i_currentAnimatingObject++;
                cas_currentState = CorvidAnimationState.idle;
            }
        }

        public void DisplayText(string _textToShow, TextDisplayType _typeToDisplay)
        {
             td_display.DisplayText(_textToShow, 0f, _typeToDisplay);
        }

        public void OpenTextBox()
        {
            t_currentTask = fd_frames.AnimateFrame(0, f_frameSize, f_speed);
            StartCoroutine(t_currentTask);
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

        public void Interact()
        {
            switch (cas_currentState)
            {
                case CorvidAnimationState.idle:
                    CheckTextBoxDone();
                    break;
                case CorvidAnimationState.animating:
                    Interrupt();
                    break;
                default:
                    break;
            }
        }

        private void Animate()
        {
            // Find the next animatable and animate it
            switch (A_objectsToAnimate[i_currentAnimatingObject])
            {
                case FrameDisplayer frame:
                    OpenTextBox();
                    break;
                case TextDisplayer dialogue:
                    DisplayText(tg.GetTextManager().GetText(s_textID), TextDisplayType.character);
                    break;
            }
        }

        private void Interrupt()
        {
            if (fd_frames.GetAnimating)
                fd_frames.SkipToTheEnd();
            else if (td_display.GetAnimating)
                td_display.SkipToTheEnd();
            i_currentAnimatingObject = A_objectsToAnimate.Length;

        }

        private void CheckTextBoxDone()
        {
            if (i_currentAnimatingObject >= A_objectsToAnimate.Length - 1)
            {
                CloseTextBox();
                return;
            }

            cas_currentState = CorvidAnimationState.animating;
            Animate();
        }
    }
}