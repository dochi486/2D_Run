using System.Collections.Generic;
using UnityEngine;
namespace Run
{
    public class MagneticEffect : MonoBehaviour
    {
        public static MagneticEffect instance;

        private void Awake()
        {
            instance = this;
        }

        Dictionary<Transform, RefFloat> items = new Dictionary<Transform, RefFloat>();
        //자석에 이끌린 transform, 가속도 float
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.GetComponent<GemItem>() == null)
                return;

            if (items.ContainsKey(collision.transform))
                return;

            //items.Add(collision.transform, 0); //초기 가속도는 0
            //함수를 통해 딕셔너리에 넣지 않고 다른 방법으로 넣으면 중복으로 넣어도 에러가 안난다.
            items[collision.transform] = new RefFloat();
        }
        internal void RemoveItem(Transform transform)
        {
            items.Remove(transform);
        }

        public float acclerate = 1200; //초당 가속도? 

        //Dictionary<Transform, float> temp = new Dictionary<Transform, float>();
        private void Update()
        {
            var pos = transform.position;
            //dictionary는 for문으로 반복 X, 딕셔너리는 인덱스 개념이 없어서 for문 사용 불가
            //temp.Clear(); //딕셔너리를 매번 초기화 하기 위한 것..
                          //업데이트 루프를 끝내고나면 임시 메모리가 쌓이기 때문에 클리어를 해주는 것.. 

            //foreach (var item in items)
            //{
            //    temp[item.Key] = item.Value;
            //}

            foreach (var item in items)
            {
                var gemTr = item.Key;
                //items[item.Key] = item.Value + acclerate * Time.deltaTime; 
                float accleration = item.Value.acc + acclerate * Time.deltaTime; //누적 가속도
                items[item.Key].acc = accleration;
                //items 딕셔너리를 바로 수정하는게 아니라 temp를 컬렉션으로 사용하는 foreach문이기 때문에 에러안남(직접 접근이 아니라서)

                Vector2 dir = (pos - gemTr.position).normalized;
                Vector2 move = dir * accleration * Time.deltaTime;
                gemTr.Translate(move);
            }
        }
    }

    class RefFloat
    {
        public float acc; //가속도를 딕셔너리 하나만 쓰고도 참조할 수 있도록 만들어준 클래스와 변수
    }
}