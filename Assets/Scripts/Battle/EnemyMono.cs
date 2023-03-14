using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMono : MonoBehaviour
{
    [SerializeField] private int _health;

    public void GetDamage(int amount)
    {
        _health -= amount;
    }
}
