using UnityEngine;

// ModelController：表示されたモデルに対して操作を行うクラス
public class ModelController : MonoBehaviour
{
    // インスペクタからCommandInvokerをアタッチするか、シーン内のシングルトンなどを利用
    public CommandInvoker invoker;

    // ユーザ入力などに応じた操作メソッド
    public void RotateModel(float angle)
    {
        ICommand rotateCommand = new RotateCommand(transform, angle);
        invoker.ExecuteCommand(rotateCommand);
    }

    public void ScaleModel(Vector3 scaleFactor)
    {
        ICommand scaleCommand = new ScaleCommand(transform, scaleFactor);
        invoker.ExecuteCommand(scaleCommand);
    }
}
