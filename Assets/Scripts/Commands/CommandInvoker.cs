using UnityEngine;

// CommandInvoker�F�R�}���h���󂯎����s����N���X
public class CommandInvoker : MonoBehaviour
{
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        // �K�v�ɉ����Ă����ł���Ƀ��O�Ǘ��◚���Ǘ��iUndo�Ȃǁj��ǉ��\
    }
}
