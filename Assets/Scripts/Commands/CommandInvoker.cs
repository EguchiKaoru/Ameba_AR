using UnityEngine;

/// <summary>
/// コマンド実行を管理するクラス
/// </summary>
public class CommandInvoker : MonoBehaviour
{
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        // 必要に応じてコマンドの履歴管理などを追加
    }
}
