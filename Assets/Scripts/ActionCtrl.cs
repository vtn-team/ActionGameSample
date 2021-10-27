using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCtrl : MonoBehaviour
{
    public enum AttackLayer
    {
        InFight,
        Airial,
        LongRange
    }

    public enum ActionType
    {
        Animation,
        CreateObject,
    }

    [Serializable]
    class Attack
    {
        public string Name;
        public float Power;
        public int Step;
        public AttackLayer Layer;
        public ActionType ActType;
        public string ActionTargetName;
    }
    [SerializeField] List<Attack> _attacks;
    [SerializeField] AnimationCtrl _animCtrl;

    HitCtrl _hitCtrl;

    const string STANDBY_MOTION = "Sword01";
    float _stopTime = 0;
    float _frameTimer = 0;
    float _camTime = 0;
    float _shakeTimer = 0;
    float _timeScale = 0;
    bool _isHitStop = false;
    bool _isActionPlaying = false;
    float _waitTimer = 0;
    bool _reserveAction = false;

    List<Character> _targetChars = new List<Character>();
    int _comboStep = 0;
    AttackLayer _comboLayer;

    private void Awake()
    {
        _hitCtrl = GetComponentInChildren<HitCtrl>();
        _timeScale = Time.timeScale;
    }

    void Update()
    {
        if(_isHitStop && Time.timeScale == _timeScale)
        {
            _frameTimer = 0;
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            _isHitStop = false;
        }

        if (Time.timeScale == 0 && _frameTimer < _stopTime)
        {
            _frameTimer += Time.unscaledDeltaTime;
            if (_frameTimer >= _stopTime)
            {
                Time.timeScale = _timeScale;
            }
        }

        if (_camTime > 0 && _shakeTimer < _camTime)
        {
            _shakeTimer += Time.deltaTime;
            if (_shakeTimer >= _camTime)
            {
                CameraEffectCtrl.Shake(0, 1);
                _camTime = 0;
            }
        }

        if (_waitTimer > 0.0f)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0.0f)
            {
                _waitTimer = 0.0f;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("here");
            _reserveAction = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (_isActionPlaying || _waitTimer > 0.0f)
            {
                return;
            }
            
            NextAction(0, AttackLayer.LongRange);
        }

        //WDebug.Log(_reserveAction);
        if (_reserveAction && !_isActionPlaying)
        {
            Debug.Log("come2");
            NextAction(_comboStep, _comboLayer);
            _comboStep++;
            if (_comboStep >= 3)
            {
                _comboStep = 0;
            }

            _reserveAction = false;
        }
        
        if (!_reserveAction && _waitTimer <= 0.0f)
        {
            Debug.Log("come1");
            _comboStep = 0;
            _animCtrl.Play(STANDBY_MOTION);
        }
    }

    void NextAction(int step, AttackLayer layer)
    {
        int actId = -1;
        Attack attack = _attacks[0];
        for (int i=0; i<_attacks.Count; ++i)
        {
            if (_attacks[i].Step != step) continue;
            if (_attacks[i].Layer != layer) continue;

            attack = _attacks[i];
            actId = i;
            break;
        }
        //var atks = _attacks.Where(a => a.Step == step && a.Layer == layer);
        if (actId == -1)
        {
            Debug.LogWarning(String.Format("attack not found. {0}/{1}", step, layer));
            return;
        }

        switch(attack.ActType)
        {
            case ActionType.Animation:
                _hitCtrl.SetCtrl(this, actId);
                _isActionPlaying = true;
                _waitTimer = 1.0f;
                _animCtrl.Play(attack.ActionTargetName);
                _animCtrl.SetPlaybackDelegate(() => {
                    _isActionPlaying = false;
                });
                break;

            case ActionType.CreateObject:
                GameObject _shooter = Resources.Load<GameObject>(attack.ActionTargetName);
                var go = Instantiate(_shooter, transform.position + transform.forward * 2, transform.rotation);
                var hitObj = go.GetComponentInChildren<HitCtrl>();
                hitObj.SetCtrl(this, actId);
                break;
        }
    }

    public void HitCallback(Character c, int actId)
    {
        Debug.Log(actId);
        Debug.Log(_attacks[actId].Power);
        

        c.Damage(100);
        c.HitBack(_attacks[actId].Power);

        CamShake(_attacks[actId].Power);
        
        HitStop(_attacks[actId].Power);

        _targetChars.Add(c);
    }
    
    void CamShake(float pow)
    {
        _shakeTimer = 0;
        _camTime = pow * 0.1f / 24.0f;
        CameraEffectCtrl.Shake(pow, 0);
    }

    void HitStop(float pow)
    {
        if (!Setting.HasHitStop) return;

        _stopTime = pow * 1.0f / 24.0f;

        //止める
        _isHitStop = true;
    }
}
