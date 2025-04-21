// File: SwipeToRotateHandler.cs
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class SwipeToRotateHandler : MonoBehaviour, IGestureHandler
{
    private Quaternion targetRotation;
    private Camera mainCamera;
    [Tooltip("スワイプ移動量あたりの回転速度")]
    public float rotationSpeed = 0.2f;

    void Awake()
    {
        targetRotation = transform.rotation;
        mainCamera = Camera.main;
    }

    public void OnDragStart(Vector2 startPosition)
    {
        Debug.Log($"[Swipe] DragStart at {startPosition}");
    }

    public void OnDrag(Vector2 delta)
    {
        // 水平ドラッグでヨー（Y軸回り）、垂直ドラッグでピッチ（X軸回り）
        float yaw = delta.x * rotationSpeed;
        float pitch = -delta.y * rotationSpeed;

        // カメラ基準の軸
        Vector3 upAxis = mainCamera.transform.up;
        Vector3 rightAxis = mainCamera.transform.right;

        // 回転を積み重ね
        targetRotation = Quaternion.AngleAxis(yaw, upAxis) * targetRotation;
        targetRotation = Quaternion.AngleAxis(pitch, rightAxis) * targetRotation;

        transform.rotation = targetRotation;
    }

    public void OnDragEnd(Vector2 endPosition)
    {
        Debug.Log($"[Swipe] DragEnd at {endPosition}");
    }

    public void OnPinchStart(float initialDistance)
    {
        // 未使用
    }
    public void OnPinch(float scaleFactor)
    {
        // 未使用
    }
    public void OnPinchEnd()
    {
        // 未使用
    }
}