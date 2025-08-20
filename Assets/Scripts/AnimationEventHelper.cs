using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPerformed, ComboAttackMovePosition, OnSpawnMinions, OnDashForward;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttackPerformed?.Invoke();
    }

    public void TriggerComboMovePosition()
    {
        ComboAttackMovePosition?.Invoke();
    }

    public void SpawnMinions()
    {
        OnSpawnMinions?.Invoke();
    }
    public void DashForward()
    {
        OnDashForward?.Invoke();
    }
}



