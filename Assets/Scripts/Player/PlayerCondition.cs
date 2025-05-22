using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public ConditionUI conditionUI;

    Condition health { get { return conditionUI.health; } }
    Condition stamina { get { return conditionUI.stamina; } }

    public event Action onTakeDamage;


    void Update()
    {
        health.Add(health.PassiveValue * Time.deltaTime);
        stamina.Add(stamina.PassiveValue * Time.deltaTime);

        if (health.CurValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void AddStamina(float amount)
    {
        stamina.Add(amount);
    }

    public void Die()
    {
        Debug.Log("ав╬З╢ы.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.CurValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
}