using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃ヒット時のコントローラ
/// </summary>
public class HitCtrl : MonoBehaviour
{
    [SerializeField] float _power = 5.0f;
    [SerializeField] Cinemachine.CinemachineImpulseSource _impluseSource = null;
    float _stopTime = 0;
    float _frameTimer = 0;
    float _timeScale;
    Collider _collider;

    bool _isHitStop = false;

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();
    }
    
    /// <summary>
    /// ヒットストップ管理
    /// NOTE: UpdateはTimescaleの影響を受けない。
    /// </summary>
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

    /// <summary>
    /// ヒット時のコールバック
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    void CamShake()
    {
        if (!Setting.HasCameraShake) return;

        Debug.Log("here");
        _impluseSource?.GenerateImpulse();
    }

    /// <summary>
    /// ヒットストップの指令を出す
    /// </summary>
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
