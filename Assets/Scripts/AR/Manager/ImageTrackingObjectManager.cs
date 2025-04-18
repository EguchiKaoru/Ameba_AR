// File: ImageTrackingObjectManager.cs
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARFoundation の Tracked Image Manager イベント（ARTrackedImageManager：ARマーカー検出機能）を受け取り、
/// 新しいマーカーが検出されたときは、マーカー名をもとにFactoryからPrefabを生成して配置します。
/// マーカーの追跡状態が「Tracking」なら表示、「Limited/None」なら非表示に切り替えます。
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingObjectManager : MonoBehaviour
{
    /// <summary>
    /// 同じ GameObject にアタッチされた ARTrackedImageManager を自動取得します。
    /// </summary>
    private ARTrackedImageManager trackedImageManager;

    /// <summary>
    /// 起動時に呼ばれ、trackedImageManager を取得します。
    /// </summary>
    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    /// <summary>
    /// 有効化時に trackedImagesChanged イベントにハンドラを登録します。
    /// </summary>
    private void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    /// <summary>
    /// 無効化時に trackedImagesChanged イベントからハンドラを解除します。
    /// </summary>
    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    /// <summary>
    /// 画像の追加・更新・削除イベントを受け取り、Prefab の生成・表示制御を行います。
    /// </summary>
    /// <param name="args">trackedImagesChanged イベントの引数</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // 1) 新規検出されたマーカーに対し、Factory 経由で Prefab を生成
        foreach (var trackedImage in args.added)
        {
            ARObjectFactory.Create(
                trackedImage.referenceImage.name,
                trackedImage.transform
            );
        }

        // 2) トラッキング状態が変化した既存マーカーの表示／非表示を切り替え
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
