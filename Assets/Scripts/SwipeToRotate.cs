using UnityEngine;

public class SwipeToRotate : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isSwiping = false;
    public float rotationSpeed = 0.2f;

    private Quaternion targetRotation; // ‰i‘±“I‚É•Û‚·‚é‰ñ“]

    void Start()
    {
        targetRotation = transform.rotation; // ‰Šú‚Ì‰ñ“]‚ğ•Û‘¶
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

                        // Y²‰ñ“]
                        float rotationY = delta.x * rotationSpeed;

                        // XV‚³‚ê‚½‰ñ“]‚ğ“K—piworldRotationj
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


