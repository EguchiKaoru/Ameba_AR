using UnityEngine;

// ModelController�F�\�����ꂽ���f���ɑ΂��đ�����s���N���X
public class ModelController : MonoBehaviour
{
    // �C���X�y�N�^����CommandInvoker���A�^�b�`���邩�A�V�[�����̃V���O���g���Ȃǂ𗘗p
    public CommandInvoker invoker;

    // ���[�U���͂Ȃǂɉ��������상�\�b�h
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
