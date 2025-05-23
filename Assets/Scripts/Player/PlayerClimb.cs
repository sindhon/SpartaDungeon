using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private float climbSpeed;      // 벽타기 속도
    [SerializeField] private LayerMask wallLayer;   // 벽을 감지하기 위한 레이어마스크
    private Vector2 climbDirection;     // 벽타기 방향
    Vector3 wallNormal;   // 감지된 벽의 법선 벡터 (벽 표면의 방향)
    Vector3 wallRight;    // 벽 기준 오른쪽 방향 벡터
    Vector3 wallUp;       // 벽 기준 위쪽 방향 벡터

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        SwitchClimb();  // 벽타기와 걷기 전환
    }

    void Climb()
    {
        Vector3 dir = wallRight * climbDirection.x + wallUp * climbDirection.y; // 벽 기준 방향으로 입력 방향을 변환하여 이동 벡터 계산
        dir *= climbSpeed;          // 계산한 방향 벡터에 벽타기 속도를 곱해 최종 이동 벡터 생성

        _rigidbody.velocity = dir;  // Rigidbody에 계산된 최종 이동 벡터를 적용
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)     // 입력이 유지되고 있을 경우
        {
            climbDirection = context.ReadValue<Vector2>();   // 입력된 이동 벡터를 저장
        }
        else if (context.phase == InputActionPhase.Canceled) // 입력이 해제되었을 경우
        {
            climbDirection = Vector2.zero;                   // 벽 타기 방향 초기화
        }
    }

    void SwitchClimb()
    {
        if (OnWall())   // 플레이어가 벽에 있을 경우
        {
            _rigidbody.useGravity = false;  // 중력 제거
            Climb();    // 벽타기 실행
        }
        else
        {
            _rigidbody.useGravity = true;   // 아닐 경우 중력 적용
        }
    }

    bool OnWall()
    {
        RaycastHit hit;

        // 벽을 감지할 플레이어의 몸 위아래 두 점 지정
        Vector3 point1 = transform.position + Vector3.up * 0.2f;
        Vector3 point2 = transform.position + Vector3.up * 1.5f;

        // 두 점 사이에 캡슐을 쏘아 앞쪽 방향으로 벽이 있는지 검사
        if (Physics.CapsuleCast(point1, point2, 0.24f, transform.forward, out hit, 0.5f, wallLayer)) 
        {
            wallNormal = hit.normal;    // 감지된 벽의 법선 벡터 저장
            UpdateWallVectors();        // 벽 기준 벡터 업데이트
            return true;    
        }

        return false;
    }

    void UpdateWallVectors()
    {
        wallRight = Vector3.Cross(-Vector3.up, wallNormal).normalized;  // 벽의 오른쪽 방향 계산
        wallUp = Vector3.Cross(wallNormal, -wallRight).normalized;      // 벽의 위쪽 방향 계산
    }
}
