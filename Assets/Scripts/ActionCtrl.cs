using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行動管理クラス
/// </summary>
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

    /// <summary>
    /// 行動定義
    /// </summary>
    [Serializable]
    class Attack
    {
        public string Name = "";
        public float Power = 1;
        public int Step = 0;
        public AttackLayer Layer = AttackLayer.InFight;
        public ActionType ActType = ActionType.Animation;
        public string ActionTargetName = "";
    }
    [SerializeField] List<Attack> _attacks = new List<Attack>();
    [SerializeField] AnimationCtrl _animCtrl = null;
    [SerializeField] Cinemachine.CinemachineImpulseSource _impluseSource = null;

    HitCtrl _hitCtrl;

    const string STANDBY_MOTION = "Sword01";
    float _stopTime = 0;
    float _frameTimer = 0;
    float _timeScale = 0;
    bool _isHitStop = false;
    bool _isActionPlaying = false;
    float _waitTimer = 0;
    bool _reserveAction = false;

    List<Character> _targetChars = new List<Character>();
    int _comboStep = 0;
    AttackLayer _comboLayer = AttackLayer.InFight;

    private void Awake()
    {
        _hitCtrl = GetComponentInChildren<HitCtrl>();
        _animCtrl?.SetEventDelegate(EventCall);
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
            _comboStep = 0;
            _targetChars.Clear();
            _animCtrl.Play(STANDBY_MOTION);
        }
    }

    /// <summary>
    /// 行動決定
    /// </summary>
    /// <param name="step">ステップ</param>
    /// <param name="layer">行動レイヤ</param>
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
                _animCtrl.SetPlaybackDelegate((int targetLayer) => {
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

    /// <summary>
    /// イベントコール
    /// </summary>
    /// <param name="evtType"></param>
    public void EventCall(int evtType)
    {
        switch(evtType)
        {
            case 1:
                _targetChars.ForEach(c => c.ShootUp());
                break;
        }
    }

    /// <summary>
    /// HitCtrlからのコールバック
    /// </summary>
    /// <param name="c"></param>
    /// <param name="actId"></param>
    public void HitCallback(Character c, int actId)
    {
        /*
        Debug.Log(actId);
        Debug.Log(_attacks[actId].Power);
        */

        c.Damage(100);
        c.HitBack(_attacks[actId].Power);

        CamShake();
        
        HitStop(_attacks[actId].Power);

        if (!_targetChars.Contains(c))
        {
            _targetChars.Add(c);
        }
    }
    
    /// <summary>
    /// カメラシェイク
    /// </summary>
    /// <param name="pow"></param>
    void CamShake()
    {
        if (!Setting.HasCameraShake) return;

        _impluseSource?.GenerateImpulse();
    }

    /// <summary>
    /// ヒットストップ
    /// </summary>
    /// <param name="pow"></param>
    void HitStop(float pow)
    {
        if (!Setting.HasHitStop) return;

        _stopTime = pow * 1.0f / 24.0f;

        //止める
        _isHitStop = true;
    }
}
