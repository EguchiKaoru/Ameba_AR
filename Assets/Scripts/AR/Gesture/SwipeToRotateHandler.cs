// File: SwipeToRotateHandler.cs
using UnityEngine;

/// <summary>
/// IGestureHandler 実装：ドラッグ操作によりアタッチオブジェクトを Y 軸回転させます。
/// </summary>
[RequireComponent(typeof(Transform))]
public class SwipeToRotateHandler : MonoBehaviour, IGestureHandler
{
    private Quaternion targetRotation;
    public float rotationSpeed = 0.2f;

    void Awake()
    {
        // 初期回転を保存
        targetRotation = transform.rotation;
    }

    // OnDragStart: 必要なら開始位置を取得（今回は使用しない）
    public void OnDragStart(Vector2 startPosition) { }

    // OnDrag: delta.x に基づき左右回転
    public void OnDrag(Vector2 delta)
    {
        float rotationY = delta.x * rotationSpeed;
        targetRotation *= Quaternion.Euler(0f, -rotationY, 0f);
        transform.rotation = targetRotation;
    }

    // OnDragEnd: 終了時に何もしない
    public void OnDragEnd(Vector2 endPosition) { }

    // 利用しないピンチ系メソッドは空実装
    public void OnPinchStart(float initialDistance) { }
    public void OnPinch(float scaleFactor) { }
    public void OnPinchEnd() { }
}