using UnityEngine;

public static class ARObjectFactory
{
    private static ARPrefabRegistry registry;

    public static void Initialize(ARPrefabRegistry r)
    {
        registry = r;
    }

    public static GameObject Create(string markerName, Transform parent)
    {
        if (registry == null)
        {
            Debug.LogError("Factory: Registry not initialized");
            return null;
        }
        var prefab = registry.GetPrefab(markerName);
        if (prefab == null) return null;

        var instance = Object.Instantiate(prefab, parent.position, parent.rotation, parent);
        // 共通初期化例：必ずジェスチャースクリプトを付ける
        if (instance.GetComponent<PinchToScale>() == null)
            instance.AddComponent<PinchToScale>();
        if (instance.GetComponent<SwipeToRotate>() == null)
            instance.AddComponent<SwipeToRotate>();
        return instance;
    }
}
