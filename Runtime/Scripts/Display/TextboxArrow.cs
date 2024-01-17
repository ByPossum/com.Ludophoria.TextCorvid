using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxArrow : MonoBehaviour
{
    private RectTransform parent;
    private Vector2 targetPos;
    private Camera cam;
    private RectTransform rt;
    [SerializeField] private GameObject target;
    [SerializeField] private float f_arrowDistance;

    public void CreateArrow(RectTransform _parent)
    {
        parent = _parent;
        cam = Camera.main;
        rt = GetComponent<RectTransform>();
    }

    public void LateUpdate()
    {
        if (parent && target)
        {
            targetPos = cam.WorldToScreenPoint(target.transform.position);
            rt.anchoredPosition = GetNewPos(targetPos, parent.position, parent.rect.width, parent.rect.height);
            rt.transform.up = -(targetPos - (Vector2)transform.position);
        }
    }

    private Vector2 GetNewPos(Vector2 targetPosition, Vector2 parentPosition, float width, float height)
    {
        Vector2 direction = (targetPosition - parentPosition).normalized;
        return new Vector2((width*0.5f * direction.x), ((height*0.5f * direction.y)));
    }
}
