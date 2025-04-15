using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARTrackedImageManager のイベントを受け取り、対応する Prefab を生成するサンプル
/// (生成時にモデルをトラッキング対象の子から切り離す)
/// </summary>
public class ARMarkerDetector : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private CommandInvoker invoker;

    // 【オプション】モデルを別のコンテナに入れたい場合は、下記のコメントを外して使ってください
    // [SerializeField] private Transform modelsContainer;

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
        // マーカーが新たに検出されたとき
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log($"Tracked image added: {trackedImage.referenceImage.name}");

            // Instantiate 時に親を指定しない（または、必要なら別のコンテナに配置する）
            GameObject modelInstance = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            // 【オプション】モデルを特定のコンテナに配置する場合
            // GameObject modelInstance = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation, modelsContainer);

            // ModelController コンポーネントがあればシーン上の CommandInvoker を注入
            ModelController modelController = modelInstance.GetComponent<ModelController>();
            if (modelController != null)
            {
                modelController.invoker = invoker;
            }
        }

        // マーカーの更新（Tracking 状態の変化に応じて必要な処理を追加）
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Debug.Log($"Tracked image is tracking: {trackedImage.referenceImage.name}");
            }
            // ここで、ユーザー操作のオフセットを適用する等の処理を追加することも検討できる
        }

        // マーカーが失われたとき
        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log($"Tracked image removed: {trackedImage.referenceImage.name}");
            // ここで、生成済みのモデルの削除や非表示にする処理を追加してください
        }
    }
}
