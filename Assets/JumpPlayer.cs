using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : Player
{
    [SerializeField] float jumpTime = 20;

    internal override void Jump()
    {
        base.Jump();
    }
}
