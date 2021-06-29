using Pathfinding;
using System.Collections;
using UnityEngine;

public class FlyingMonster : MonsterBase
{
    //스프라이트가 오른쪽 보고있으므로 오른쪽으로 갈 때는 y 그대로, 왼쪽으로 갈 때 180
    AIPath aiPath;

    IEnumerator Start()
    {
        animator = GetComponentInChildren<Animator>();
        aiPath = GetComponent<AIPath>();
        while (true)
        {

            if (aiPath.desiredVelocity.x > 0)
            {
                transform.rotation = Quaternion.identity;  //Quaternion.Euler(0, 0, 0);과 같은 뜻
            }
            else if (aiPath.desiredVelocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            yield return null;
        }

    }

}
