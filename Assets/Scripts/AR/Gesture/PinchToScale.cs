// File: PinchToScale.cs
using UnityEngine;

/// <summary>
/// 画面上でのピンチジェスチャーにより、オブジェクトのスケールを拡大・縮小するコンポーネント。
/// タッチが2本指の場合に動作し、初期距離と現在距離の比率からスケールを計算します。
/// </summary>
[RequireComponent(typeof(Transform))]
public class PinchToScale : MonoBehaviour
{
    /// <summary>
    /// ピンチ開始時の指と指の距離。
    /// </summary>
    private float initialDistance;

    /// <summary>
    /// ピンチ開始時のオブジェクトのローカルスケール。
    /// </summary>
    private Vector3 initialScale;

    /// <summary>
    /// 毎フレーム呼ばれ、タッチ数が2本指の場合スケールを更新。
    /// </summary>
    void Update()
    {
        // 2本のタッチが検出されているか
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // ジェスチャー開始時の処理
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                // 指の初期距離を記録
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                // オブジェクトの初期スケールを記録
                initialScale = transform.localScale;
            }
            // ジェスチャー中の移動処理
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // 現在の指の距離を測定
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                if (Mathf.Approximately(initialDistance, 0f)) return;

                // 距離の比率からスケールファクターを計算
                float scaleFactor = currentDistance / initialDistance;
                // 初期スケールにスケールファクターを乗算して設定
                transform.localScale = initialScale * scaleFactor;
            }
        }
    }
}
