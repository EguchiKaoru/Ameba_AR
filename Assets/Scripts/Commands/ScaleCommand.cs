using UnityEngine;

// ScaleCommand：モデルを指定の倍率で拡大・縮小させるコマンド
public class ScaleCommand : ICommand
{
    private Transform model;
    private Vector3 scaleFactor;

    public ScaleCommand(Transform model, Vector3 scaleFactor)
    {
        this.model = model;
        this.scaleFactor = scaleFactor;
    }

    public void Execute()
    {
        model.localScale = Vector3.Scale(model.localScale, scaleFactor);
        Debug.Log($"ScaleCommand executed: scale factor {scaleFactor}");
    }
}