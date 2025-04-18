// File: ImageTrackingObjectManager.cs
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
// ARObjectFactory を宣言している名前空間があれば using を追加してください。
// グローバル名前空間（namespace 宣言なし）で定義している場合、以下は不要です。
// using YourNamespace.AR.Factory;


/// <summary>
/// ARFoundation の Tracked Image Manager イベント（ARTrackedImageManager：ARマーカー検出機能）を受け取り、
/// 新しいマーカーが検出されたときは、マーカー名をもとにFactoryからPrefabを生成して配置します。
/// マーカーの追跡状態が「Tracking」なら表示、「Limited/None」なら非表示に切り替えます。
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingObjectManager : MonoBehaviour
{
    /// <summary>
    /// AR Tracked Image Manager コンポーネントへの参照。
    /// Inspector でシーン内の ARTrackedImageManager を割り当ててください。
    /// </summary>
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    /// <summary>
    /// MonoBehaviour が有効化された際に呼ばれる。
    /// trackedImageManager の trackedImagesChanged イベントにハンドラを登録する。
    /// </summary>
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    /// <summary>
    /// MonoBehaviour が無効化される際に呼ばれる。
    /// イベントハンドラを解除してメモリリークを防止する。
    /// </summary>
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    /// <summary>
    /// ARFoundation の画像追加・更新・削除イベントを受け取り、
    /// Prefab の生成および表示制御を行う。
    /// </summary>
    /// <param name="args">trackedImagesChanged イベントの引数</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // 1) 新たに検出されたマーカーに対し Factory を使って Prefab を生成
        foreach (var trackedImage in args.added)
        {
            ARObjectFactory.Create(
                trackedImage.referenceImage.name,
                trackedImage.transform
            );
        }

        // 2) 既存マーカーの追跡状態が変化した際の表示制御
        foreach (var trackedImage in args.updated)
        {
            bool isTracking = trackedImage.trackingState == TrackingState.Tracking;
            trackedImage.gameObject.SetActive(isTracking);
        }

        // 3) トラッキング対象外になったマーカーは非アクティブ化
        foreach (var trackedImage in args.removed)
        {
            trackedImage.gameObject.SetActive(false);
        }
    }
}