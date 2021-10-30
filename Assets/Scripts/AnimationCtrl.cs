using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Animation管理+ヘルパクラス
/// 
/// NOTE: Motion Blendを使用するものは、適宜派生させること
/// NOTE: 汎用化させるよりは、仕様が同じものをまとめて特化させた方がオーバヘッドが少ないので、そうした方がよい(Player、NPC、Enemy、Bossなど)
/// </summary>
public class AnimationCtrl : MonoBehaviour
{
    public struct AnimStack
    {
        public string stateName;
        public float duration;
    }
    public delegate void Callback(int param);

    [SerializeField] protected Animator _animator;
    [SerializeField] bool _isActiveStart = true;

    int _targetLayer = 0;
    float _duration = 0.0f;
    protected Callback _pbkCallback = null;
    protected Callback _evtCallback = null;

    Queue<AnimStack> _animQueue = new Queue<AnimStack>();

    private void Awake()
    {
        if (!_isActiveStart)
        {
            DisActive();
        }
    }

    private void Start()
    {
        if (_animator == null) Debug.LogError("Animator Not Found:" + gameObject.name);
    }

    public virtual void Active()
    {
        _animator.enabled = true;
    }

    public virtual void DisActive()
    {
        _animator.enabled = false;
    }

    /// <summary>
    /// Animatorを強制的に更新する
    /// </summary>
    public void ForceUpdate()
    {
        _animator.Update(0.0f);
    }

    /// <summary>
    /// Animation再生時間を調整する
    /// </summary>
    public void SetNormalizedTime(float time, int layer = 0)
    {
        _animator.Play(0, layer, time);
        _animator.Update(0.0f);
    }

    /// <summary>
    /// アニメーションを切り替える
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="dur">遅延時間</param>
    /// <returns></returns>
    public virtual void Play(string stateName, float dur = 0.0f)
    {
        Active();
        _animator.CrossFade(stateName, dur);
        _duration = dur;
        Debug.Log("Play:" + stateName);
    }

    /// <summary>
    /// 再生が終わったら次のアニメーションを流す
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="dur">遅延時間</param>
    /// <returns></returns>
    public void PlayQueue(string stateName, float dur = 0.0f)
    {
        _animQueue.Enqueue(new AnimStack() { stateName = stateName, duration = dur });
        Debug.Log("PlayQueue:" + stateName);
    }

    /// <summary>
    /// 対象レイヤーのアニメーションが再生中かどうかを返す
    /// </summary>
    /// <param name="layer">レイヤーID、基本は0で省略すると0が入る</param>
    /// <returns></returns>
    public bool IsPlayingAnimation(int layer = 0)
    {
        if (_duration > 0.0f) return true;
        var state = _animator.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true; //ループは永遠にtrue帰る
        return state.normalizedTime < 1.0f;
    }

    /*
    /// <summary>
    /// 対象レイヤーのアニメーションがループアニメかどうかを返す
    /// </summary>
    /// <param name="layer">レイヤーID、基本は0で省略すると0が入る</param>
    /// <returns></returns>
    public bool IsLoop(int layer = 0)
    {
        return _animator.GetCurrentAnimatorStateInfo(layer).loop;
    }
    */

    /// <summary>
    /// イベントコールバックを設定する
    /// </summary>
    /// <param name="cb">コールバック</param>
    /// <param name="target">監視対象レイヤー</param>
    public void SetEventDelegate(Callback cb)
    {
        _evtCallback = cb;
    }

    /// <summary>
    /// 再生終了時のコールバックを設定する
    /// </summary>
    /// <param name="cb">コールバック</param>
    /// <param name="target">監視対象レイヤー</param>
    public void SetPlaybackDelegate(Callback cb, int target = 0)
    {
        _pbkCallback = cb;
        _targetLayer = target;
    }

    /// <summary>
    /// アニメーションからのイベントを通知する
    /// </summary>
    /// <param name="evntType"></param>
    public void AnimationEvent(int evtType)
    {
        _evtCallback?.Invoke(evtType);
    }


    //将来的にコルーチンにする
    protected virtual void Update()
    {
        if (_animator == null) return;

        if(_duration > 0.0f)
        {
            _duration -= Time.deltaTime;
        }

        if (_pbkCallback != null)
        {
            if (!IsPlayingAnimation(_targetLayer))
            {
                Callback back = _pbkCallback;
                _pbkCallback = null;
                back(_targetLayer);
            }
        }

        if (_animQueue.Count > 0)
        {
            //0でいいのかはおいおい考える
            if (!IsPlayingAnimation(0))
            {
                var anim = _animQueue.Dequeue();
                Play(anim.stateName, anim.duration);
            }
        }
    }
}
