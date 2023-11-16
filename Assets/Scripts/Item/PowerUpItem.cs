using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem :MonoBehaviour, ItemBase
{
    public void Use(Character player)
    {
        player.Index++;
    }
}
