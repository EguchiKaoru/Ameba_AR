// File: GestureInputManager.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// タッチ入力を一元的に検知し、
/// Interactableレイヤー上のオブジェクトに対して
/// IGestureHandlerインターフェースのメソッドを呼び出す
/// ・単一指：ドラッグ (OnDragStart/OnDrag/OnDragEnd)
/// ・二本指：ピンチ (OnPinchStart/OnPinch/OnPinchEnd)
/// </summary>
public class GestureInputManager : MonoBehaviour
{
    /// <summary>シングルトンインスタンス</summary>
    public static GestureInputManager Instance { get; private set; }

    [Tooltip("タッチ操作を受け付けるレイヤー（例：Interactableなど）")]
    [SerializeField]
    private LayerMask interactableLayerMask;

    /// <summary>メインカメラへの参照</summary>
    private Camera mainCamera;

    /// <summary>
    /// 指ごとにアタッチされた複数のIGestureHandlerを保持する
    /// key: fingerId, value: ハンドラリスト
    /// </summary>
    private Dictionary<int, List<IGestureHandler>> activeHandlers = new Dictionary<int, List<IGestureHandler>>();

    /// <summary>現在ピンチ中かどうか</summary>
    private bool pinchActive = false;

    /// <summary>ピンチ中のハンドラリスト</summary>
    private List<IGestureHandler> pinchHandlers;

    /// <summary>ピンチ開始時の2本指間距離</summary>
    private float initialPinchDistance;

    private void Awake()
    {
        // シングルトンパターン: 重複インスタンスは破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            // 単一指：ドラッグ処理
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (touchCount == 2)
        {
            // 二本指：ピンチ処理
            HandlePinch(Input.GetTouch(0), Input.GetTouch(1));
        }
        else if (pinchActive)
        {
            // 指が離れた場合、ピンチ終了扱い
            EndPinch();
        }
    }

    #region Single Touch (Drag)

    /// <summary>
    /// 単一指によるドラッグ操作を管理
    /// </summary>
    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                // タッチ開始時にオブジェクトを検出
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
                {
                    // ColliderからすべてのIGestureHandlerを取得
                    var handlers = new List<IGestureHandler>(hit.collider.GetComponents<IGestureHandler>());
                    if (handlers.Count > 0)
                    {
                        // fingerIdごとにハンドラを登録
                        activeHandlers[touch.fingerId] = handlers;
                        Debug.Log($"[GIM] DragStart on {hit.collider.gameObject.name} (fingerId={touch.fingerId})");
                        // 各ハンドラに開始通知
                        foreach (var handler in handlers)
                            handler.OnDragStart(touch.position);
                    }
                }
                break;

            case TouchPhase.Moved:
                // ドラッグ中はDeltaを通知
                if (activeHandlers.TryGetValue(touch.fingerId, out var dragHandlers))
                {
                    foreach (var handler in dragHandlers)
                        handler.OnDrag(touch.deltaPosition);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                // タッチ終了時に終了通知と登録解除
                if (activeHandlers.TryGetValue(touch.fingerId, out var endHandlers))
                {
                    foreach (var handler in endHandlers)
                        handler.OnDragEnd(touch.position);
                    Debug.Log($"[GIM] DragEnd on fingerId={touch.fingerId}");
                    activeHandlers.Remove(touch.fingerId);
                }
                break;
        }
    }

    #endregion

    #region Two-Finger Touch (Pinch)

    /// <summary>
    /// 二本指によるピンチ (拡大/縮小) 操作を管理
    /// </summary>
    private void HandlePinch(Touch touch1, Touch touch2)
    {
        if (!pinchActive &&
            (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began))
        {
            // 両指とも同一オブジェクト上ならピンチ開始
            Ray r1 = mainCamera.ScreenPointToRay(touch1.position);
            Ray r2 = mainCamera.ScreenPointToRay(touch2.position);
            if (Physics.Raycast(r1, out var hit1, Mathf.Infinity, interactableLayerMask) &&
                Physics.Raycast(r2, out var hit2, Mathf.Infinity, interactableLayerMask) &&
                hit1.collider.gameObject == hit2.collider.gameObject)
            {
                pinchHandlers = new List<IGestureHandler>(hit1.collider.GetComponents<IGestureHandler>());
                if (pinchHandlers.Count > 0)
                {
                    pinchActive = true;
                    initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    Debug.Log("[GIM] PinchStart");
                    foreach (var handler in pinchHandlers)
                        handler.OnPinchStart(initialPinchDistance);
                }
            }
        }
        else if (pinchActive && (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved))
        {
            // 移動中はスケール比を通知
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float scaleFactor = initialPinchDistance > 0f ? currentDistance / initialPinchDistance : 1f;
            foreach (var handler in pinchHandlers)
                handler.OnPinch(scaleFactor);
        }
        else if (pinchActive &&
                 (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled ||
                  touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled))
        {
            // 指の離脱でピンチ終了
            Debug.Log("[GIM] PinchEnd");
            foreach (var handler in pinchHandlers)
                handler.OnPinchEnd();
            pinchActive = false;
            pinchHandlers = null;
        }
    }

    /// <summary>
    /// 予期せぬタイミングでのピンチ終了処理
    /// </summary>
    private void EndPinch()
    {
        Debug.Log("[GIM] PinchEnd");
        if (pinchHandlers != null)
        {
            foreach (var handler in pinchHandlers)
                handler.OnPinchEnd();
        }
        pinchActive = false;
        pinchHandlers = null;
    }

    #endregion
}
