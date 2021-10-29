using UnityEngine;
using System.Collections;

/// <summary>
/// ダメージ数字をポップアップさせる
/// 
/// NOTE: 3D座標から2D座標に変換している
/// </summary>
public class DamagePopup : MonoBehaviour
{
    [SerializeField] Damage DamageTemplate = null;
    static DamagePopup Instance = null;
    
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ダメージ表示
    /// </summary>
    /// <param name="go">ダメージを受けたオブジェクト</param>
    /// <param name="dmg">ダメージ値</param>
    /// <param name="col">色</param>
    static public void Pop(GameObject go, int dmg, Color col)
    {
        if (!Setting.HasDamageUI) return;

        var dgo = Instantiate(Instance.DamageTemplate.gameObject, Instance.transform.parent);
        var damage = dgo.GetComponent<Damage>();
        //damage.RectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, go.transform.position);
        damage.Set(go, dmg);
        damage.SetColor(col);
    }
}
