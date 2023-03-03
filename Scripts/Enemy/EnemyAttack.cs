using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{


    public Animator enemyAnimator;
    public Transform enemyTransform;
    public Transform player;

    public float speed;
    public float attackRange, sightRange; //시야 범위, 공격 범위
    public bool playerInAttackRange , playerInSightRange; //bool타입 시야 범위, 공격 범위

    public LayerMask whatIsPlayer;

    //anima
    private bool canMove;
    private bool canAttack;

    //Attacking
    public float damage = 1f; //공격력
    public float attackDelay; //공격 딜레이
    public float health;  //현재 체력



    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemyAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);  //시야 범위
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //공격 범위


        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); //player 시야 범위안에 있지만, 공격 범위 내에 있지 않은 경우 (추적)
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); //player 시야 범위, 공격 범위 내에 있음 (공격)
    }
  
    private void ChasePlayer()
    {
        canMove = true;
        enemyTransform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z), Vector3.up);
        enemyTransform.Translate(Vector3.forward * speed * Time.deltaTime);       
    }

    public void AttackPlayer()
    {
        canMove = false;
        enemyAnimator.SetBool("isPlayerSpotted", false);
    }

    public void TakeDamage(int damage) { //데미지 입는거
        health -= damage;
        enemyAnimator.SetBool("isTakingHit", true);
        
        if (health <= 0) this.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerAttack>().TakeDamage(1);
        }
    }





    /*    //데미지를 입었을 때 실행할 처리
        public override void OnDamage(float damage)
        {
            //피격 애니메이션 재생
            enemyAnimator.SetTrigger("Hit");

            //livingEntity의 OnDamage()를 실행하여 데미지 적용
            base.OnDamage(damage);

            //체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
            if (health <= 0 && !dead)
            {
                Die();
                enemyAnimator.SetTrigger("Death");
            }
        }

        //데미지 주기
        public void OnDamage()
        {
            livingEntity attackTarget = targetEntity.GetComponent<livingEntity>();

            attackTarget.OnDamage(damage);

            lastAttackTime = Time.time;
        }*/
}
