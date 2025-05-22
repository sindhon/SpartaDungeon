using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable  // 상호작용 가능한 오브젝트들이 반드시 구현해야 하는 인터페이스
{
    public string GetInteractPrompt();  // 플레이어에게 상호작용 시 표시할 텍스트
    public void OnInteract();   // 상호작용 되었을 때 호출될 함수
}

public class Item : MonoBehaviour, IInteractable
{
    public ItemData data;   // 아이템 정보

    public string GetInteractPrompt()
    {
        // 아이템의 이름과 설명을 반환
        string str = $"{data.displayName}\n{data.description}"; 
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data; // 아이템의 데이터를 전달
        CharacterManager.Instance.Player.addItem?.Invoke(); // 인벤토리에 추가
        Destroy(gameObject);    // 해당 아이템 제거
    }
}
