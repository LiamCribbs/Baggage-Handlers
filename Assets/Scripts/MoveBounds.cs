using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBounds : MonoBehaviour
{
    public Vector2 size;
    public float outerPadding;

    public float Right => transform.localPosition.x + size.x;
    public float Left => transform.localPosition.x - size.x;
    public float Top => transform.localPosition.y + size.y;
    public float Bottom => transform.localPosition.y - size.y;

    public float OuterRight => transform.localPosition.x + size.x + outerPadding;
    public float OuterLeft => transform.localPosition.x - size.x - outerPadding;
    public float OuterTop => transform.localPosition.y + size.y + outerPadding;
    public float OuterBottom => transform.localPosition.y - size.y - outerPadding;

    public float RightWorld => transform.position.x + size.x;
    public float LeftWorld => transform.position.x - size.x;
    public float TopWorld => transform.position.y + size.y;
    public float BottomWorld => transform.position.y - size.y;

    public float OuterRightWorld => transform.position.x + size.x + outerPadding;
    public float OuterLeftWorld => transform.position.x - size.x - outerPadding;
    public float OuterTopWorld => transform.position.y + size.y + outerPadding;
    public float OuterBottomWorld => transform.position.y - size.y - outerPadding;

    public Vector3 ClampToBounds(Vector3 point)
    {
        point.x = point.x > Right ? Right : point.x < Left ? Left : point.x;
        point.y = point.y > Top ? Top : point.y < Bottom ? Bottom : point.y;
        return point;
    }

    public Vector3 ClampToOuterBounds(Vector3 point)
    {
        point.x = point.x > OuterRight ? OuterRight : point.x < OuterLeft ? OuterLeft : point.x;
        point.y = point.y > OuterTop ? OuterTop : point.y < OuterBottom ? OuterBottom : point.y;
        return point;
    }

#if UNITY_EDITOR
    public Color DEBUG_COLOR = new Color(0.54f, 0.2f, 1f);
    public Color DEBUG_COLOR_2 = new Color(0.4f, 0.12f, 0.8f);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = DEBUG_COLOR;
        Gizmos.DrawWireCube(transform.position, size * 2f);
        Gizmos.color = DEBUG_COLOR_2;
        Gizmos.DrawWireCube(transform.position, (size + new Vector2(outerPadding, outerPadding)) * 2f);
    }
#endif
}