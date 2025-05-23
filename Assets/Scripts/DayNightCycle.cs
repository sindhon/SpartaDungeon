using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;              // 현재 시간
    public float fullDayLength;     // 하루 길이
    public float startTime = 0.4f;  // 시작 시간
    private float timeRate;         // 시간 진행 속도 비율
    public Vector3 noon;            // 정오일 때 태양/달의 회전 방향

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;   // 시간에 따른 태양의 색
    public AnimationCurve sunIntensity;     // 태양빛 강도

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;  // 시간에 따른 달의 색
    public AnimationCurve moonIntensity;    // 달빛 강도

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;      // 환경광 강도
    public AnimationCurve reflectionIntensityMultiplier;    // 반사광 강도

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;    // 하루 길이에 따른 시간 진행 속도 계산
        time = startTime;                   // 초기 시간 설정
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;   // 시간 진행 (0 ~ 1 순환)

        // 태양과 달의 색, 밝기 업데이트
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // 시간에 따라 환경광과 반사광의 강도 조절
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);    // 시간에 따라 빛의 강도 조절

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f; // 광원의 회전 설정 (태양은 0.25에서 90도, 달은 0.75에서 90도)
        lightSource.color = colorGradiant.Evaluate(time);   // 색 설정
        lightSource.intensity = intensity;                  // 밝기 설정

        GameObject go = lightSource.gameObject;

        // 광원의 강도가 0이면 비활성화, 0보다 크면 활성화
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
}
