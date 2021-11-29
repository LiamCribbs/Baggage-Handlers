using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public static Weather instance;

    [SerializeField] Vector2 windDirection;
    [SerializeField] float windSpeed;

    public static Vector2 Wind => instance.windDirection * instance.windSpeed;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}