using UnityEngine;

// CommandInvoker：コマンドを受け取り実行するクラス
public class CommandInvoker : MonoBehaviour
{
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        // 必要に応じてここでさらにログ管理や履歴管理（Undoなど）を追加可能
    }
}
