using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ダメージUIクラス
/// </summary>
public class Damage : MonoBehaviour
{
    [SerializeField] AnimationCurve _animCurve = null;

    RectTransform _rect = null;
    Text _text;
    float _timer = 0.0f;
    Vector3 _movVec = new Vector3(0, 0.3f, 0);
    Vector3 _mov = Vector3.zero;
    Vector2 _random;
    Character _target = null;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _text = GetComponent<Text>();
        _random = new Vector2(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-5.0f, 5.0f));
    }

    /// <summary>
    /// 動きの管理
    /// </summary>
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        _timer += Time.unscaledDeltaTime;

        _mov += _movVec * _animCurve.Evaluate(_timer);
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.HeadPos) + _random;
        _rect.position += _mov;

        if (_timer > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="go">ダメージを受けたオブジェクト</param>
    /// <param name="dmg">ダメージ値</param>
    public void Set(GameObject go, int dmg)
    {
        _target = go.GetComponent<Character>();

        _text.text = dmg.ToString();
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="col">ダメージの色</param>
    public void SetColor(Color col)
    {
        _text.color = col;
    }
}
