// File: PinchToScaleHandler.cs
using UnityEngine;

/// <summary>
/// IGestureHandler 実装：ピンチ操作によりアタッチオブジェクトのスケールを変更します。
/// </summary>
[RequireComponent(typeof(Transform))]
public class PinchToScaleHandler : MonoBehaviour, IGestureHandler
{
    private float initialDistance;
    private Vector3 initialScale;

    // OnPinchStart: ジェスチャー開始時に初期距離と初期スケールを記録
    public void OnPinchStart(float initialDistance)
    {
        this.initialDistance = initialDistance;
        initialScale = transform.localScale;
    }

    // OnPinch: 呼び出しごとに scaleFactor を適用してスケール更新
    public void OnPinch(float scaleFactor)
    {
        if (Mathf.Approximately(initialDistance, 0f)) return;
        transform.localScale = initialScale * scaleFactor;
    }

    // OnPinchEnd: 必要に応じて終了処理（今回は何もしない）
    public void OnPinchEnd() { }

    // 利用しないメソッドは空実装
    public void OnDragStart(Vector2 startPosition) { }
    public void OnDrag(Vector2 delta) { }
    public void OnDragEnd(Vector2 endPosition) { }
}
