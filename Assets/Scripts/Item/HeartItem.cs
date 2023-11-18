using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : MonoBehaviour, ItemBase
{
    [SerializeField]
    int _recovery = 5;
    public void Use(Character player)
    {
        Debug.Log("‰ñ•œ");
        player.HP += _recovery;
    }
}
