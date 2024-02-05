using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 _initialPos;
    Rigidbody _rbody;
    float _moveTimer = 0.0f;

    public Vector3 HeadPos => _head.transform.position;

    public int HP => _hp;
    public int MaxHP { get; protected set; }
    LifeChange _lifeChange;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        MaxHP = _hp;
        _initialPos = transform.position;
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
        _hp -= dmg;
        _lifeChange?.Invoke(dmg);

        if (_hp <= 0)
        {
            GameController.Instance.GameOver(_charId);
        }
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
        _head.transform.position = new Vector3(transform.position.x, 2, transform.position.z);

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

        //キー入力
        if (_charId == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Move(new Vector2(0, 1));
            if (Input.GetKeyDown(KeyCode.DownArrow)) Move(new Vector2(0, -1));
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(new Vector2(-1, 0));
            if (Input.GetKeyDown(KeyCode.RightArrow)) Move(new Vector2(1, 0));
        }

        if (_charId == 2)
        {
            if (Input.GetKeyDown(KeyCode.W)) Move(new Vector2(0, 1));
            if (Input.GetKeyDown(KeyCode.S)) Move(new Vector2(0, -1));
            if (Input.GetKeyDown(KeyCode.A)) Move(new Vector2(-1, 0));
            if (Input.GetKeyDown(KeyCode.D)) Move(new Vector2(1, 0));
        }
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="dir">移動方向</param>
    public void Move(Vector2 dir)
    {
        Vector3 pos = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(pos.x, 0.3f, pos.z), new Vector3(dir.x, 0, dir.y), out hit))
        {
            //壁に近い場合はダメ
            if (hit.distance < 2.0f)
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

    private void OnCollisionEnter(Collision collision)
    {

    }
}
