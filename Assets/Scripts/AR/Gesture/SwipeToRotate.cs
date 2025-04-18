// File: SwipeToRotate.cs
using UnityEngine;

/// <summary>
/// 画面上でのスワイプジェスチャーにより、オブジェクトをY軸回転させるコンポーネント。
/// 1本指の移動量に応じて、回転速度を調整可能。
/// </summary>
[RequireComponent(typeof(Transform))]
public class SwipeToRotate : MonoBehaviour
{
    /// <summary>
    /// スワイプ開始位置（スクリーン座標）。
    /// </summary>
    private Vector2 startTouchPosition;

    /// <summary>
    /// スワイプ中に更新される現在のタッチ位置（スクリーン座標）。
    /// </summary>
    private Vector2 currentTouchPosition;

    /// <summary>
    /// スワイプ中フラグ。
    /// </summary>
    private bool isSwiping = false;

    /// <summary>
    /// 1ピクセルの移動あたりの回転量（度）。Inspector上で調整可能。
    /// </summary>
    [Tooltip("スワイプ距離に対する回転速度 (度/ピクセル) ")]
    public float rotationSpeed = 0.2f;

    /// <summary>
    /// 現在の目標回転値を保持するQuaternion。
    /// </summary>
    private Quaternion targetRotation;

    /// <summary>
    /// 初期化処理。開始時の回転をtargetRotationに保存。
    /// </summary>
    void Start()
    {
        targetRotation = transform.rotation;
    }

    /// <summary>
    /// 毎フレーム呼ばれ、タッチ数が1本指の場合スワイプに応じて回転を更新。
    /// </summary>
    void Update()
    {
        // 1本指のタッチを検出
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                // スワイプ開始
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                // スワイプ移動中
                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        currentTouchPosition = touch.position;
                        Vector2 delta = currentTouchPosition - startTouchPosition;

                        // 水平方向の移動量からY軸回転量を計算
                        float rotationY = delta.x * rotationSpeed;

                        // 回転を積算し、目標回転を更新
                        targetRotation *= Quaternion.Euler(0f, -rotationY, 0f);
                        transform.rotation = targetRotation;

                        // 次フレーム用に基準位置を更新
                        startTouchPosition = currentTouchPosition;
                    }
                    break;

                // スワイプ終了またはキャンセル
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isSwiping = false;
                    break;
            }
        }
    }
}
