using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackCollider : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<MonsterBase>()?.OnDamage(damage);

        //Monster monster = collision.GetComponent<Monster>();
        //if (monster == null)
        //    return;
        //monster.OnDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<MonsterBase>()?.OnDamage(damage);

    }
}

