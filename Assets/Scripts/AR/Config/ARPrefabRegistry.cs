// File: ARPrefabRegistry.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ARマーカー名とPrefabを一元管理するScriptableObjectクラス。
/// Unityエディタのアセットとして保存し、Inspector上で対応関係を設定できる。
/// </summary>
[CreateAssetMenu(fileName = "ARPrefabRegistry", menuName = "AR/Prefab Registry")]
public class ARPrefabRegistry : ScriptableObject
{
    /// <summary>
    /// マーカー名とPrefabの組み合わせを表すエントリ。
    /// Serializable属性によりInspectorで編集可能。
    /// </summary>
    [System.Serializable]
    public class Entry
    {
        /// <summary>
        /// ARマーカー（QRコードやAR Reference Image）の識別用の文字列。
        /// ImageTrackingやQR読み取り時に返される名称と一致させる。
        /// </summary>
        public string markerName;

        /// <summary>
        /// 該当マーカー検出時にInstantiateするPrefab。
        /// PrefabはプロジェクトのAssetsフォルダに保存されたGameObject。
        /// </summary>
        public GameObject prefab;
    }

    /// <summary>
    /// マーカー名とPrefabのエントリ一覧。
    /// Inspector上でサイズ変更や要素追加が可能。
    /// </summary>
    public List<Entry> entries = new List<Entry>();

    /// <summary>
    /// 指定したマーカー名に対応するPrefabを返却するメソッド。
    /// 該当エントリが存在しない場合はnull。
    /// </summary>
    /// <param name="name">検索するマーカー名</param>
    /// <returns>対応するPrefab、またはnull</returns>
    public GameObject GetPrefab(string name)
    {
        // entriesリストからmarkerNameが一致する最初のEntryを検索
        Entry entry = entries.Find(x => x.markerName == name);

        // 見つかった場合はそのPrefabを返し、見つからなければnullを返す
        if (entry != null)
        {
            return entry.prefab;
        }
        else
        {
            Debug.LogWarning($"ARPrefabRegistry: Prefab not found for marker '{name}'");
            return null;
        }
    }
}
