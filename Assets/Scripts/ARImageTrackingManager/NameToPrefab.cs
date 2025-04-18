// File: NameToPrefab.cs
using System;
using UnityEngine;

/// <summary>
/// ARリファレンス画像の名前と、それを検出したときに生成するプレハブを対応付けるクラス。
/// Serializable属性が付いているため、Unityインスペクタ上で編集が可能。
/// </summary>
[Serializable]
public class NameToPrefab
{
    /// <summary>
    /// リファレンス画像の名前（Reference Image Libraryで設定した名前と一致させること）。
    /// </summary>
    public string name;

    /// <summary>
    /// 画像検出時にInstantiateして表示するプレハブ。
    /// </summary>
    public GameObject prefab;
}
