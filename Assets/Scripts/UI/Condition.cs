using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour  // 체력, 스테미나를 관리
{
    [SerializeField] private float curValue;    // 현재 상태값
    [SerializeField] private float startValue;  // 시작 상태값
    [SerializeField] private float maxValue;    // 최대 상태값
    [SerializeField] private float passiveValue;    // 지속적인 값 (자연 회복 등)
    [SerializeField] private Image uiBar;       // 상태값 비율로 UI에 표시할 상태바

    public float CurValue { get { return curValue; } }
    public float PassiveValue { get { return passiveValue; } }

    void Start()
    {
        curValue = startValue;  // 현재 상태값 초기화
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage(); // 현재 상태 비율에 맞게 UI 상태바 길이 조절
    }

    float GetPercentage()
    {
        return curValue / maxValue; // 상태값의 비율
    }

    public void Add(float value)    // 상태값 추가용 함수
    {
        curValue = Mathf.Min(curValue + value, maxValue);   
    }

    public void Subtract(float value)   // 상태값 감소용 함수
    {
        curValue = Mathf.Max(curValue - value, 0f);         
    }
}
