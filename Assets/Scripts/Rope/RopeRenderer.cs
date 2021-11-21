using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public VertletRope rope;

    protected virtual void Awake()
    {
        rope.Setup(rope.maxLength);
    }

    protected virtual void FixedUpdate()
    {
        rope.startPosition = start.position;
        rope.endPosition = end.position;
        rope.Simulate();
    }

    protected virtual void LateUpdate()
    {
        rope.DrawRope();
    }

    public void SetLength(float amount)
    {
        if (amount < 0f)
        {
            amount = 0f;
        }

        rope.maxLength = amount;
        rope.SetSegmentLength();
    }
}