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
    public static GestureInputManager Instance { get; private set; }

    [Tooltip("タッチ操作を受け付けるレイヤー（Interactableなど）")]
    [SerializeField]
    private LayerMask interactableLayerMask;

    private Camera mainCamera;
    private Dictionary<int, IGestureHandler> activeHandlers = new Dictionary<int, IGestureHandler>();

    private bool pinchActive = false;
    private IGestureHandler pinchHandler;
    private float initialPinchDistance;

    private void Awake()
    {
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
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (touchCount == 2)
        {
            HandlePinch(Input.GetTouch(0), Input.GetTouch(1));
        }
        else if (pinchActive)
        {
            EndPinch();
        }
    }

    #region Single Touch (Drag)

    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
                {
                    var handler = hit.collider.GetComponent<IGestureHandler>();
                    if (handler != null)
                    {
                        activeHandlers[touch.fingerId] = handler;
                        Debug.Log($"[GIM] DragStart on {hit.collider.gameObject.name} (fingerId={touch.fingerId})");
                        // IGestureHandler.OnDragStart は Vector2 を受け取るよう修正
                        handler.OnDragStart(touch.position);
                    }
                }
                break;

            case TouchPhase.Moved:
                if (activeHandlers.TryGetValue(touch.fingerId, out var dragHandler))
                {
                    dragHandler.OnDrag(touch.deltaPosition);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (activeHandlers.TryGetValue(touch.fingerId, out var endHandler))
                {
                    endHandler.OnDragEnd(touch.position);
                    Debug.Log($"[GIM] DragEnd on fingerId={touch.fingerId}");
                    activeHandlers.Remove(touch.fingerId);
                }
                break;
        }
    }

    #endregion

    #region Two-Finger Touch (Pinch)

    private void HandlePinch(Touch touch1, Touch touch2)
    {
        if (!pinchActive &&
            (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began))
        {
            Ray r1 = mainCamera.ScreenPointToRay(touch1.position);
            Ray r2 = mainCamera.ScreenPointToRay(touch2.position);
            if (Physics.Raycast(r1, out var hit1, Mathf.Infinity, interactableLayerMask) &&
                Physics.Raycast(r2, out var hit2, Mathf.Infinity, interactableLayerMask) &&
                hit1.collider.gameObject == hit2.collider.gameObject)
            {
                pinchHandler = hit1.collider.GetComponent<IGestureHandler>();
                if (pinchHandler != null)
                {
                    pinchActive = true;
                    initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    Debug.Log("[GIM] PinchStart");
                    pinchHandler.OnPinchStart(initialPinchDistance);
                }
            }
        }
        else if (pinchActive && (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved))
        {
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float scaleFactor = initialPinchDistance > 0f ? currentDistance / initialPinchDistance : 1f;
            pinchHandler?.OnPinch(scaleFactor);
        }
        else if (pinchActive && (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled))
        {
            Debug.Log("[GIM] PinchEnd");
            pinchHandler?.OnPinchEnd();
            pinchActive = false;
            pinchHandler = null;
        }
    }

    private void EndPinch()
    {
        Debug.Log("[GIM] PinchEnd");
        pinchHandler?.OnPinchEnd();
        pinchActive = false;
        pinchHandler = null;
    }

    #endregion
}