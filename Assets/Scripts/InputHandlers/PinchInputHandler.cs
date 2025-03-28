using UnityEngine;

/// <summary>
/// ピンチ操作を検知し、拡大コマンドを実行するハンドラ（例）
/// </summary>
public class PinchInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private CommandInvoker invoker;
    [SerializeField] private float scaleSpeed = 0.01f;

    private float lastDistance = 0f;
    private bool isPinching = false;

    public void ProcessInput()
    {
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                lastDistance = Vector2.Distance(t1.position, t2.position);
                isPinching = true;
            }
            else if ((t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved) && isPinching)
            {
                float currentDistance = Vector2.Distance(t1.position, t2.position);
                float diff = currentDistance - lastDistance;

                float scaleFactorValue = 1 + (diff * scaleSpeed);
                Vector3 scaleFactor = new Vector3(scaleFactorValue, scaleFactorValue, scaleFactorValue);

                var scaleCmd = new ScaleCommand(transform, scaleFactor);
                invoker.ExecuteCommand(scaleCmd);

                lastDistance = currentDistance;
            }
        }
        else
        {
            isPinching = false;
        }
    }

    private void Update()
    {
        ProcessInput();
    }
}
