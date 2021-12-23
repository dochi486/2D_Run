using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : Player
{
    [SerializeField] float jumpTime = 20;

    internal override void StartJump()
    {
        base.StartJump();
        jumpTime++;

        if(Input.GetKeyUp(KeyCode.W))
            rigid.AddForce(new Vector2(0, jumpTime));

        if (jumpTime >= 30)
            rigid.AddForce(new Vector2(0,jumpTime));
    }
}
