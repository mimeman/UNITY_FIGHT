using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{


    public Animator enemyAnimator;
    public Transform enemyTransform;
    public Transform player;

    public float speed;
    public float attackRange, sightRange; //�þ� ����, ���� ����
    public bool playerInAttackRange , playerInSightRange; //boolŸ�� �þ� ����, ���� ����

    public LayerMask whatIsPlayer;

    //anima
    private bool canMove;
    private bool canAttack;

    //Attacking
    public float damage = 1f; //���ݷ�
    public float attackDelay; //���� ������
    public float health;  //���� ü��



    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemyAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);  //�þ� ����
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //���� ����


        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); //player �þ� �����ȿ� ������, ���� ���� ���� ���� ���� ��� (����)
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); //player �þ� ����, ���� ���� ���� ���� (����)
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

    public void TakeDamage(int damage) { //������ �Դ°�
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





    /*    //�������� �Ծ��� �� ������ ó��
        public override void OnDamage(float damage)
        {
            //�ǰ� �ִϸ��̼� ���
            enemyAnimator.SetTrigger("Hit");

            //livingEntity�� OnDamage()�� �����Ͽ� ������ ����
            base.OnDamage(damage);

            //ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
            if (health <= 0 && !dead)
            {
                Die();
                enemyAnimator.SetTrigger("Death");
            }
        }

        //������ �ֱ�
        public void OnDamage()
        {
            livingEntity attackTarget = targetEntity.GetComponent<livingEntity>();

            attackTarget.OnDamage(damage);

            lastAttackTime = Time.time;
        }*/
}
