using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveLocal : MonoBehaviour
{
    public Vector3 moveAxis = new Vector3(0, 1, 0);
    public float speed = 1;
    public float destroyTime = 5;


    private void OnEnable()
    {
        ObjectPool.Destroy(gameObject, destroyTime); //ObjectPool에서 꺼내면 인에이블 되니까 디스트로이를 켜는 것. Awake를 하면 1번만 실행된다. 
        //매번 실행되어야 하는 걸 OnEnable에 넣어둬야 한다. 
    }
    void Update()
    {
        transform.Translate(moveAxis * speed * Time.deltaTime, Space.Self);
    }
}
