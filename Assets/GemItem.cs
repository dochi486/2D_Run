using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : MonoBehaviour
{
    bool isUse = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //collision.transform.CompareTag("Player");

        if (isUse)
            return;

        if (collision.transform.GetComponentInParent<Player>() == null)
            return;

        print(collision.transform.name +  collision.transform.position);
        GetComponentInChildren<Animator>().Play("Hide",1); 
        //0번레이어는 코인이 돌아가는 애니메이션 진행되고 1번 레이어에서 코인 사라지는 애니메이션 만들거라서 1번 레이어로
        RunGameManager.instance.AddCoin(100);

    }

}
