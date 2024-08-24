using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static TurnManager _instance;
    public static TurnManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TurnManager>();
                if (_instance == null)
                {
                    Debug.LogError("オブジェクトが見つかりません");
                }
            }
            return _instance;
        }
    }
    [SerializeField]
    GameObject _player1;
    [SerializeField]
    GameObject _player2;

    [SerializeField]
    int _moveCount;
    public int MoveCount
    {
        get => _moveCount;
        set
        {
            _moveCount = value;
            if(_moveCount <= 0)
            {
                TurnChange();
            }
        }
    }

    bool _isTurn;
    public bool IsTurn => _isTurn;
    int _defaultMove;

    private void Awake()
    {
        var random = Random.Range(0, 2);
        _isTurn = random == 1 ? true : false;
        _defaultMove = _moveCount;
        if (_isTurn)
        {
            _player1.SetActive(true);
            _player2.SetActive(false);
        }
        else
        {
            _player1.SetActive(false);
            _player2.SetActive(true);
        }
    }

    public void TurnChange()
    {
        _isTurn = !_isTurn;
        _moveCount = _defaultMove;
        if(_isTurn)
        {
            _player1.SetActive(true);
            _player2.SetActive(false);
        }
        else
        {
            _player1.SetActive(false);
            _player2.SetActive(true);
        }
    }
}
