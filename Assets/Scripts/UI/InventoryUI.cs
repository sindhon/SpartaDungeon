using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public ItemSlot[] slots;    // 인벤토리 아이템 슬롯들

    public GameObject inventoryWindow;  // 인벤토리 UI 창
    public Transform slotPanel;         // 아이템 슬롯이 들어갈 패널
    public Transform dropPosition;      // 아이템을 떨어뜨릴 위치

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;        // 선택한 아이템 이름
    public TextMeshProUGUI selectedItemDescription; // 선택한 아이템 설명
    public TextMeshProUGUI selectedStatName;        // 선택한 아이템 정보 이름 (체력, 스테미나)
    public TextMeshProUGUI selectedStatValue;       // 선택한 아이템 정보 값 (체력, 스테미나 값)
    public GameObject useButton;        // 아이템 소모 버튼
    public GameObject equipButton;      // 아이템 장착 버튼
    public GameObject unequipButton;    // 아이템 장착 해제 버튼
    public GameObject dropButton;       // 아이템 떨어뜨리기 버튼

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;      // 선택한 아이템 데이터
    int selectedItemIndex = 0;  // 선택한 아이템 슬롯 인덱스

    int curEquipIndex;  // 현재 장비 슬롯 인덱스

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // 인벤토리 토글 이벤트 구독
        CharacterManager.Instance.Player.addItem += AddItem;    // 아이템 추가 이벤트 구독

        inventoryWindow.SetActive(false);           // 인벤토리 UI창 비활성화
        slots = new ItemSlot[slotPanel.childCount]; // 패널에 자식으로 있는 슬롯만큼의 배열 생성

        for (int i = 0; i < slots.Length; i++)  // 배열에 모든 슬롯 추가
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();  // 선택된 아이템 정보 UI 비활성화
    }

    void Update()
    {

    }

    void ClearSelectedItemWindow()  // 선택된 아이템 정보 UI 초기화
    {
        selectedItemName.text = string.Empty; 
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())   // 인벤토리 UI가 활성화되어 있을 경우
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy; // Hierarchy에 인벤토리UI 활성화 유무 반환
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;  // 상호작용으로 확인한 아이템 데이터

        if (data.canStack)  // 해당 아이템이 겹칠 수 있을 경우
        {
            ItemSlot slot = GetItemStack(data); // 해당 데이터가 있는 슬롯
            if (slot != null)
            {
                slot.quantity++;    // 슬롯에 아이템 개수 증가
                UpdateUI();         // UI 초기화
                CharacterManager.Instance.Player.itemData = null;   // 상호작용으로 확인한 아이템 데이터 제거
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();    // 빈 슬롯

        if (emptySlot != null)
        {
            emptySlot.item = data;  // 빈슬롯에 아이템 저장
            emptySlot.quantity = 1; // 아이템 개수 1로 저장
            UpdateUI();     // UI 초기화
            CharacterManager.Instance.Player.itemData = null;   // 상호작용으로 확인한 아이템 데이터 제거
            return;
        }

        ThrowItem(data);    // 아이템을 넣을 슬롯이 없을 경우 아이템을 바닥에 던짐
        CharacterManager.Instance.Player.itemData = null;   // 상호작용으로 확인한 아이템 데이터 제거
    }

    void UpdateUI()     // 인벤토리 UI 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) 
            {
                slots[i].Set();    
            }
            else
            {
                slots[i].Clear();  
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)  
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)   // 슬롯에 있는 아이템과 같고 최대 저장개수보다 개수가 적을 경우
            {
                return slots[i]; // 해당 아이템 슬롯 반환
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)  // 슬롯에 아이템이 없을 경우
            {
                return slots[i];    // 빈 슬롯 반환
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)   
    {
        // 아이템을 정해진 위치에 떨어뜨림
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;  // 해당 인덱스 슬롯에 아이템이 없을 경우 리턴

        selectedItem = slots[index].item;   // 선택한 슬롯의 아이템 저장
        selectedItemIndex = index;          // 선택한 슬롯 인덱스 저장

        selectedItemName.text = selectedItem.displayName;   // 선택한 아이템 이름 표시
        selectedItemDescription.text = selectedItem.description;    // 선택한 아이템 설명 표시

        // 선택한 아이템 스탯 정보 초기화
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++) // 아이템 스탯 정보 표시
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";    
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);  // 소모형 아이템일 경우 사용 버튼 활성화

        // 아이템 장착 여부에 따라 버튼 활성화
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);

        dropButton.SetActive(true); // 떨어뜨리기 버튼 활성화
    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)   // 소모형 아이템일 경우
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)   // 아이템의 스탯(체력, 스테미나) 타입에 따라 그에 맞는 상태값 회복
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        condition.AddStamina(selectedItem.consumables[i].value);
                        break;
                }
            }

            RemoveSelectedItem();   // 사용한 아이템 제거
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);    // 선택한 아이템 떨구기
        RemoveSelectedItem();       // 떨군 아이템 제거
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;    // 해당 슬롯의 아이템 개수 줄이기

        if (slots[selectedItemIndex].quantity <= 0) // 아이템 개수가 0보다 작거나 같아졌을 경우
        {
            selectedItem = null;    // 선택한 아이템 제거
            slots[selectedItemIndex].item = null;   // 해당 슬롯의 아이템 제거
            selectedItemIndex = -1; 
            ClearSelectedItemWindow();  // 선택한 아이템 정보 UI창 초기화
        }

        UpdateUI(); // 인벤토리 UI 초기화
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex); // 이미 장착한 아이템 해제
        }

        slots[selectedItemIndex].equipped = true; // 선택한 슬롯 아이템 장착 상태로 전환
        curEquipIndex = selectedItemIndex;  // 현재 아이템 인덱스를 장착된 아이템 슬롯의 인덱스에 저장
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);  // 실제 아이템 장착
        UpdateUI();

        SelectItem(selectedItemIndex);  // 아이템 정보 표시
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;  // 장착되어 있던 아이템 해제
        CharacterManager.Instance.Player.equip.UnEquip();   // 실제 아이템 해제
        UpdateUI(); // 인벤토리 UI 초기화

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);  // 아이템 정보 표시
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex); // 장착한 아이템 해제
    }
}
