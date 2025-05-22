using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable // 데미지를 받는 오브젝트가 구현해야 할 인터페이스
{
    void TakePhysicalDamage(int damage);    // 데미지를 받는 함수
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public ConditionUI conditionUI;

    Condition health { get { return conditionUI.health; } }     // 플레이어 체력
    Condition stamina { get { return conditionUI.stamina; } }   // 플레이어 스테미나

    public event Action onTakeDamage;   // 데미지 받기 델리게이트


    void Update()
    {
        health.Add(health.PassiveValue * Time.deltaTime);   // 기본 체력 회복
        stamina.Add(stamina.PassiveValue * Time.deltaTime); // 기본 스테미나 회복

        if (health.CurValue <= 0f)  // 체력이 0보다 작을 경우
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount); // 입력 값만큼 체력 회복
    }

    public void AddStamina(float amount)
    {
        stamina.Add(amount); // 입력 값만큼 스테미나 회복
    }

    public void Die()
    {
        Debug.Log("죽었다.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);    // 입력 값만큼 체력 감소
        onTakeDamage?.Invoke();     // 데미지를 받았다는 이벤트 발생
    }

    public bool UseStamina(float amount)
    {
        if (stamina.CurValue - amount < 0f) // 스테미나가 부족할 경우
        {
            return false;
        }

        stamina.Subtract(amount);   // 입력 값만큼 스테미나 소모
        return true;
    }
}