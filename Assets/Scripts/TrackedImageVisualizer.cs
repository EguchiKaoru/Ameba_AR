using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageVisualizer : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    // �\������ 3D ���f���̃v���n�u�iInspector �Őݒ�j
    public GameObject modelPrefab;

    // ���������I�u�W�F�N�g��ێ����鎫��
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // �V�������o���ꂽ�摜�ɑ΂��ă��f���𐶐�
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // �摜�̖��O�iReference Image Library �ɐݒ肵�����O�j���L�[�ɂ���
            string imageName = trackedImage.referenceImage.name;
            GameObject spawnedModel = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            // �e�� trackedImage �ɂ��邱�ƂŁA�摜�̃g���b�L���O�ɍ��킹�ē����悤�ɂ���
            spawnedModel.transform.parent = trackedImage.transform;
            spawnedObjects[imageName] = spawnedModel;
        }

        // ���o���̉摜�ɑ΂��čX�V�i�ʒu������̓����j
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedObjects.TryGetValue(imageName, out GameObject spawnedModel))
            {
                spawnedModel.transform.position = trackedImage.transform.position;
                spawnedModel.transform.rotation = trackedImage.transform.rotation;
            }
        }

        // �g���b�L���O���O�ꂽ�摜�ɑ΂��ă��f�����폜
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedObjects.TryGetValue(imageName, out GameObject spawnedModel))
            {
                Destroy(spawnedModel);
                spawnedObjects.Remove(imageName);
            }
        }
    }
}
