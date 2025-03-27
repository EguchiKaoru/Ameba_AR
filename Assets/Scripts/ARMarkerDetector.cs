using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARMarkerDetector : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject modelPrefab; // �e�}�[�J�[�ɑΉ�����v���n�u
    public CommandInvoker invoker; // �e���f���ɋ��ʂ�CommandInvoker���g�p

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // -----------------------------
        // �V���Ɍ��o���ꂽ�}�[�J�[
        // -----------------------------
        foreach (var trackedImage in eventArgs.added)
        {
            // �}�[�J�[�̈ʒu�E��]�ɍ��킹�ă��f���𐶐�
            var modelInstance = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);

            // ModelController�R���|�[�l���g��Invoker���Z�b�g
            var modelController = modelInstance.GetComponent<ModelController>();
            if (modelController != null)
            {
                modelController.invoker = invoker;
            }
        }

        // -----------------------------
        // �����̃}�[�J�[���X�V���ꂽ�ꍇ
        // -----------------------------
        foreach (var trackedImage in eventArgs.updated)
        {
            // �ʒu���Ԃ̍X�V���K�v�Ȃ炱���Ŏ���
        }

        // -----------------------------
        // �}�[�J�[���폜���ꂽ�ꍇ
        // -----------------------------
        foreach (var trackedImage in eventArgs.removed)
        {
            // �Y���}�[�J�[�ɕR�Â����I�u�W�F�N�g���\��/�폜���鏈���Ȃ�
        }
    }
}
