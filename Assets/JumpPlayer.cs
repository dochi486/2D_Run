using UnityEngine;

public class JumpPlayer : Player
{
    [SerializeField] float jumpTime = 20;

    internal override void StartJump()
    {
        base.StartJump();
        jumpTime += 1;

        if (Input.GetKeyUp(KeyCode.W))
        {
            JumpByValue();
            jumpTime = 0;
        }
        if (jumpTime >= 1000)
            JumpByValue();
    }

    private void JumpByValue()
    {
        rigid.AddForce(new Vector2(0, jumpTime));
    }
}
