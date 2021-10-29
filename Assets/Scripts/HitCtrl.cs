using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCtrl : MonoBehaviour
{
    [SerializeField] float _power = 5.0f;
    [SerializeField] int _combo = 0;
    [SerializeField] float _comboSpan = 0.5f;
    [SerializeField] Cinemachine.CinemachineImpulseSource _impluseSource;
    float _stopTime = 0;
    float _frameTimer = 0;
    float _camTime = 0;
    float _shakeTimer = 0;
    float _timeScale;
    Collider _collider;

    bool _isHitStop = false;

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();
    }
    
    void Update()
    {
        if(_isHitStop)
        {
            HitStop();
            _isHitStop = false;
        }
        if (Time.timeScale == 0 && _frameTimer < _stopTime)
        {
            _frameTimer += Time.unscaledDeltaTime;
            if(_frameTimer >= _stopTime)
            {
                Time.timeScale = _timeScale;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Character")) return;

        EffectManager.PlayEffect("Hit", this.transform);
        //ここ、本格的に実装する際はキャッシュするなど、工夫は必要
        Character chara = other.gameObject.GetComponent<Character>();
        chara.Damage(100);
        chara.HitBack(_power);

        CamShake();
        _isHitStop = true;
    }

    void CamShake()
    {
        if (!Setting.HasCameraShake) return;

        Debug.Log("here");
        _impluseSource?.GenerateImpulse();
    }

    void HitStop()
    {
        if (!Setting.HasHitStop) return;

        _stopTime = _power * 1.0f / 24.0f;

        //止める
        _frameTimer = 0;
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
    }
}
