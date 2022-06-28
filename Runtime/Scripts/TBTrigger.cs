using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TextCorvid
{
    public class TBTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("Who can activate this TextBox?")]
        private GameObject go_target;
        [SerializeField, Tooltip("Do you want this textbox turned off once you leave the trigger?")]
        private bool b_turnOffOnExit;
        private GameObject go_textBox;
        private void TurnOnTextBox(GameObject _objectToCheck)
        {
            if (_objectToCheck == go_target)
            {
                go_textBox.SetActive(true);
            }
        }
        private void TurnOffTextBox(GameObject _objectToCheck)
        {
            if (b_turnOffOnExit && _objectToCheck == go_target)
                go_textBox.SetActive(false);
        }
    
        private void Start()
        {
            go_textBox = GetComponentInChildren<RectTransform>(true).gameObject;
        }
    
        #region 2D Colliders
        private void OnCollisionEnter2D(Collision2D collision)
        {
            TurnOnTextBox(collision.gameObject);
        }
    
        private void OnCollisionExit2D(Collision2D collision)
        {
            TurnOffTextBox(collision.gameObject);
        }
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            TurnOnTextBox(collision.gameObject);
        }
    
        private void OnTriggerExit2D(Collider2D collision)
        {
            TurnOffTextBox(collision.gameObject);
        }
        #endregion
        #region 3D Colliders
        private void OnTriggerEnter(Collider other)
        {
            TurnOnTextBox(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            TurnOffTextBox(other.gameObject);
        }
        private void OnCollisionEnter(Collision collision)
        {
            TurnOnTextBox(collision.gameObject);
        }
        private void OnCollisionExit(Collision collision)
        {
            TurnOffTextBox(collision.gameObject);
        }
        #endregion
    }
}