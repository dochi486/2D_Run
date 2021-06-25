using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    //RunGameManager runGameManager; //싱글톤 안했을 때 사용하는 방법
    Animator animator;
    Rigidbody2D rigid;
    public Vector2 jumpForce = new Vector2(0, 1000);
    public float gravityScale = 7;
    public Transform rayStart;

    internal void OnStageClear()
    {
        animator.Play("Idle");
    }

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //runGameManager = GameObject.Find("Canvas").GetComponent<RunGameManager>(); //싱글톤 안했을 때 사용하는 방법
        //runGameManager = FindObjectOfType<RunGameManager>(); //씬에 있는 모든 오브젝트에서 런게임매니저를 찾는 코드
        //runGameManager = RunGameManager.instance; //찾는 과정 없이 바로 인스턴스에 있는 것을 할당하는 것(싱글톤)

        animator = transform.Find("Sprite").GetComponent<Animator>();
        //animator = GetComponentInChildren<Animator>();

        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        cameraTr = Camera.main.transform;
        //rayStart = transform;
        offsetXCameraPos = cameraTr.position.x - transform.position.x;
        //animator.Play("Run"); Idle로 시작하는 게 자연스러우니까!


    }

    public Transform cameraTr;
    public float offsetXCameraPos; //카메라랑 플레이어의 X값 차이 내 프로젝트에서는 -5! 
    public float allowedOffsetX = 0.2f;
    public float restoreSpeed = 40;
    private void RestoreXPosition()
    {
        float offsetX = cameraTr.position.x - transform.position.x;
        if(offsetX > offsetXCameraPos + allowedOffsetX)
        {
            transform.Translate(restoreSpeed * Time.deltaTime, 0, 0);
        }
    }

    public float speed = 20;
    public float midairVelocity = 10;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    transform.Find("MagneticEffect").gameObject.SetActive(true);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    transform.Find("MagneticEffect").gameObject.SetActive(false);
        //}



        if (RunGameManager.IsPlaying() == false)
            return;
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


        RestoreXPosition();

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
