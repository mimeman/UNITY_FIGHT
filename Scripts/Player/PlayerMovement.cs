using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 moveDirection;

    public Rigidbody rb;

    public Transform playerTransform;
    public Transform cameraTransform;

    public BoxCollider groundChecker;
    [Space(3)]

    [Header("Move")]
    private float horizontal;
    private float vertical;
    public float movespeed;
    public float slideForce;
    public float smoothTurnTime; 
    public float smoothTurnVelocity;
    [Space(3)]

    //Animation
    public bool isWalking = false;
    public bool isSliding = false;
    public bool isGrounded = true;

    public Animator playerAnimator;

    public PlayerAttack playerAttack;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        GetMovementInput();

        if (Input.GetButtonDown("Slide") && !isSliding && isGrounded && !playerAttack.isPunching && !playerAttack.isKicking)
        {
            StartCoroutine(Slide());
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetMovementInput()
    {
        // Gets input from the horizontal axis
        horizontal = Input.GetAxisRaw("Horizontal");

        // Gets input from the vertical axis
        vertical = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        // 플레이어가 움직이고 펀치를 날리지 않는 경우
        if (horizontal != 0 || vertical != 0 && !playerAttack.isPunching && !playerAttack.isKicking && !playerAttack.isCrossPunching &&!isSliding)
        {
            // 플레이어 입력을 사용하여 방향 벡터 생성
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            // 플레이어가 회전해야 하는 각도를 계산합니다.
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            
            // SmoothDampAngle 메서드를 사용하여 대상 각도를 기반으로 새 각도를 계산합니다.
            // 플레이어가 해당 각도를 보는 데 걸리는 시간
            float smoothAngle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, angle, ref smoothTurnVelocity, smoothTurnTime);

            playerTransform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            playerTransform.Translate(Vector3.forward * movespeed * Time.fixedDeltaTime);

            isWalking = true;
            playerAnimator.SetBool("isWalking", true);
        }

        // 플레이어가 움직이지 않으면
        else
        {
            isWalking = false;
            playerAnimator.SetBool("isWalking", false);
        }
    }


    #region jump, slide
    private IEnumerator Slide()
    {
        //전진 방향으로 힘을 더함
        rb.AddForce(playerTransform.forward * slideForce, ForceMode.Impulse);

        isSliding = true;
        playerAnimator.SetBool("isSliding", true);

        yield return new WaitForSeconds(1.38f);

        // 플레이어가 가로 또는 세로 축에 입력한 경우
        if (true)
        {
            playerAnimator.SetBool("isWalking", true);
        }

        isSliding = false;
        playerAnimator.SetBool("isSliding", false);
    }


    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 땅에 닿으면 isGrounded를 true로 설정
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    #endregion
}




//카메라 https://www.youtube.com/watch?v=4HpC--2iowE
