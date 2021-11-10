using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILineAnchor : MonoBehaviour
{
    [System.NonSerialized] public new RectTransform transform;

    void Awake()
    {
        transform = (RectTransform)base.transform;
    }

    void OnEnable()
    {
        MouseToAnchorDrawer.Instance.AddAnchor(this);
    }

    void OnDisable()
    {
        MouseToAnchorDrawer.Instance.RemoveAnchor(this);
    }

    public Vector2 ClosestPoint(Vector2 point, float Stickiness)
    {
        Rect rect = transform.rect;
        Vector2 position = transform.localPosition;
        Vector2 min = rect.min + position;
        Vector2 max = rect.max + position;

        if (point.x < min.x)
        {
            point.x = min.x;
        }
        else if (point.x > max.x)
        {
            point.x = max.x;
        }
        //else
        //{
        //    float segmentPercentage = MouseToAnchorDrawer.Instance.stickinessCurve.Evaluate(Mathf.InverseLerp(min.x, max.x, point.x));
        //    point.x = Mathf.Lerp(point.x, position.x, segmentPercentage * Stickiness);
        //}

        if (point.y < min.y)
        {
            point.y = min.y;
        }
        else if (point.y > max.y)
        {
            point.y = max.y;
        }
        //else
        //{
        //    float segmentPercentage = MouseToAnchorDrawer.Instance.stickinessCurve.Evaluate(Mathf.InverseLerp(min.x, max.x, point.x));
        //    point.y = Mathf.Lerp(point.y, position.y, segmentPercentage * Stickiness);
        //}

        return point;
    }

    // Find intersection point
    float Intersection(in Vector2 point, in Vector2 direction, in Vector2 segment0, in Vector2 segment1)
    {
        return ((direction.x * point.x + direction.y *
        (segment0.x - point.x)) - (direction.x * segment1.y)) /
        (direction.y * (segment0.x + segment1.x) -
        direction.x * (segment0.y + segment1.y));
    }
}