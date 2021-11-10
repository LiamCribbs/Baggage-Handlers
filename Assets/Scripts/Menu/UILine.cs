using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILine : MonoBehaviour
{
    public const float Height = 4f;

    public static Color color;

    public new RectTransform transform;
    public UnityEngine.UI.Graphic graphic;

    void Awake()
    {
        color = graphic.color;
    }
}