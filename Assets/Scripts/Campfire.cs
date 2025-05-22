using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public int damage;          // 캠프파이어의 데미지
    public float damageRate;    // 데미지 주기

    List<IDamagable> things = new List<IDamagable>();   // 캠프파이어에 들어온 데미지를 입는 오브젝트를 저장하는 리스트

    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);   // 주기마다 데미지를 줌
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);   // 캠프파이어에 들어온 오브젝트 데미지 적용
        }
    }

    private void OnTriggerEnter(Collider other) // 들어온 데미지를 입는 오브젝트를 리스트에 추가
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)  // 나간 데미지를 입는 오브젝트를 리스트에서 제거
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
