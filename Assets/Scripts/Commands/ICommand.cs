using UnityEngine;

/// <summary>
/// すべてのコマンドが実装するインターフェース
/// </summary>
public interface ICommand
{
    void Execute();
}
