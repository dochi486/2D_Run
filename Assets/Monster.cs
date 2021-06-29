using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp = 10;
    public int range = 5;
    public float minWorldX;
    public float maxWorldX;
    public float speed = 5;
    public enum DirectionType
    {
        Right,
        Left,
    }

    DirectionType direction = DirectionType.Right;
    StateType state = StateType.Patrol;

    IEnumerator Start()
    {
        animator = GetComponentInChildren<Animator>();
        minWorldX = transform.position.x - range;
        maxWorldX = transform.position.x + range;

        animator.Play("Run");
        while (true)
        {
            //direction == DirectionType.Right
            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (direction == DirectionType.Right)
            {
               
                if(transform.position.x > maxWorldX)
                {
                    direction = DirectionType.Left;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else
            {
                if (transform.position.x < minWorldX)
                {
                    direction = DirectionType.Right;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
            yield return null;
            if (state == StateType.Die)
                yield break ; //코루틴에서 함수밖으로 나갈 때는 yield break;로 할 수있따.

        }
    }
    Animator animator;

    public enum StateType
    {
        Patrol,
        Attack,
        Attacked,
        Die
    }

    internal void OnDamage(int damage)
    {
        hp -= damage;

            GetComponentInChildren<Animator>().Play("TakeHit");
  
        if(hp <= 0)
        {
            StartCoroutine(DieCo());
        }
    }
    public float dieDelay = 0.3f;
    public float destroyDelay = 0.6f;
    internal int damage;

    IEnumerator DieCo()
    {
        state = StateType.Die;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new WaitForSeconds(dieDelay);
        GetComponentInChildren<Animator>().Play("Death");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
