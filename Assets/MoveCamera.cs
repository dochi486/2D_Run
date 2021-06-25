using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public float speed = 20;

    // Update is called once per frame
    void Update()
    {
        //아래 코드를 두 군데에 썼기 때문에 리팩토링해서 사용하는 방법
        if (RunGameManager.IsPlaying() == false)
            return;

        //if (RunGameManager.instance.gameStateType != RunGameManager.GameStateType.Playing)
        //    return;

        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
