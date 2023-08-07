using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxArrow : MonoBehaviour
{
    
    public void CreateArrow(RectTransform parent, Vector2 target)
    {
        Vector2 parentPos = parent.transform.position;
        float x = GetPercentageOnBoarder(parentPos.x, parent.rect.width, target.x);
        float y = GetPercentageOnBoarder(parentPos.y, parent.rect.height, target.y);
        transform.position = new Vector2(x, y);

    }

    private float GetPercentageOnBoarder(float boxPos, float size, float targetPos)
    {
        float max = (boxPos + (size * 0.5f));
        float min = (boxPos - (size * 0.5f));
        float target = targetPos + (size * 0.5f);
        float percentage = ((max + min - target) / max);
        float result = Mathf.Clamp(max + (max * percentage), boxPos - (size * 0.5f), boxPos + (size * 0.5f));
        Debug.Log($"Max: {max} | Min: {min} | Max + Min: {max + min} | Target: {target} | Neumerator: {max + min - target} | %: {percentage} | result: {result}");
        return result;
    }
}
