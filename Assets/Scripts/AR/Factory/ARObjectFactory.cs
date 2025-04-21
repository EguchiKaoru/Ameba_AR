// File: ARObjectFactory.cs
using UnityEngine;

/// <summary>
/// マーカー名に対応する Prefab を ScriptableObject から取得し、
/// インスタンス生成と共通初期化（ジェスチャーハンドラの追加）を行う Factory クラス。
/// </summary>
public static class ARObjectFactory
{
    private static ARPrefabRegistry registry;

    /// <summary>
    /// Factory の初期化。シーン起動時に呼び出し、Registry を設定します。
    /// </summary>
    public static void Initialize(ARPrefabRegistry r)
    {
        registry = r;
    }

    /// <summary>
    /// 指定マーカー名に対応する Prefab を生成し、
    /// 必要なハンドラコンポーネントを自動アタッチします。
    /// </summary>
    /// <param name="markerName">Registry に登録されたマーカー名</param>
    /// <param name="parent">追跡対象画像の Transform</param>
    /// <returns>生成した GameObject インスタンス、または null</returns>
    public static GameObject Create(string markerName, Transform parent)
    {
        if (registry == null)
        {
            Debug.LogError("ARObjectFactory: Registry not initialized");
            return null;
        }

        // Registry から Prefab を取得
        GameObject prefab = registry.GetPrefab(markerName);
        if (prefab == null)
        {
            Debug.LogWarning($"ARObjectFactory: No prefab found for marker '{markerName}'");
            return null;
        }

        // Prefab をインスタンス化
        GameObject instance = Object.Instantiate(prefab, parent.position, parent.rotation, parent);

        // ジェスチャーハンドラを自動アタッチ
        if (instance.GetComponent<PinchToScaleHandler>() == null)
            instance.AddComponent<PinchToScaleHandler>();
        if (instance.GetComponent<SwipeToRotateHandler>() == null)
            instance.AddComponent<SwipeToRotateHandler>();

        // GestureHandlerRegistrar は不要になったため削除

        return instance;
    }
}