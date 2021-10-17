using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damage : MonoBehaviour
{
    [SerializeField] AnimationCurve _animCurve = null;

    RectTransform _rect = null;
    Text _text;
    float _timer = 0.0f;
    Vector3 _movVec = new Vector3(0, 0.3f, 0);
    Vector3 _mov = Vector3.zero;
    Vector2 _random;
    GameObject _target = null;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _text = GetComponent<Text>();
        _random = new Vector2(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-5.0f, 5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        _timer += Time.unscaledDeltaTime;

        _mov += _movVec * _animCurve.Evaluate(_timer);
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position) + _random;
        _rect.position += _mov;

        if (_timer > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Set(GameObject go, int dmg)
    {
        _target = go;
        _text.text = dmg.ToString();
    }

    public void SetColor(Color col)
    {
        _text.color = col;
    }
}
