using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffectCtrl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;

    static CameraEffectCtrl _instance;

    CinemachineBasicMultiChannelPerlin _noise;

    float _cpow = 0;
    float _opow = 0;
    float _tpow = 0;
    float _timer = 1;

    void Awake()
    {
        _instance = this;
        _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        _timer += Time.unscaledDeltaTime * 5.0f;
        _cpow = Mathf.Lerp(_opow, _tpow, _timer);
        _noise.m_AmplitudeGain = _cpow;
    }

    void CamShake(float pow, float stime)
    {
        _opow = _cpow;
        _tpow = pow;
        _timer = stime;
    }

    static public void Shake(float pow, float stime)
    {
        if (!Setting.HasCameraShake) return;
        _instance.CamShake(pow, stime);
    }
}
