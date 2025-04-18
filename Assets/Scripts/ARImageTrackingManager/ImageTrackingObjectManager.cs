// File: ImageTrackingObjectManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARFoundationのTracked Image Managerイベントをフックし、
/// リファレンスマーカーの検出・更新・削除に応じてプレハブを生成・表示・非表示するクラス。
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingObjectManager : MonoBehaviour
{
    /// <summary>
    /// ARTrackedImageManagerコンポーネントへの参照。
    /// シーン上のAR Session OriginにアタッチされたComponentをドラッグ＆ドロップで設定する。
    /// </summary>
    [HideInInspector]
    public ARTrackedImageManager arTrackedImageManager;

    /// <summary>
    /// リファレンス画像名とプレハブのマッピングリスト。
    /// 名前だけのエントリは、Editorスクリプトで自動的に補完される。
    /// </summary>
    [HideInInspector, SerializeField]
    public List<NameToPrefab> markerNameToPrefab = new List<NameToPrefab>();

    #region Unity Lifecycle

    /// <summary>
    /// コンポーネントが有効化されたときに呼ばれる。
    /// AR FoundationのtrackedImagesChangedイベントにハンドラを登録する。
    /// </summary>
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    /// <summary>
    /// コンポーネントが無効化されたときに呼ばれる。
    /// イベントハンドラを解除してメモリリークを防止。
    /// </summary>
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    #endregion

    #region AR Image Tracking Callbacks

    /// <summary>
    /// 画像の追加・更新・削除イベント時に呼ばれる。
    /// added: 新規検出
    /// updated: トラッキング状態の変化
    /// removed: トラッキング対象外
    /// </summary>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 新しく認識された画像に対する処理：プレハブを生成
        foreach (var trackedImage in eventArgs.added)
        {
            string imageName = trackedImage.referenceImage.name;
            GameObject prefab = markerNameToPrefab.Find(x => x.name == imageName)?.prefab;
            if (prefab != null)
            {
                // 検出位置・回転でインスタンス化し、trackedImageの子要素に設定
                Instantiate(prefab,
                            trackedImage.transform.position,
                            trackedImage.transform.rotation,
                            trackedImage.transform);
            }
        }

        // トラッキング状態が変化したときの処理：表示・非表示を切り替え
        foreach (var trackedImage in eventArgs.updated)
        {
            bool isTracked = trackedImage.trackingState == TrackingState.Tracking;
            trackedImage.gameObject.SetActive(isTracked);
        }

        // トラッキング対象外になったときの処理：非表示
        foreach (var trackedImage in eventArgs.removed)
        {
            trackedImage.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Reference Library Sync

    /// <summary>
    /// 指定したIReferenceImageLibrary内に、指定の名前の画像が存在するかチェックする。
    /// </summary>
    private bool HasNameInReferenceLibrary(IReferenceImageLibrary library, string name)
    {
        for (int i = 0; i < library.count; i++)
        {
            if (library[i].name == name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// markerNameToPrefabリストをReferenceImageLibraryの内容と同期させる。
    /// - 存在しない名前は削除
    /// - 新規登録された画像は名前のみエントリを追加
    /// </summary>
    public void UpdateNameToPrefabMappings()
    {
        if (arTrackedImageManager == null || arTrackedImageManager.referenceLibrary == null)
            return;

        // (1) リファレンスライブラリにない名前のマッピングを削除
        for (int i = markerNameToPrefab.Count - 1; i >= 0; i--)
        {
            if (!HasNameInReferenceLibrary(arTrackedImageManager.referenceLibrary,
                                            markerNameToPrefab[i].name))
            {
                markerNameToPrefab.RemoveAt(i);
            }
        }

        // (2) ライブラリに追加された画像名をマッピングに追加
        for (int i = 0; i < arTrackedImageManager.referenceLibrary.count; i++)
        {
            string name = arTrackedImageManager.referenceLibrary[i].name;
            if (!markerNameToPrefab.Exists(x => x.name == name))
            {
                markerNameToPrefab.Add(new NameToPrefab { name = name });
            }
        }
    }

    #endregion
}