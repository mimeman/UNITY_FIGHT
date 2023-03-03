using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : livingEntity
{
    public Collider leftHandHitbox;
    public Collider rightHandHitbox;
    public Collider leftfootHitbox;
    public Collider rightfootHitbox;

    public bool hasHitSomething = false;
    public bool isPunching = false;
    public bool isPunchingCombo = false;
    public bool isKicking = false;
    public bool isCrossPunching = false;

    public float damage = 1f; //공격력

    public Animator playerAnimator;

    public PlayerMovement playerMovement;

    /*public CustomAnimationEvents AnimationEvent;*/

/*    public float cooldown = 0.5f; //공격 쿨타임
    private float maxTime = 0.8f; //콤보 종료까지의 최대 시간
    private float timeStamp; //시간 찍어줌
    private int maxCombo = 3; //콤보 최대 공격 횟수
    private int combo = 0; //현재 콤보
    private float lastTime; //마지막 공격 시간*/




    void Start()
    {
        // Disable the punch hitbox

        /*AnimationEvent.AnimationEvent.AddListener(OnAnimationEvent);*/

        leftHandHitbox.enabled = false;
        rightHandHitbox.enabled = false;
        leftfootHitbox.enabled = false;
        rightfootHitbox.enabled = false;
    }


    void Update()
    {
        // If presses left click and not punching
        if (Input.GetButtonDown("Fire1") && !isPunching && playerMovement.isGrounded)
        {
            StartCoroutine(Punch());
        }

        // If presses "E"
        else if (Input.GetKeyDown(KeyCode.E) && playerMovement.isGrounded)
        {
            StartCoroutine(Kick());
        }

        // If presses "R"
        else if (Input.GetKeyDown(KeyCode.R) && playerMovement.isGrounded)
        {
            StartCoroutine(CrossPunch());
        }

        // If presses "F"
        else if (Input.GetKeyDown(KeyCode.F) && playerMovement.isGrounded)
        {
            ComboAttack();
        }
    }



    #region Attack
    private IEnumerator Punch()
    {
        leftHandHitbox.enabled = true;
        isPunching = true;
        playerAnimator.SetBool("isPunching", true);

        Debug.Log("Punch");

        yield return new WaitForSeconds(1.2f);

        leftHandHitbox.enabled = false;
        isPunching = false;

        playerAnimator.SetBool("isPunching", false);

        Debug.Log("Punch end");
    }

    private IEnumerator Kick()
    {
        rightfootHitbox.enabled = true;
        isKicking = true;
        playerAnimator.SetBool("isKicking", true);

        Debug.Log("isKicking");

        yield return new WaitForSeconds(1.7f);

        rightfootHitbox.enabled = false;
        isKicking = false;
        playerAnimator.SetBool("isKicking", false);
        
        Debug.Log("isKicking end");
    }

    private IEnumerator CrossPunch()
    {
        leftHandHitbox.enabled = true;
        isCrossPunching = true;
        playerAnimator.SetBool("isCrossPunching", true);

        Debug.Log("isCrossPunching");

        yield return new WaitForSeconds(1.8f);
        leftHandHitbox.enabled = false;
        isCrossPunching = false;
        playerAnimator.SetBool("isCrossPunching", false);
        
        Debug.Log("isCrossPunching end");
    }

    private void ComboAttack()
    {
        leftHandHitbox.enabled = true;
        rightHandHitbox.enabled = true;
        playerAnimator.SetTrigger("ComboAttack");

        leftHandHitbox.enabled = false;
        rightHandHitbox.enabled = false;

    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        // 충돌하는 물체가 적이고 플레이어가 펀치 중에 아직 무언가를 치지 않은 경우 적에게 피해를 줍니다.
        if (other.tag == "Enemy" && isPunching && isCrossPunching && isKicking && isPunchingCombo)
        {        
            other.GetComponent<EnemyAttack>().TakeDamage(1); // EnemyAttack에 있는 TakeDame호출
        }

    }

    //데미지를 입었을 때 실행할 처리
    public void TakeDamage(int damage) //데미지 입는거
    { 
        health -= damage;
        playerAnimator.SetBool("isTakingHit", true);

        if (health <= 0) this.gameObject.SetActive(false);
    }


    /*private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "Combo1":
                StartCoroutine(CrossPunch());
                break;

            case "Combo2":
                break;

            case "Combo3":
                break;
        }
    }*/

    /*    void comgbo()
    {  // 쿨타임 < 시간
        if (timeStamp < Time.time && Input.GetButtonDown("Fire1") && combo < maxCombo)
        {
            combo++;
            Debug.Log("HIT " + combo);
            timeStamp = Time.time + cooldown;

            switch (combo)
            {
                case 1:
                    break;

                case 2:
                    break;
            }
        }
        if ((Time.time - timeStamp) > cooldown)
            combo = 0;
    }*/
}
