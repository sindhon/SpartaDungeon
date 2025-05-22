using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public Image image;         // 데미지를 받았을 때 깜빡일 이미지
    public float flashSpeed;    // 이미지가 사라지는 속도

    private Coroutine coroutine;    

    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;   // 데미지 받을 경우 해당 함수 실행
    }

    public void Flash()
    {
        if (coroutine != null)  // 이미 실행 중인 코루틴이 있을 경우
        {
            StopCoroutine(coroutine);   // 코루틴 중지
        }

        image.enabled = true;   // 이미지 활성화
        image.color = new Color(1f, 100 / 255f, 100 / 255f);
        coroutine = StartCoroutine(FadeAway());     // 이미지가 점점 사라지도록 하는 코루틴 시작
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;    //시작 알파 값
        float a = startAlpha;       // 현재 알파 값

        while (a > 0)   // 알파값이 0이 될 때까지 반복
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;    // 점점 투명하게 알파값 조정
            image.color = new Color(1f, 100 / 255f, 100 / 255f, a);     // 알파값 적용
            yield return null;
        }

        image.enabled = false;  // 완전히 투명해질 경우 이미지 비활성화
    }
}
