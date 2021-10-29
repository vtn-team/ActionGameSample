using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    [System.Serializable]
    class Effect
    {
        public string _name = "";
        public GameObject _effect = null;
    }

    [SerializeField] List<Effect> _hitEffect = new List<Effect>();

    Dictionary<string, GameObject> _effectDic = new Dictionary<string, GameObject>();
    static EffectManager _instance;
    private void Awake()
    {
        _instance = this;
        _hitEffect.ForEach(ef => _effectDic.Add(ef._name, ef._effect));
    }

    static public GameObject PlayEffect(string key, Transform t)
    {
        if (!Setting.HasParticleEffect) return null;

        if (!_instance._effectDic.ContainsKey(key)) return null;
        return Instantiate(_instance._effectDic[key], t.position, t.rotation);
    }
}
