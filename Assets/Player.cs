using System.Collections;
using System.Collections.Generic;
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
        ////rayStart = transform;
        //animator.Play("Run"); Idle로 시작하는 게 자연스러우니까!


    }



    public float speed = 20;
    public float midairVelocity = 10;

    int jumpCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (state == StateType.IdleOrRunOrJump)
        {
            Move();
            Jump();
        }
        Attack();
        UpdateSprite(); //애니메이션
        //dochi.dochiAge = 4;
    }
    StateType state = StateType.IdleOrRunOrJump;
    //public class Hedgehogs
    //{
    //    public string dochiName;
    //    public float dochiAge;
    //}

    //Hedgehogs dochi;


    [System.Serializable]
    public class AttackInfo
    {
        public string clipName;
        public float animationTime; //0.6f
        public float dashSpeed;
        public float dashTime;
        public GameObject collider;
    }
    public List<AttackInfo> attacks;
    public enum StateType
    {
        Attack,
        IdleOrRunOrJump,
        Attacked,
        Die
    }
    Coroutine attackHandle;
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attackHandle != null)
            {
                currentAttack?.collider.SetActive(false);
                StopCoroutine(attackHandle);
            }
            attackHandle = StartCoroutine(AttackCo());

        }
    }


    int currentAttackIndex = 0;
    AttackInfo currentAttack = new AttackInfo();
    IEnumerator AttackCo()
    {
        state = StateType.Attack;
        currentAttack = attacks[currentAttackIndex];
        currentAttackIndex++;
        if (currentAttackIndex == attacks.Count)
            currentAttackIndex = 0;
        animator.Play(currentAttack.clipName);
        currentAttack.collider.SetActive(true);
        //currentAttack.dashSpeed

        float dashEndTime = Time.time + currentAttack.dashTime;
        float waitEndTime = Time.time + currentAttack.animationTime;
        while (waitEndTime > Time.time)
        {
            if (dashEndTime > Time.time)
                transform.Translate(currentAttack.dashSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }
        //yield return new WaitForSeconds(currentAttack.animationTime);


        state = StateType.IdleOrRunOrJump;

        currentAttack.collider.SetActive(false);

        currentAttackIndex = 0;
    }

    float moveX;

    private void UpdateSprite()
    {
        if (state != StateType.IdleOrRunOrJump)
            return;


        float velocity = rigid.velocity.y;
        float absVelocity = Mathf.Abs(velocity);

        //string animationName = "";
        string animationName = string.Empty; //< -이걸 더 자주 쓴다

        //float absVelocity = velocity > 0 ? velocity : -velocity;

        if (IsGround())
        {
            jumpCount = 0;
            if (moveX == 0)
                animationName = "Idle";
            else
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

    private void Jump()
    {
        if (jumpCount < 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartJump();

            }
        }
    }

    private void StartJump()
    {
        jumpCount++;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(jumpForce);
    }

    private void Move()
    {

        //a,d 좌우이동
        //float moveX = 0;
        moveX = 0;

        if (Input.GetKey(KeyCode.A))
            moveX = -1;
        if (Input.GetKey(KeyCode.D))
            moveX = 1;
        if (moveX != 0)
        {
            UpdateRotation(moveX);
            transform.Translate(1 * speed * Time.deltaTime, 0, 0);
            //Space.World추가 안했을 땐 회전한 상태로 오른쪽으로 갔는데 로컬축이 기본값이라서 그렇다. 
            //플레이 상태에서 플레이어랑 스프라이트의  로컬 축 x 값 변경해봐도 계획한대로 가는데 왜 로컬축이라서 방향이 다른거지??


        }
    }

    private void UpdateRotation(float currentMove)
    {
        if (currentMove == 0)
            return;

        transform.rotation = Quaternion.Euler(0, currentMove < 0 ? 180 : 0, 0);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {


        Monster monster = collision.gameObject.GetComponent<Monster>();
        if (monster == null || monster.hp <= 0)
            return;

        bool isStepped = false; //밟았을 때 true
        if (collision.contacts[0].normal.y > 0.9f)
            isStepped = true;

        if (isStepped)
        {
            monster.OnDamage(1);
            StartJump();
        }
        else
        {


            hitpoint -= monster.damage;
            StartCoroutine(HitCo());

            if (hitpoint <= 0)
            {
                StartCoroutine(DieCo());
            }

        }
    }
    private IEnumerator DieCo()
    {
        yield return new WaitForSeconds(delayHit);
        state = StateType.Die;
        animator.Play("Die");
    }

    public float delayHit = 0.3f;
    IEnumerator HitCo()
    {
        state = StateType.Attacked;
        animator.Play("Damage");

        yield return new WaitForSeconds(delayHit);
        state = StateType.IdleOrRunOrJump;
    }

    public int hitpoint = 5; //hp는 hitpoint의 얒ㄱ자
    //OnTriggerEnter2D에서는 collision.GetComponent<>바로 가능했찌만
    //OnCollisionEnter2D는 한 단계를 거쳐서 겟할 수 있다
}

