using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // 아이템 데이터

    public Button button;   // 인벤토리 슬롯 버튼
    public Image icon;      // 아이템 아이콘
    public TextMeshProUGUI quantityText;    // 아이템 개수 텍스트
    private Outline outline;    // 장착된 아이템 확인용 테두리

    public InventoryUI inventory;

    public int index;   // 인벤토리 슬롯 인덱스
    public bool equipped;   // 아이템이 장착되었는지 확인
    public int quantity;    // 아이템 개수

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()   // 슬롯에 아이템이 있을 경우
    {
        icon.gameObject.SetActive(true);    // 아이콘 활성화
        icon.sprite = item.icon;            // 아이템 아이콘 표시
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;  // 아이템 개수가 1이상일 경우 개수 표시

        if (outline != null)
        {
            outline.enabled = equipped;     // 아이템의 장착 유무에 따라 테두리 표시
        }
    }

    public void Clear() // 슬롯에 아이템이 없을 경우
    {
        item = null;  // 아이템 해제
        icon.gameObject.SetActive(false);   // 아이콘 비활성화
        quantityText.text = string.Empty;   // 개수 표시 텍스트 비우기
    }

    public void OnClickButton() // 아이템 슬롯 클릭할 경우
    {
        inventory.SelectItem(index);    // 해당 인덱스의 아이템 선택
    }
}
