using UnityEngine;

public class SwipeToRotate : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isSwiping = false;
    public float rotationSpeed = 0.2f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
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

                        float rotationY = delta.x * rotationSpeed;
                        float rotationX = -delta.y * rotationSpeed;

                        Vector3 camRight = mainCamera.transform.right;
                        Vector3 camUp = mainCamera.transform.up;

                        transform.Rotate(camUp, rotationY, Space.World);
                        transform.Rotate(camRight, rotationX, Space.World);

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
