using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TextCorvid
{

    public class MoveScaler : Mover
    {
        [SerializeField] private Animator a_animationController;
        [SerializeField] private float f_duration;
        [SerializeField] private GameObject go_startPoint;
        [SerializeField] private GameObject go_endPoint;
        [SerializeField] private Vector3 v_startPos;
        [SerializeField] private Vector3 v_endPos;

        public override async void Begin()
        {
            if (a_animationController)
                a_animationController?.SetBool("active", true);

            await Move(go_startPoint, go_endPoint);
        }

        public override GameObject GetEnd()
        {
            return go_endPoint;
        }

        public override GameObject GetStart()
        {
            return go_startPoint;
        }

        private async Task Move(GameObject _start, GameObject _end)
        {
            Vector3 _startPoint = _start ? Camera.main.WorldToScreenPoint(_start.transform.position) : transform.parent.position;
            Vector3 _endPoint = _end ? Camera.main.WorldToScreenPoint(_end.transform.position) : transform.parent.position;
            for (float i = 0; i < 1; i += Time.deltaTime * (1 / f_duration))
            {
                transform.parent.position = Vector3.Lerp(_startPoint, _endPoint, i);
                await Task.Yield();
            }
        }
    }

}