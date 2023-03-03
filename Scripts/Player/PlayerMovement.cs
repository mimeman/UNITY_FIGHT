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
        // �÷��̾ �����̰� ��ġ�� ������ �ʴ� ���
        if (horizontal != 0 || vertical != 0 && !playerAttack.isPunching && !playerAttack.isKicking && !playerAttack.isCrossPunching &&!isSliding)
        {
            // �÷��̾� �Է��� ����Ͽ� ���� ���� ����
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            // �÷��̾ ȸ���ؾ� �ϴ� ������ ����մϴ�.
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            
            // SmoothDampAngle �޼��带 ����Ͽ� ��� ������ ������� �� ������ ����մϴ�.
            // �÷��̾ �ش� ������ ���� �� �ɸ��� �ð�
            float smoothAngle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, angle, ref smoothTurnVelocity, smoothTurnTime);

            playerTransform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            playerTransform.Translate(Vector3.forward * movespeed * Time.fixedDeltaTime);

            isWalking = true;
            playerAnimator.SetBool("isWalking", true);
        }

        // �÷��̾ �������� ������
        else
        {
            isWalking = false;
            playerAnimator.SetBool("isWalking", false);
        }
    }


    #region jump, slide
    private IEnumerator Slide()
    {
        //���� �������� ���� ����
        rb.AddForce(playerTransform.forward * slideForce, ForceMode.Impulse);

        isSliding = true;
        playerAnimator.SetBool("isSliding", true);

        yield return new WaitForSeconds(1.38f);

        // �÷��̾ ���� �Ǵ� ���� �࿡ �Է��� ���
        if (true)
        {
            playerAnimator.SetBool("isWalking", true);
        }

        isSliding = false;
        playerAnimator.SetBool("isSliding", false);
    }


    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ���� ������ isGrounded�� true�� ����
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    #endregion
}




//ī�޶� https://www.youtube.com/watch?v=4HpC--2iowE
