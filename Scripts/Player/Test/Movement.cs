using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region 선언부
    [SerializeField] //보호수준 유지 되면서 인스펙터 창에서 접근 가능
    private float walkSpeed; //사용자 걷는 속도
    [SerializeField]
    private float runSpeed; //사용자 뛰는 속도
    [SerializeField]
    private float crouchSpeed; //앉았을 때 속도

    private float applySpeed; //walk, run -> applyeSpeed에 대입 용도



    [SerializeField]
    private float jumpForce; //점프 정도

    //상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;


    //앉았을 때 얼마나 앉을지 결정하는 변수.
    [SerializeField]
    private float crouchPosY; //앉기 값
    private float originPosY; //원래 값
    private float applyCrouchPosY; //여기다 대입해서 쓸거임



    [SerializeField]
    private float lookSensitivity; //카메라 민감도

    [SerializeField]
    private float cameraRotationLimit; //카메라 회전 될때 리미트 걸기
    private float currentCameraRotationX; //바라보는 시점


    [SerializeField] //필요한 컴포넌트
    private Camera myCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider; //땅 착지 여부

    #endregion

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();

        applySpeed = walkSpeed;

        originPosY = myCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }


    void Update() // 컴퓨터마다 다르지만 대략 1초에 60번 실행
    {

        TryRun();
        TryCrouch();
        Move();                  //키보드 입력에 따라 이동함
        CameraRotation();        //마우스를 위아래(Y) 움직임에 따라 카메라 X 축 회전
        CharactorRotation();     //마우스 좌우(X) 움직임에 따라 캐릭터 Y 축 회전 
    }

    void TryCrouch() //앉으려고 실행하는거
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch) //앉으면 
        {
            Debug.Log("앉음");
            applySpeed = crouchSpeed; //앉았을 경우 스피드로 변경
            applyCrouchPosY = crouchPosY; //웅크리기 시점
        }

        else
        { //일어서면
            Debug.Log("일어남");
            applySpeed = walkSpeed; //걷는 스피드로
            applyCrouchPosY = originPosY; //원래 시점

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
        if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 쉬프트키 누르면 
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//왼쪽 쉬프트키 때면
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
        //앉은 상태에서 점프시 앉은 상태 해제
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }
    #endregion

    #region move,Rotation
    void Move() // WASD, 방향키 키보드 입력에 따라 이동
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // A D
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // W S 

        Vector3 _MoveHorizontal = transform.right * _moveDirX; //좌우 이동 벡터 값, 방향 * 크기 = transform.right * _moveDirX
        Vector3 _MoveVertical = transform.forward * _moveDirZ; //앞뒤 이동 벡터 값, 방향 * 크기 = transform.right * _moveDirX

        Vector3 _velocity = (_MoveHorizontal + _MoveVertical).normalized * applySpeed; //속도 벡터

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }

    void CharactorRotation()
    {
        //좌우 캐릭터 회전 
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity; //감도

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    void CameraRotation()
    {
        //상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //회전 감도
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(_cameraRotationX, -cameraRotationLimit, cameraRotationLimit); //_cameraRotationX값이 -45 ~ 45 도에 고정 시킴

        myCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    #endregion
}


//유니티에서는 X 축 이동 -> 좌우,
// Z 축 이동 -> 앞뒤,
//Y 축 이동 -> 위아래