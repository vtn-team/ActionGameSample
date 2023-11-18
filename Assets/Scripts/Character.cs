using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// 簡単なキャラクター管理
/// </summary>
public class Character : MonoBehaviour
{
    public delegate void LifeChange(int diff);

    [SerializeField] int _charId = 1; //変えないこと
    [SerializeField] int _hp = 100;
    [SerializeField] int _criRate = 80;
    [SerializeField] float _moveInterval = 1.0f;
    [SerializeField] GameObject _head;
    [SerializeField] Shooter _shooter;
    [SerializeField] List<BulletData> _bulletData = new List<BulletData>();
    [SerializeField] Slider _hpBar;
    public List<BulletData> Bullets => _bulletData;

    Vector3 _initialPos;
    Rigidbody _rbody;
    float _moveTimer = 0.0f;
    float _stopTimer;
    int _bulletIndex = 0;
    bool _isStop = false;
    float _timeScale;

    public Vector3 HeadPos => _head.transform.position;
    public int ID => _charId;

    public int HP
    {
        get => _hp;
        set
        {
            _hpBar.value = value;
            _hp = value;
            if (_hp <= 0)
            {
                DestroyAction();
            }
        }
    }
    public int Index
    {
        get => _bulletIndex;
        set
        {
            if (_bulletIndex > _bulletData.Count) return;
            _bulletIndex = value;
        }
    }
    public int MaxHP { get; protected set; }
    LifeChange _lifeChange;

    [Serializable]
    public class BulletData
    {
        [SerializeField]
        GameObject _bulletPrefab;
        public GameObject BulletPrefab => _bulletPrefab;

        [SerializeField]
        int _damage;
        public int Damage => _damage;

        [SerializeField]
        float _range;
        public float Range => _range;
    }

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        MaxHP = _hp;
        _initialPos = transform.position;
        _shooter.Set(this);
        _hpBar.maxValue = MaxHP;
        _hpBar.value = MaxHP;
    }

    public void SetLifeChangeDelegate(LifeChange dlg)
    {
        _lifeChange += dlg;
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="dmg"></param>
    public void Damage(int dmg)
    {
        DamagePopup.Pop(gameObject, dmg, Color.red);
        HP -= dmg;
        _lifeChange?.Invoke(dmg);
    }

    /// <summary>
    /// ノックバックする
    /// </summary>
    /// <param name="pow">ノックバックする威力</param>
    public void HitBack(float pow)
    {
        if (!Setting.HasKnockback) return;

        //都合に応じて関数を変えること
        _rbody.AddForce(-transform.forward * pow, ForceMode.Impulse);
    }

    void Update()
    {
        if (!_isStop)
        {
            if (GameController.IsGameOver) return;

            if (transform.position.y < -3)
            {
                Damage(MaxHP);
                return;
            }

            if (_moveTimer > 0.0f)
            {
                _moveTimer -= Time.unscaledDeltaTime;
                return;
            }

            //if(Gamepad.current.leftStick.)
            //キー入力
            if (_charId == 1 && TurnManager.Instance.IsTurn)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) Move(new Vector2(0, 1));
                if (Input.GetKeyDown(KeyCode.DownArrow)) Move(new Vector2(0, -1));
                if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(new Vector2(-1, 0));
                if (Input.GetKeyDown(KeyCode.RightArrow)) Move(new Vector2(1, 0));
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    _shooter.Shot();
                    TurnManager.Instance.MoveCount--;
                }
            }

            if (_charId == 2 && !TurnManager.Instance.IsTurn)
            {
                if (Input.GetKeyDown(KeyCode.W)) Move(new Vector2(0, 1));
                if (Input.GetKeyDown(KeyCode.S)) Move(new Vector2(0, -1));
                if (Input.GetKeyDown(KeyCode.A)) Move(new Vector2(-1, 0));
                if (Input.GetKeyDown(KeyCode.D)) Move(new Vector2(1, 0));
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _shooter.Shot();
                    TurnManager.Instance.MoveCount--;
                }
            }
        }
        else
        {
            _stopTimer += Time.unscaledDeltaTime;
            if(_stopTimer > 1f)
            {
                if(Time.timeScale > _timeScale)
                {
                    Time.timeScale = _timeScale;
                    GameController.Instance.GameOver(_charId);
                    _isStop = false;
                }
                Time.timeScale += Time.unscaledDeltaTime; ;
            }
        }
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="dir">移動方向</param>
    public void Move(Vector2 dir)
    {
        TurnManager.Instance.MoveCount--;
        Vector3 pos = transform.position;

        Debug.DrawLine(pos, new Vector3(dir.x, pos.y, dir.y), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(pos, new Vector3(dir.x, 0, dir.y), out hit))
        {
            if (hit.distance < 2.0f && hit.collider.gameObject.layer == 10)
            {
                pos = transform.position + (Vector3.up * 6);
            }
            //壁に近い場合はダメ
            if (hit.distance < 2.0f && hit.collider.gameObject.layer != 9 && hit.collider.gameObject.layer != 10)
            {
                Debug.Log(hit);
                return;
            }
        }
        //ノックバック分丸める
        float x = Mathf.Round(pos.x - _initialPos.x);
        x = x - x % 2.0f;

        float y = Mathf.Round(pos.z - _initialPos.z);
        y = y - y % 2.0f;

        //移動
        transform.position = _initialPos + new Vector3(x + dir.x * 2.0f, pos.y, y + dir.y * 2.0f);

        _moveTimer = _moveInterval;
    }

    private void DestroyAction()
    {
        _timeScale = Time.timeScale;
        _rbody.AddForce((-transform.forward + Vector3.up) * 30, ForceMode.Impulse);
        Time.timeScale = 0;
        _isStop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<ItemBase>().Use(this);
            Destroy(other.gameObject);
        }
    }
}
