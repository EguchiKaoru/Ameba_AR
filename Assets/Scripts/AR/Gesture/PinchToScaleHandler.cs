// File: PinchToScaleHandler.cs
using UnityEngine;

/// <summary>
/// IGestureHandler 実装：ピンチ操作によりアタッチオブジェクトを拡大/縮小します。
/// ログは開始時と終了時のみ出力し、継続的なログは行いません。
/// </summary>
[RequireComponent(typeof(Transform))]
public class PinchToScaleHandler : MonoBehaviour, IGestureHandler
{
    private Vector3 initialScale;
    private float startPinchDistance;

    void Awake()
    {
        // 初期スケールを保存
        initialScale = transform.localScale;
    }

    /// <summary>
    /// ピンチ開始時に呼ばれ、一度だけログを出力します。
    /// </summary>
    public void OnPinchStart(float initialDistance)
    {
        startPinchDistance = initialDistance;
        initialScale = transform.localScale;
        Debug.Log($"[Pinch] {gameObject.name} pinch started. initial distance = {startPinchDistance:F2}");
    }

    /// <summary>
    /// ピンチ中に呼ばれ、スケールを更新します（ログ出力なし）。
    /// </summary>
    public void OnPinch(float scaleFactor)
    {
        transform.localScale = initialScale * scaleFactor;
    }

    /// <summary>
    /// ピンチ終了時に呼ばれ、一度だけ最終スケールをログ出力します。
    /// </summary>
    public void OnPinchEnd()
    {
        float finalScale = transform.localScale.x; // 均一スケールを仮定
        Debug.Log($"[Pinch] {gameObject.name} pinch ended. final scale = {finalScale:F2}");
    }

    // ドラッグ系メソッドは未使用のため空実装
    public void OnDragStart(Vector2 startPosition) { }
    public void OnDrag(Vector2 delta) { }
    public void OnDragEnd(Vector2 endPosition) { }
}
