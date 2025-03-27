using UnityEngine;

// ICommandインターフェース：すべてのコマンドはこのインターフェースを実装する
public interface ICommand
{
    void Execute();
}