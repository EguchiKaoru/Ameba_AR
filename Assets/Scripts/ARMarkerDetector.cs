using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARMarkerDetector : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject modelPrefab; // 各マーカーに対応するプレハブ
    public CommandInvoker invoker; // 各モデルに共通のCommandInvokerを使用

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
        // 新たに検出されたマーカー
        // -----------------------------
        foreach (var trackedImage in eventArgs.added)
        {
            // マーカーの位置・回転に合わせてモデルを生成
            var modelInstance = Instantiate(modelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);

            // ModelControllerコンポーネントにInvokerをセット
            var modelController = modelInstance.GetComponent<ModelController>();
            if (modelController != null)
            {
                modelController.invoker = invoker;
            }
        }

        // -----------------------------
        // 既存のマーカーが更新された場合
        // -----------------------------
        foreach (var trackedImage in eventArgs.updated)
        {
            // 位置や状態の更新が必要ならここで実装
        }

        // -----------------------------
        // マーカーが削除された場合
        // -----------------------------
        foreach (var trackedImage in eventArgs.removed)
        {
            // 該当マーカーに紐づいたオブジェクトを非表示/削除する処理など
        }
    }
}
