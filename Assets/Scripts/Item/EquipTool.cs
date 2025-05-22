using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;    // 공격 주기
    public bool attacking;      // 공격하고 있는지 획인
    public float attackDistance;    // 공격 사거리
    public float useStamina;    // 소모 스테미나

    [Header("Combat")]
    public bool doesDealDamage; // 아이템이 공격용인지 확인
    public int damage;          // 아이템의 공격력

    private Animator animator;
    private Camera camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking) // 공격 중이 아닐 경우
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))  // 스테이나가 충분할 경우 스테미나 소모
            {
                attacking = true;  // 공격 중으로 전환
                animator.SetTrigger("Attack");  // 공격 애니메이션 재생
                Invoke("OnCanAttack", attackRate);  // 공격 주기 이후 공격 가능
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;  
    }

    public void OnHit() // 애니메이션 이벤트에서 호출되는 함수
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 화면 중앙에서 쏜 레이
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))  // 공격 사거리만큼의 레이 안에 물체가 있을 경우
        {
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable target))  // 공격용 아이템이고 대상이 데미지를 받는 오브젝트일 경우
            {
                target.TakePhysicalDamage(damage);  // 대상에게 피해를 줌
            }
        }
    }
}
