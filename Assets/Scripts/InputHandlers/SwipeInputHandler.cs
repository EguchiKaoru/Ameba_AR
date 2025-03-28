using UnityEngine;

/// <summary>
/// スワイプ入力を検知し、回転コマンドを実行するハンドラ
/// </summary>
public class SwipeInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private CommandInvoker invoker;
    [SerializeField] private float rotationSpeed = 0.2f;

    private Vector2 startTouchPosition;
    private bool isSwiping = false;

    /// <summary>
    /// 入力を処理する
    /// </summary>
    public void ProcessInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        Vector2 currentTouchPosition = touch.position;
                        Vector2 delta = currentTouchPosition - startTouchPosition;
                        float rotationY = delta.x * rotationSpeed;

                        // 微小な変化ならログ出力しない（閾値: 1）
                        if (Mathf.Abs(delta.x) >= 1f)
                        {
                            Debug.Log($"Swipe delta: {delta}, calculated rotation: {rotationY}");
                            var rotateCmd = new RotateCommand(transform, -rotationY);
                            invoker.ExecuteCommand(rotateCmd);
                        }

                        startTouchPosition = currentTouchPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isSwiping = false;
                    break;
            }
        }
    }

}
