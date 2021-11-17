using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pigeon;
using Pigeon.Math;

public class MouseToAnchorDrawer : MonoBehaviour
{
    static MouseToAnchorDrawer instance;
    public static MouseToAnchorDrawer Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<MouseToAnchorDrawer>();
            }
            return instance;
        }
    }

    public RectTransform rectTransform;
    public Canvas canvas;

    public GameObject uiLinePrefab;

    public Vector2 lineDistanceFadeRange;
    public AnimationCurve lineDistanceFadeCurve;

    public List<UILineAnchor> anchors;
    public List<UILine> lines;

    [Range(0f, 1f)] public float stickiness;
    public AnimationCurve stickinessCurve;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 mousePosition);
        for (int i = 0; i < anchors.Count; i++)
        {
            UILine line = lines[i];
            UILineAnchor anchor = anchors[i];
            line.transform.localPosition = mousePosition;

            //Vector2 anchorPosition = anchor.ClosestPoint(mousePosition, stickiness);
            Vector2 anchorPosition = rectTransform.InverseTransformPoint(anchor.transform.position);
            Vector2 distance = anchorPosition - mousePosition;

            float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            line.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            float distanceMag = distance.MagFast();
            line.transform.sizeDelta = new Vector2(distanceMag, UILine.Height);

            UILine.color.a = lineDistanceFadeCurve.Evaluate(1f - Mathf.InverseLerp(lineDistanceFadeRange.x, lineDistanceFadeRange.y, distanceMag));
            line.graphic.color = UILine.color;
        }
    }

    public void AddAnchor(UILineAnchor anchor)
    {
        anchors.Add(anchor);
        lines.Add(Instantiate(uiLinePrefab, transform).GetComponent<UILine>());
    }

    public void RemoveAnchor(UILineAnchor anchor)
    {
        int i = anchors.IndexOf(anchor);
        anchors.Remove(anchor);
        Destroy(lines[i].gameObject);
        lines.RemoveAt(i);
    }
}