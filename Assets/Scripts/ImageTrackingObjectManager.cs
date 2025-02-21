using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class NameToPrefab
{
    public string name;
    public GameObject prefab;
}

public class ImageTrackingObjectManager : MonoBehaviour
{
    [HideInInspector]
    public ARTrackedImageManager arTrackedImageManager;

    [HideInInspector, SerializeField]
    public List<NameToPrefab> markerNameToPrefab = new List<NameToPrefab>();

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            var name = trackedImage.referenceImage.name;
            var prefab = markerNameToPrefab.Find(x => x.name == name)?.prefab;
            if (prefab != null)
            {
                var instance = Instantiate(
                    prefab,
                    trackedImage.transform.position,
                    trackedImage.transform.rotation,
                    trackedImage.transform
                );
            }
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                trackedImage.gameObject.SetActive(true);
            }
            else if (trackedImage.trackingState == TrackingState.Limited)
            {
                trackedImage.gameObject.SetActive(false);
            }
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            trackedImage.gameObject.SetActive(false);
        }
    }

    bool HasNameInReferenceLibrary(IReferenceImageLibrary library, string name)
    {
        for (int i = 0; i < library.count; i++)
        {
            if (library[i].name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateNameToPrefabMappings()
    {
        if (arTrackedImageManager == null || arTrackedImageManager.referenceLibrary == null)
        {
            return;
        }

        // リストを逆順で走査しながら削除
        for (int i = markerNameToPrefab.Count - 1; i >= 0; i--)
        {
            if (!HasNameInReferenceLibrary(arTrackedImageManager.referenceLibrary, markerNameToPrefab[i].name))
            {
                markerNameToPrefab.RemoveAt(i);
            }
        }

        // 追加する処理（そのまま）
        for (int i = 0; i < arTrackedImageManager.referenceLibrary.count; i++)
        {
            var name = arTrackedImageManager.referenceLibrary[i].name;
            if (!markerNameToPrefab.Exists(x => x.name == name))
            {
                markerNameToPrefab.Add(new NameToPrefab { name = name });
            }
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ImageTrackingObjectManager))]
public class ImageTrackingObjectManagerEditor : Editor
{
    bool showNameToPrefabMappings = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var manager = (ImageTrackingObjectManager)target;
        ARTrackedImageManager newManager = (ARTrackedImageManager)
            EditorGUILayout.ObjectField(
                "AR Tracked Image Manager",
                manager.arTrackedImageManager,
                typeof(ARTrackedImageManager),
                true
            );
        if (newManager != manager.arTrackedImageManager)
        {
            manager.arTrackedImageManager = newManager;
        }
        if (manager.arTrackedImageManager == null)
        {
            EditorGUILayout.HelpBox("Tracked Image Manager is required.", MessageType.Error);
        }
        else
        {
            manager.UpdateNameToPrefabMappings();
            if (manager.markerNameToPrefab.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "There are no reference images in the Reference Image Library.",
                    MessageType.Warning
                );
            }
            else
            {
                showNameToPrefabMappings = EditorGUILayout.Foldout(
                    showNameToPrefabMappings,
                    new GUIContent("Marker To Prefab", "The mapping from marker name to prefab."),
                    true
                );
                if (showNameToPrefabMappings)
                {
                    foreach (var pair in manager.markerNameToPrefab)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField(pair.name);
                        var newPrefab = (GameObject)
                            EditorGUILayout.ObjectField(pair.prefab, typeof(GameObject), true);
                        if (newPrefab != pair.prefab)
                        {
                            pair.prefab = newPrefab;
                            EditorUtility.SetDirty(manager);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}
#endif
