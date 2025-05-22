using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;         // 아이템 확인 주기
    private float lastCheckTime;            // 마지막으로 확인한 시간
    public float maxCheckDistance;          // 아이템 확인 거리
    public LayerMask layerMask;             // 아이템을 확인하기 위한 레이어 마스크

    public GameObject curInteractGameObject;    // 현재 확인하고 있는 아이템
    private IInteractable curInteractable;      // 

    public TextMeshProUGUI promptText;      // 아이템 정보 표시용 텍스트
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)  // 마지막으로 확인한 시간으로부터 걸린 시간이 확인 주기를 지났을 경우
        {
            lastCheckTime = Time.time;  // 마지막으로 확인한 시간을 현재 시간으로 저장

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // 화면의 중앙에서 쏘는 레이
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // 아이템 확인 거리의 레이에 아이템이 있을 경우
            {
                if (hit.collider.gameObject != curInteractGameObject)   // 현재 확인하고 아이템이 아닐 경우
                {
                    curInteractGameObject = hit.collider.gameObject;    // 현재 확인 아이템을 바꿈
                    curInteractable = hit.collider.GetComponent<IInteractable>();   
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;

                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // 확인한 아이템의 정보 텍스트 활성화
        promptText.text = curInteractable.GetInteractPrompt();  // 정보 텍스트 표시
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
