using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType    // 아이템 타입
{
    Consumable, // 소모 아이템
    Equipable   // 장비 아이템
}

public enum ConsumableType  // 소모 아이템 타입
{
    Health,
    Stamina
}

[Serializable]
public class ItemDataConsumable // 소모 아이템 정보
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;      // 아이템 이름
    public string description;      // 아이템 설명
    public ItemType type;           // 아이템 타입
    public Sprite icon;             // 아이템 아이콘
    public GameObject dropPrefab;   // 드롭할 아이템

    [Header("Stacking")]
    public bool canStack;           // 여러 개를 가질 수 있는지 확인
    public int maxStackAmount;      // 겹쳐서 가질 수 있는 최대 아이템 개수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    // 소모 아이템 정보들

    [Header("Equip")]
    public GameObject equipPrefab;  // 장착할 아이템
}
