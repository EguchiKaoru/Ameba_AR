using UnityEngine;

/// <summary>
/// モデルを拡大・縮小させるコマンド
/// </summary>
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
        Debug.Log($"[ScaleCommand] scale factor {scaleFactor}");
    }
}
