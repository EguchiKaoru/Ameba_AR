using UnityEngine;

/// <summary>
/// モデルを回転させるコマンド
/// </summary>
public class RotateCommand : ICommand
{
    private Transform model;
    private float angle;

    public RotateCommand(Transform model, float angle)
    {
        this.model = model;
        this.angle = angle;
    }

    public void Execute()
    {
        model.Rotate(Vector3.up, angle);
        Debug.Log($"[RotateCommand] {angle} degrees rotated");
    }
}
