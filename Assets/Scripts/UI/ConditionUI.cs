using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    void Start()
    {
        CharacterManager.Instance.Player.condition.conditionUI = this;
    }
}
