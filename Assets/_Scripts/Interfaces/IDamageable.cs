using System.Collections;
using UnityEngine;

public interface IDamageable
{
    int HP { get;}
    void TakeDamage(int damage);
    void Die();
}