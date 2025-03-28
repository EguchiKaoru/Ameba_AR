using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARTrackedImageManagerのイベントを受け取り、対応するPrefabを生成するサンプル
/// </summary>
public class ARMarkerDetector : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private CommandInvoker invoker;

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log($"Tracked image added: {trackedImage.referenceImage.name}");
            var modelInstance = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            var modelController = modelInstance.GetComponent<ModelController>();
            if (modelController != null)
            {
                modelController.invoker = invoker;
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            Debug.Log($"Tracked image updated: {trackedImage.referenceImage.name}");
            // 必要に応じて位置・回転の更新など
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log($"Tracked image removed: {trackedImage.referenceImage.name}");
            // マーカーが見えなくなった際の処理（オブジェクト削除など）
        }
    }
}