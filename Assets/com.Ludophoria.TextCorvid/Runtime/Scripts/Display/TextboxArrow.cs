using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxArrow : MonoBehaviour
{
    private RectTransform parent;
    private Vector2 targetPos;
    private Camera cam;
    [SerializeField] private GameObject target;
    [SerializeField] private float f_arrowDistance;

    public void CreateArrow(RectTransform _parent)
    {
        parent = _parent;
        cam = Camera.main;
    }

    public void Update()
    {
        if (parent && target)
        {
            targetPos = cam.WorldToScreenPoint(target.transform.position);
            transform.position = GetNewPos(targetPos, parent.position, parent.rect.width, parent.rect.height);
            transform.up = -(targetPos - (Vector2)transform.position);
        }
    }

    private Vector2 GetNewPos(Vector2 targetPosition, Vector2 parentPosition, float width, float height)
    {
        Vector2 direction = (targetPosition - parentPosition).normalized;
        return new Vector2(parentPosition.x + ((width * f_arrowDistance) * direction.x), parentPosition.y + ((height * f_arrowDistance) * direction.y));
    }
}
