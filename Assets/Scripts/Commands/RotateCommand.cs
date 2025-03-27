using UnityEngine;

// RotateCommand：モデルを指定角度回転させるコマンド
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
        Debug.Log($"RotateCommand executed: {angle} degrees rotated");
    }
}