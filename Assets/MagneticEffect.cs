using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagneticEffect : MonoBehaviour
{

    Dictionary<Transform, float> items = new Dictionary<Transform, float>(); //자석에 붙은 transform, 가속도 float..
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<GemItem>() == null)
            return;

        //items.Add(collision.transform, 0); //초기 가속도는 0
        //함수를 통해 딕셔너리에 넣지 않고 다른 방법으로 넣으면 중복으로 넣어도 에러가 안난다...
        items[collision.transform] = 0;

    }

    public float acclerate = 1200; //초당 가속도? 

    Dictionary<Transform, float> temp = new Dictionary<Transform, float>();
    private void Update()
    {
        //items에 있는 아이템들을 트랜스폼의 포지션으로 이동하게???
        var pos = transform.position;
        //dictionary는 for문으로 반복 X, 딕셔너리는 인덱스 개념이 없어서 for문 사용 불가
        temp.Clear(); //딕셔너리를 매번 초기화 하기 위한 것..
                      //업데이트 루프를 끝내고나면 임시 메모리가 쌓이기 때문에 클리어를 해주는 것.. 

        foreach (var item in items)
        {
            temp[item.Key] = item.Value;
        }

        foreach (var item in temp)
        {
            var gemTr = item.Key;
            //items[item.Key] = item.Value + acclerate * Time.deltaTime; 
            float accleration = item.Value + acclerate * Time.deltaTime; //누적 가속도
            items[item.Key] = accleration;

            Vector2 dir = (pos - gemTr.position).normalized;
            Vector2 move = dir * accleration * Time.deltaTime;
            gemTr.Translate(move);
        }
    }
}
