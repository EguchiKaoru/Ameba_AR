using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageVisualizer : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    // 表示する 3D モデルのプレハブ（Inspector で設定）
    public GameObject modelPrefab;

    // 生成したオブジェクトを保持する辞書
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
        // 新しく検出された画像に対してモデルを生成
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // 画像の名前（Reference Image Library に設定した名前）をキーにする
            string imageName = trackedImage.referenceImage.name;
            GameObject spawnedModel = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            // 親を trackedImage にすることで、画像のトラッキングに合わせて動くようにする
            spawnedModel.transform.parent = trackedImage.transform;
            spawnedObjects[imageName] = spawnedModel;
        }

        // 検出中の画像に対して更新（位置や向きの同期）
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedObjects.TryGetValue(imageName, out GameObject spawnedModel))
            {
                spawnedModel.transform.position = trackedImage.transform.position;
                spawnedModel.transform.rotation = trackedImage.transform.rotation;
            }
        }

        // トラッキングが外れた画像に対してモデルを削除
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
