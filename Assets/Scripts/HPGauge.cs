using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField] float _effectStopTime = 0.5f;
    [SerializeField] float _effectTotalTime = 0.8f;
    [SerializeField] Character _char;
    [SerializeField] Image _hpbar;
    [SerializeField] Image _effect;

    RectTransform _rect;
    Vector2 _offset = new Vector2(0, 100);

    float _prevRate;
    float _updTimer = 0.0f;

    void Start()
    {
        _char.SetLifeChangeDelegate(UpdateLife);
        _rect = GetComponent<RectTransform>();
        _prevRate = 1.0f;
    }

    void Update()
    {
        //‚ ‚ñ‚Ü‚è‚æ‚­‚È‚¢
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _char.transform.position) + _offset;

        //
        if (_updTimer > 0.0f)
        {
            _updTimer -= Time.deltaTime;
            if ((_effectTotalTime - _updTimer) > _effectStopTime)
            {
                float time = (_effectTotalTime - _updTimer) / (_effectTotalTime - _effectStopTime);
                if (time > 1.0f) time = 1.0f;

                _effect.fillAmount = Mathf.Lerp(_prevRate, (float)_char.HP / _char.MaxHP, easeInQuint(time));
            }

            if (_updTimer < 0.0f)
            {
                _prevRate = (float)_char.HP / _char.MaxHP;
                _updTimer = 0.0f;
            }
        }
    }

    void UpdateLife(int diff)
    {
        _hpbar.fillAmount = ((float)_char.HP / _char.MaxHP);
        _updTimer = _effectTotalTime;
    }

    float easeInQuint(float t)
    {
        return 1 - Mathf.Pow(1 - t, 2);
    }
}
