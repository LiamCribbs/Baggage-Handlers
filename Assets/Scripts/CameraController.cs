using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public new Camera camera;

    public Transform player;
    public Vector2 playerBounds;
    public Transform luggagePlane;
    public Vector2 luggagePlaneBoundsHorizontal;
    public Vector2 luggagePlaneBoundVertical;

    float aspectRatio;

    public float minSize;

    public float sizeLerpSpeedSmaller;
    public float sizeLerpSpeedLarger;
    public float positionLerpSpeed;

    [Space(20)]
    public float positionShakeMultiplier;
    public float rotationShakeMultiplier;
    Vector3 rotationShakeValue;
    float shakeIntensity;
    public float ShakeSpeed = 0.53f;
    public float shakeDecay = 1f;
    public float maxShakeIntensity = 275f;

    void Awake()
    {
        instance = this;

        aspectRatio = (float)Screen.height / Screen.width;
    }

    public void Shake(float shake)
    {
        shakeIntensity += shake;
    }

    void UpdateShake()
    {
        shakeIntensity -= shakeDecay * Time.deltaTime;

        if (shakeIntensity < 0f)
        {
            shakeIntensity = 0f;
            ///rotationShakeValue.x = 0f;
            ///rotationShakeValue.y = 0f;
            ///rotationShakeValue.z = 0f;
        }

        if (shakeIntensity > maxShakeIntensity)
        {
            shakeIntensity = maxShakeIntensity;
        }

        float t = ShakeSpeed * Time.deltaTime;
        if (t > 1f)
        {
            t = 1f;
        }

        rotationShakeValue.x = Mathf.LerpUnclamped(rotationShakeValue.x, (Random.value * 2f - 1f) * shakeIntensity * positionShakeMultiplier, t);
        rotationShakeValue.y = Mathf.LerpUnclamped(rotationShakeValue.y, (Random.value * 2f - 1f) * shakeIntensity * positionShakeMultiplier, t);
        rotationShakeValue.z = Mathf.LerpUnclamped(rotationShakeValue.z, (Random.value * 2f - 1f) * shakeIntensity * rotationShakeMultiplier, t);
    }

    void Update()
    {
        UpdateShake();

        Vector2 playerPos = player.localPosition;
        Vector2 luggagePlanePos = luggagePlane.localPosition;

        if (playerPos.x < luggagePlanePos.x)
        {
            playerPos.x -= playerBounds.x;
            luggagePlanePos.x += luggagePlaneBoundsHorizontal.x;
        }
        else
        {
            playerPos.x += playerBounds.x;
            luggagePlanePos.x -= luggagePlaneBoundsHorizontal.y;
        }

        if (playerPos.y < luggagePlanePos.y)
        {
            playerPos.y -= playerBounds.y;
            luggagePlanePos.y += luggagePlaneBoundVertical.x;
        }
        else
        {
            playerPos.y += playerBounds.y;
            luggagePlanePos.y -= luggagePlaneBoundVertical.y;
        }

        //Vector2 minPos = playerPos;
        //Vector2 maxPos = playerPos;
        //if (playerPos.x < luggagePlanePos.x)
        //{
        //    maxPos.x = luggagePlanePos.x;
        //}
        //if (playerPos.y < luggagePlanePos.y)
        //{
        //    maxPos.y = luggagePlanePos.y;
        //}

        Vector2 planeDistance = playerPos - luggagePlanePos;
        Vector2 absPlaneDistance = new Vector2()
        {
            x = planeDistance.x < 0f ? -planeDistance.x : planeDistance.x,
            y = planeDistance.y < 0f ? -planeDistance.y : planeDistance.y
        };

        // Choose target size based on which axis the planes are further apart
        float targetSizeX = absPlaneDistance.x * aspectRatio * 0.5f;
        float targetSizeY = absPlaneDistance.y * 0.5f;
        float targetSize = targetSizeX > targetSizeY ? targetSizeX : targetSizeY;
        if (targetSize < minSize)
        {
            targetSize = minSize;
        }

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, (camera.orthographicSize < targetSize ? sizeLerpSpeedLarger : sizeLerpSpeedSmaller) * Time.deltaTime);

        // Center the camera between the planes
        Vector3 centerPos = Vector2.LerpUnclamped(playerPos, luggagePlanePos, 0.5f);
        Vector3 cameraPos = transform.localPosition;
        centerPos.x = Mathf.Lerp(cameraPos.x, centerPos.x, positionLerpSpeed * Time.deltaTime) + rotationShakeValue.x;
        centerPos.y = Mathf.Lerp(cameraPos.y, centerPos.y, positionLerpSpeed * Time.deltaTime) + rotationShakeValue.y;
        centerPos.z = cameraPos.z;

        camera.transform.localPosition = centerPos;
        camera.transform.localRotation = Quaternion.Euler(0f, 0f, rotationShakeValue.z);
    }
}