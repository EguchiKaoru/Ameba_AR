using UnityEngine;

public class ImageTrackingController : MonoBehaviour
{
    // Inspector から紐づける ScriptableObject アセット用のフィールド
    [SerializeField]
    private ARPrefabRegistry prefabRegistry;

    // シーンが読み込まれてこの GameObject が有効化された直後に呼ばれる
    void Awake()
    {
        // Factory の内部に、Inspector で設定した registry を渡して初期化
        ARObjectFactory.Initialize(prefabRegistry);
    }
}
