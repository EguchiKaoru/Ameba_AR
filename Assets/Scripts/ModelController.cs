using UnityEngine;

/// <summary>
/// マーカーで生成されたモデルなどにアタッチして、UIボタンから直接コマンドを実行する例
/// </summary>
public class ModelController : MonoBehaviour
{
    public CommandInvoker invoker;

    public void RotateModel(float angle)
    {
        var rotateCommand = new RotateCommand(transform, angle);
        invoker.ExecuteCommand(rotateCommand);
    }

    public void ScaleModel(Vector3 scaleFactor)
    {
        var scaleCommand = new ScaleCommand(transform, scaleFactor);
        invoker.ExecuteCommand(scaleCommand);
    }
}
