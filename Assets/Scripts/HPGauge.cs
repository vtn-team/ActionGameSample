using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField] Character _char;
    [SerializeField] Image _hpbar;

    RectTransform _rect;
    Vector2 _offset = new Vector2(0, 100);

    void Start()
    {
        _char.SetLifeChangeDelegate(UpdateLife);
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        //‚ ‚ñ‚Ü‚è‚æ‚­‚È‚¢
        _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _char.transform.position) + _offset;
    }

    void UpdateLife(int diff)
    {
        _hpbar.fillAmount = ((float)_char.HP / _char.MaxHP);
    }
}
