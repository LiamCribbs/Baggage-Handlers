using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float Difficulty { get; private set; }

    void Awake()
    {
        instance = this;
    }
}