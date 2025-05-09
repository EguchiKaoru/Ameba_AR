using UnityEngine;

public class SwipeToRotate : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isSwiping = false;
    public float rotationSpeed = 0.2f;

    private Quaternion targetRotation; // 永続的に保持する回転

    void Start()
    {
        targetRotation = transform.rotation; // 初期の回転を保存
    }

    void Update()
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
                        currentTouchPosition = touch.position;
                        Vector2 delta = currentTouchPosition - startTouchPosition;

                        // Y軸回転
                        float rotationY = delta.x * rotationSpeed;

                        // 更新された回転を適用（worldRotation）
                        targetRotation *= Quaternion.Euler(0, -rotationY, 0);
                        transform.rotation = targetRotation;

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


