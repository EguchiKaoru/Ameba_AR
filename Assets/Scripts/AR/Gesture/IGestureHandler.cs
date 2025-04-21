// File: IGestureHandler.cs
using UnityEngine;

/// <summary>
/// タッチジェスチャーのイベントを受け取るハンドラ用インターフェース。
/// GestureInputManager から呼び出され、各ジェスチャー動作をオブジェクト側で処理します。
/// </summary>
public interface IGestureHandler
{
    /// <summary>
    /// ドラッグ（スワイプ）操作の開始時に呼ばれます。
    /// </summary>
    /// <param name="startPosition">画面座標でのタッチ開始位置</param>
    void OnDragStart(Vector2 startPosition);

    /// <summary>
    /// ドラッグ中の移動量に応じて呼ばれます。
    /// </summary>
    /// <param name="delta">前フレームからの指の移動量</param>
    void OnDrag(Vector2 delta);

    /// <summary>
    /// ドラッグ操作終了時に呼ばれます。
    /// </summary>
    /// <param name="endPosition">画面座標でのタッチ終了位置</param>
    void OnDragEnd(Vector2 endPosition);

    /// <summary>
    /// ピンチ（拡大/縮小）操作の開始時に呼ばれます。
    /// </summary>
    /// <param name="initialDistance">ジェスチャー開始時の2本指間の距離</param>
    void OnPinchStart(float initialDistance);

    /// <summary>
    /// ピンチ操作中に呼ばれ、拡大縮小率を通知します。
    /// </summary>
    /// <param name="scaleFactor">初期距離に対する現在距離の比率</param>
    void OnPinch(float scaleFactor);

    /// <summary>
    /// ピンチ操作終了時に呼ばれます。
    /// </summary>
    void OnPinchEnd();
}
