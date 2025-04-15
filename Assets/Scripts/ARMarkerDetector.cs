using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARTrackedImageManager のイベントを受け取り、対応する Prefab を生成するサンプル
/// (生成時に専用のコンテナに配置して、トラッキング対象の子から切り離す)
/// </summary>
public class ARMarkerDetector : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private CommandInvoker invoker;

    // モデルを配置する専用コンテナを設定（シーン上の空のオブジェクトを用意して、ここに割り当てる）
    [SerializeField] private Transform modelsContainer;

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
        // 新たに検出されたマーカーに対してモデルを生成
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log($"Tracked image added: {trackedImage.referenceImage.name}");

            // Instantiate 時に、親として modelsContainer を指定することで、
            // 生成されたモデルは ARTrackedImage の子ではなく、専用コンテナの子になる
            var modelInstance = Instantiate(modelPrefab,
                                            trackedImage.transform.position,
                                            trackedImage.transform.rotation,
                                            modelsContainer);

            var modelController = modelInstance.GetComponent<ModelController>();
            if (modelController != null)
            {
                modelController.invoker = invoker;
            }
        }

        // マーカーの更新（例として、Tracking 状態が Tracking の場合のみログ出力）
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Debug.Log($"Tracked image is tracking: {trackedImage.referenceImage.name}");
            }
        }

        // マーカーが失われたときの処理
        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log($"Tracked image removed: {trackedImage.referenceImage.name}");
            // 必要に応じて、生成済みモデルの削除処理などを追加する
            Debug.Log($"必要に応じて、生成済みモデルの削除処理などを追加する: {trackedImage.referenceImage.name}");
        }
    }
}
