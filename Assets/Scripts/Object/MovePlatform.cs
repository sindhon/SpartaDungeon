using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    private Vector3 startPos;                   // 시작 위치
    [SerializeField] private Vector3 targetPos; // 도착 위치
    [SerializeField] private float speed;       // 발판 이동 속도

    private void Start()
    {
        startPos = transform.position;  // 시작 위치 설정
    }

    private void Update()
    {
        Moving();   // 발판 이동
    }

    void Moving()
    {
        float t = Mathf.PingPong(speed * Time.time, 1);             // 0 ~ 1을 오가며 반복적인 값을 반환
        transform.position = Vector3.Lerp(startPos, targetPos, t);  // 시작 위치와 도착 위치 사이를 선형 보간하여 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(this.transform);   // 충돌체를 자식 오브젝트로 설정 (충돌체가 발판과 같이 이동)
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.SetParent(null); // 발판에서 떨어질 경우 자식 관계 해제
    }
}
