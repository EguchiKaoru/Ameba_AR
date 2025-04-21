// File: GestureInputManager.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// タッチ入力を一元的に検知し、画面上のInteractableレイヤーのオブジェクトに
/// 対応するIGestureHandlerのジェスチャーメソッドを通知します。
/// ・単一指：ドラッグ (OnDragStart/OnDrag/OnDragEnd)
/// ・二本指：ピンチ (OnPinchStart/OnPinch/OnPinchEnd)
/// </summary>
public class GestureInputManager : MonoBehaviour
{
    /// <summary>
    /// シーン内で唯一のインスタンスを保持するシングルトン
    /// </summary>
    public static GestureInputManager Instance { get; private set; }

    [Tooltip("タッチ操作を受け付けるレイヤー（Interactableなど）")]
    [SerializeField]
    private LayerMask interactableLayerMask;

    private Camera mainCamera;

    // ドラッグ操作中のタッチIDと対応ハンドラのマッピング
    private Dictionary<int, IGestureHandler> dragHandlers = new Dictionary<int, IGestureHandler>();

    // ピンチ操作中のフラグと対象ハンドラ
    private bool pinchActive = false;
    private IGestureHandler pinchHandler;
    private float initialPinchDistance;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // メインカメラをキャッシュ
        mainCamera = Camera.main;
    }

    private void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (touchCount == 2)
        {
            HandlePinch(Input.GetTouch(0), Input.GetTouch(1));
        }
        else if (pinchActive)
        {
            // タッチが2本から外れた場合、ピンチ終了処理
            EndPinch();
        }
    }

    #region Single Touch (Drag)

    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                // レイキャストで操作対象を取得
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, interactableLayerMask))
                {
                    var handler = hit.collider.GetComponent<IGestureHandler>();
                    if (handler != null)
                    {
                        dragHandlers[touch.fingerId] = handler;
                        handler.OnDragStart(touch.position);
                    }
                }
                break;

            case TouchPhase.Moved:
                if (dragHandlers.TryGetValue(touch.fingerId, out var dragHandler))
                {
                    dragHandler.OnDrag(touch.deltaPosition);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (dragHandlers.TryGetValue(touch.fingerId, out var endHandler))
                {
                    endHandler.OnDragEnd(touch.position);
                    dragHandlers.Remove(touch.fingerId);
                }
                break;
        }
    }

    #endregion

    #region Two-Finger Touch (Pinch)

    private void HandlePinch(Touch touch1, Touch touch2)
    {
        // ピンチ開始
        if (!pinchActive &&
            (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began))
        {
            // 2本指とも同じオブジェクトを指しているかチェック
            Ray ray1 = mainCamera.ScreenPointToRay(touch1.position);
            Ray ray2 = mainCamera.ScreenPointToRay(touch2.position);
            if (Physics.Raycast(ray1, out var hit1, Mathf.Infinity, interactableLayerMask) &&
                Physics.Raycast(ray2, out var hit2, Mathf.Infinity, interactableLayerMask) &&
                hit1.collider.gameObject == hit2.collider.gameObject)
            {
                pinchHandler = hit1.collider.GetComponent<IGestureHandler>();
                if (pinchHandler != null)
                {
                    pinchActive = true;
                    initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    pinchHandler.OnPinchStart(initialPinchDistance);
                }
            }
        }
        // ピンチ中の更新
        else if (pinchActive && (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved))
        {
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float scaleFactor = initialPinchDistance > 0f ? currentDistance / initialPinchDistance : 1f;
            pinchHandler?.OnPinch(scaleFactor);
        }
        // ピンチ終了
        else if (pinchActive && (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled))
        {
            pinchHandler?.OnPinchEnd();
            pinchActive = false;
            pinchHandler = null;
        }
    }

    private void EndPinch()
    {
        pinchHandler?.OnPinchEnd();
        pinchActive = false;
        pinchHandler = null;
    }

    #endregion
}
