using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    public Vector2 jumpForce = new Vector2(0, 1000);
    public float gravityScale = 7;
    public Transform rayStart;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("Sprite").GetComponent<Animator>();
        //animator = GetComponentInChildren<Animator>();

        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        //rayStart = transform;

        animator.Play("Run");

    }

    public float speed = 20;
    public float midairVelocity = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(jumpForce);
            //if(rigid.velocity.y > 10)
            //{
            //    animator.Play("Jump_Up");
            //} 안된다ㅠㅠ
        }
        float velocity = rigid.velocity.y;
        float absVelocity = Mathf.Abs(velocity);
        string animationName = "";
        //string animationName = string.Empty; <-이걸 더 자주 쓴다

        //float absVelocity = velocity > 0 ? velocity : -velocity;

        if (IsGround())
        {
            animationName = "Run";
        }
        else
        {
            if (absVelocity < midairVelocity)
                //midAir
                animationName = "Jump_MidAir";

            else if (velocity > 0)
                animationName = "Jump_Up";

            else
                //떨어진다
                animationName = "Jump_Fall";

        }

        animator.Play(animationName);

    }

    public float rayCheckDistance = 0.1f;
    public LayerMask groundLayer;
    private bool IsGround()
    {
        var hit = Physics2D.Raycast(rayStart.position, Vector2.down, rayCheckDistance, groundLayer);
        return hit.transform != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(rayStart.position, Vector2.down * rayCheckDistance);
    }

}
