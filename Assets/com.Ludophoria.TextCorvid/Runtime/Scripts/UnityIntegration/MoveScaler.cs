using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoveScaler : MonoBehaviour
{
    [SerializeField] private Animator a_animationController;
    [SerializeField] private float f_duration;
    [SerializeField] private GameObject go_startPoint;
    [SerializeField] private GameObject go_endPoint;

    private async void Start()
    {
        if(a_animationController)
            a_animationController?.SetBool("active", true);
        await Move(go_startPoint, go_endPoint);
    }

    private async Task Move(GameObject _start, GameObject _end)
    {
        Vector3 _startPoint = Camera.main.WorldToScreenPoint(_start.transform.position);
        Vector3 _endPoint = Camera.main.WorldToScreenPoint(_end.transform.position);
        for(float i = 0; i < 1; i += Time.deltaTime * (1 / f_duration))
        {
            transform.position = Vector3.Lerp(_startPoint, _endPoint, i);
            await Task.Yield();
        }
    }
}
