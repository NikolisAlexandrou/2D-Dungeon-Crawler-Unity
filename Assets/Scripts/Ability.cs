using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Create New Ability", fileName = "New Ability")]
public class Ability : ScriptableObject
{
    [Header("Ability")]
    public string name;

    public float distance;

    public bool isUsingAbility;
    public float abilityChargeDelay;


    public float abilityDuration;
    public float abilitySpeed;


    public bool hasCooldown;
    public float abilityCooldown;

    public bool attackCD;
    public float attackCDTimer;

    public GameObject[] minions;
}
