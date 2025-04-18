// File: ImageTrackingObjectManagerEditor.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// ImageTrackingObjectManager用のカスタムエディタ。
/// Inspector上でARTrackedImageManagerの設定、
/// 画像名⇔プレハブマッピングのUIを提供する。
/// </summary>
[CustomEditor(typeof(ImageTrackingObjectManager))]
public class ImageTrackingObjectManagerEditor : Editor
{
    // Marker To Prefab の折りたたみ制御
    private bool showNameToPrefabMappings = true;

    public override void OnInspectorGUI()
    {
        // デフォルトのInspector描画
        DrawDefaultInspector();

        // 対象となるマネージャインスタンスを取得
        var manager = (ImageTrackingObjectManager)target;

        // ARTrackedImageManagerをドラッグ＆ドロップで設定させる
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

        // 未設定時のエラーメッセージ
        if (manager.arTrackedImageManager == null)
        {
            EditorGUILayout.HelpBox("Tracked Image Manager が未設定です。", MessageType.Error);
            return;
        }

        // リファレンスライブラリとマッピングリストを同期
        manager.UpdateNameToPrefabMappings();

        // 画像が1つもない場合の警告
        if (manager.markerNameToPrefab.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "Reference Image Library に画像が設定されていません。",
                MessageType.Warning
            );
            return;
        }

        // マッピングリストの折りたたみ表示
        showNameToPrefabMappings = EditorGUILayout.Foldout(
            showNameToPrefabMappings,
            new GUIContent("Marker To Prefab", "リファレンス画像名とプレハブのマッピング"),
            true
        );

        if (showNameToPrefabMappings)
        {
            // 各マッピングを1行ずつ編集可能
            foreach (var pair in manager.markerNameToPrefab)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.LabelField(pair.name, GUILayout.Width(150));
                GameObject newPrefab = (GameObject)
                    EditorGUILayout.ObjectField(pair.prefab, typeof(GameObject), true);
                if (newPrefab != pair.prefab)
                {
                    pair.prefab = newPrefab;
                    // 変更を永続化
                    EditorUtility.SetDirty(manager);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif