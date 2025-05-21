using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [SerializeField] private float curValue;
    [SerializeField] private float startValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float passiveValue;
    [SerializeField] private Image uiBar;

    public float CurValue { get { return curValue; } }
    public float PassiveValue { get { return passiveValue; } }

    void Start()
    {
        curValue = startValue;
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0f);
    }
}
