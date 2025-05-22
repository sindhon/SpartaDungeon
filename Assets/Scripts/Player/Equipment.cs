using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;          // 현재 장착 아이템
    public Transform equipParent;   // 아이템을 장착할 위치

    private PlayerController controller;   
    private PlayerCondition condition;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();  // 기존 아이템 해제
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)   // 장착한 아이템이 있을 경우
        {
            Destroy(curEquip.gameObject);   // 장착한 아이템 제거
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)  // 인벤토리가 꺼져있고 장착한 아이템이 있고 입력이 되고 있을 경우 
        {
            curEquip.OnAttackInput();   // 현재 장착한 아이템으로 공격
        }
    }
}
