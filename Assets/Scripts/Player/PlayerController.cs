using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;   // 플레이어 이동 속도
    [SerializeField] private float jumpPower;   // 플레이어 점프력
    [SerializeField] private LayerMask groundLayer; // 바닥 체크를 위한 레이어마스크
    private Vector2 movementDirection;          // 이동 방향 

    [Header("Look")]
    [SerializeField] private Transform cameraContainer; // 카메라의 상하 회전을 담당하는 오브젝트
    [SerializeField] private float lookSensitivity; // 마우스 감도
    [SerializeField] private float minXLook;    // 올려다볼 수 있는 각도
    [SerializeField] private float maxXLook;    // 내려다볼 수 있는 각도
    private float camCurXRot;      //  현재 카메라의 상하 회전값
    private Vector2 mouseDelta; // 프레임마다 입력된 마우스 이동값

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 화면 중앙에 마우스 커서 고정
    }

    private void FixedUpdate()
    {
        Movement(); // 플레이어 이동
    }

    private void LateUpdate()
    {
        Look();     // 플레이어 화면 이동
    }

    void Movement()
    {
        Vector3 dir = transform.forward * movementDirection.y + transform.right * movementDirection.x; // 이동 방향을 입력값 기준으로 계산 (앞/뒤는 y축, 좌/우는 x축 기준)
        dir *= moveSpeed;               // 계산한 방향 벡터에 이동 속도를 곱해 최종 이동 벡터 생성
        dir.y = _rigidbody.velocity.y;  // y축 방향 속도는 현재 Rigidbody의 y속도를 유지 (점프나 중력 등 반영)

        _rigidbody.velocity = dir;      // Rigidbody에 계산된 최종 이동 벡터를 적용
    }

    void Look()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;              // 카메라의 상하 회전값
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);     // 카메라 상하 회전 각도 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 카메라를 상하로 회전

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // 플레이어 오브젝트를 좌우로 회전
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)        // 입력이 유지되고 있을 경우
        {
            movementDirection = context.ReadValue<Vector2>();   // 입력된 2D 이동 벡터를 저장
        }
        else if (context.phase == InputActionPhase.Canceled)    // 입력이 해제되었을 경우
        {
            movementDirection = Vector2.zero;                   // 이동 방향 초기화
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();  // 마우스 이동 값을 읽어 마우스 델타에 저장
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && OnGround())    // 플레이어가 바닥에 있고 입력이 되었을 때 
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // 플레이어를 위쪽 힘을 가해 점프 실행
        }
    }

    bool OnGround()
    {
        Ray[] rays = new Ray[4] // 플레이어 아래 방향으로 4개의 레이를 쏨
        {
            new Ray(transform.position + (transform.forward * 0.2f + transform.up * 0.01f), Vector3.down),  // 앞
            new Ray(transform.position + (-transform.forward * 0.2f + transform.up * 0.01f), Vector3.down), // 뒤
            new Ray(transform.position + (transform.right * 0.2f + transform.up * 0.01f), Vector3.down),    // 오른쪽
            new Ray(transform.position + (-transform.right * 0.2f + transform.up * 0.01f), Vector3.down)    // 왼쪽
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))    // 각 레이가 바닥 레이어에 닿았을 경우
            {
                return true;    // 바닥(true)
            }
        }

        return false;   // 공중(false)
    }
}
