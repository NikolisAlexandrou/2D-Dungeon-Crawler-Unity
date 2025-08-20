using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Create New Blessing", fileName = "New Blessing")]
public class Blessings : ScriptableObject
{
    public Sprite sprite;
    public string Name;

    [Header("Damage Stats")]
    public float minStrengthMultiplier;
    public float maxStrengthMultiplier;

    public float minCritChanceMultiplier;
    public float maxCritChanceMultiplier;

    public float minCritDamageMultiplier;
    public float maxCritDamageMultiplier;

    [Header("Util Stats")]
    public float minManaMultiplier;
    public float maxManaMultiplier;

    public float minHealthMultiplier;
    public float maxHealthMultiplier;

    public float minDefenseMultiplier;
    public float maxDefenseMultiplier;
}