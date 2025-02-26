using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;

public class ARInteractionManager : MonoBehaviour
{
    public Camera arCamera;

    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Ray ray = arCamera.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Touched: " + hit.transform.name);
            }
        }
    }
}
