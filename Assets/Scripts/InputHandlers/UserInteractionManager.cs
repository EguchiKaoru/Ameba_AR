using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数の IInputHandler を一括管理する例
/// </summary>
public class UserInteractionManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> inputHandlerComponents;

    private List<IInputHandler> inputHandlers = new List<IInputHandler>();

    private void Awake()
    {
        // Inspectorで設定した MonoBehaviour を IInputHandler にキャスト
        foreach (var comp in inputHandlerComponents)
        {
            if (comp is IInputHandler handler)
            {
                inputHandlers.Add(handler);
            }
        }
    }

    private void Update()
    {
        // 登録されているすべてのハンドラの入力を処理
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput();
        }
    }
}
