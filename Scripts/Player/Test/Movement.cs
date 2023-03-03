using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region �����
    [SerializeField] //��ȣ���� ���� �Ǹ鼭 �ν����� â���� ���� ����
    private float walkSpeed; //����� �ȴ� �ӵ�
    [SerializeField]
    private float runSpeed; //����� �ٴ� �ӵ�
    [SerializeField]
    private float crouchSpeed; //�ɾ��� �� �ӵ�

    private float applySpeed; //walk, run -> applyeSpeed�� ���� �뵵



    [SerializeField]
    private float jumpForce; //���� ����

    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;


    //�ɾ��� �� �󸶳� ������ �����ϴ� ����.
    [SerializeField]
    private float crouchPosY; //�ɱ� ��
    private float originPosY; //���� ��
    private float applyCrouchPosY; //����� �����ؼ� ������



    [SerializeField]
    private float lookSensitivity; //ī�޶� �ΰ���

    [SerializeField]
    private float cameraRotationLimit; //ī�޶� ȸ�� �ɶ� ����Ʈ �ɱ�
    private float currentCameraRotationX; //�ٶ󺸴� ����


    [SerializeField] //�ʿ��� ������Ʈ
    private Camera myCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider; //�� ���� ����

    #endregion

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();

        applySpeed = walkSpeed;

        originPosY = myCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }


    void Update() // ��ǻ�͸��� �ٸ����� �뷫 1�ʿ� 60�� ����
    {

        TryRun();
        TryCrouch();
        Move();                  //Ű���� �Է¿� ���� �̵���
        CameraRotation();        //���콺�� ���Ʒ�(Y) �����ӿ� ���� ī�޶� X �� ȸ��
        CharactorRotation();     //���콺 �¿�(X) �����ӿ� ���� ĳ���� Y �� ȸ�� 
    }

    void TryCrouch() //�������� �����ϴ°�
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch) //������ 
        {
            Debug.Log("����");
            applySpeed = crouchSpeed; //�ɾ��� ��� ���ǵ�� ����
            applyCrouchPosY = crouchPosY; //��ũ���� ����
        }

        else
        { //�Ͼ��
            Debug.Log("�Ͼ");
            applySpeed = walkSpeed; //�ȴ� ���ǵ��
            applyCrouchPosY = originPosY; //���� ����

        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {

        float _posY = myCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.35f);
            myCamera.transform.localPosition = new Vector3(0f, _posY, 0f);

            if (count < 15)
                break;

            yield return null;
        }

        myCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    #region Jump


    #endregion

    #region Run
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //���� ����ƮŰ ������ 
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//���� ����ƮŰ ����
        {
            RunningCancel();
        }
    }

    void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Running()
    {
        //���� ���¿��� ������ ���� ���� ����
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }
    #endregion

    #region move,Rotation
    void Move() // WASD, ����Ű Ű���� �Է¿� ���� �̵�
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // A D
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // W S 

        Vector3 _MoveHorizontal = transform.right * _moveDirX; //�¿� �̵� ���� ��, ���� * ũ�� = transform.right * _moveDirX
        Vector3 _MoveVertical = transform.forward * _moveDirZ; //�յ� �̵� ���� ��, ���� * ũ�� = transform.right * _moveDirX

        Vector3 _velocity = (_MoveHorizontal + _MoveVertical).normalized * applySpeed; //�ӵ� ����

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }

    void CharactorRotation()
    {
        //�¿� ĳ���� ȸ�� 
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity; //����

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //ȸ�� ����
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(_cameraRotationX, -cameraRotationLimit, cameraRotationLimit); //_cameraRotationX���� -45 ~ 45 ���� ���� ��Ŵ

        myCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    #endregion
}


//����Ƽ������ X �� �̵� -> �¿�,
// Z �� �̵� -> �յ�,
//Y �� �̵� -> ���Ʒ�