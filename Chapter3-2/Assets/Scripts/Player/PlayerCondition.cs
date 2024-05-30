using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public bool _isUsedItemSpeed;
    public bool _isUsedItemJump;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    private void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void GetUsedItemSpeed(bool isUsed, float applyingTime)
    {
        _isUsedItemSpeed = isUsed;
        StartCoroutine("isUsedItemSpeed", applyingTime);
    }

    public void GetUsedItemJump(bool isUsed, float applyingTime)
    {
        _isUsedItemJump = isUsed;
        StartCoroutine("isUseItemJump", applyingTime);
    }

    IEnumerator isUsedItemSpeed(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        PlayerManager.Instance.Player.controller.UseConsumableItemSpeed = 0;
        _isUsedItemSpeed = false;
    }

    IEnumerator isUseItemJump(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        PlayerManager.Instance.Player.controller.UseConsumableJump = 0;
        _isUsedItemJump = false;
    }
}
