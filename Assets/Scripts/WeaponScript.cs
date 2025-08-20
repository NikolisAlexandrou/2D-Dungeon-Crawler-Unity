using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    MELEE,
    RANGED,
    MAGIC
}



[CreateAssetMenu(menuName = "Create New Weapon", fileName = "New Weapon")]
public class WeaponScript : ScriptableObject
{
    [Header("Stats")]
    public string weaponName;
    [TextArea]
    public string Description;

    public int maxCombo;

    public WeaponType weaponType;

    public int damage;
    public int strength;
    public int critChance;
    public int critDamage;
    public int attackSpeed;
    public int mana;
    public float AOEDistance;

    [Header("Ability")]
    public bool hasAbility;
    public int manaCost;
    public int cooldown;

    [Header("Magic Items Only")]
    public GameObject Projectile;
    public float projectileSpeed;
    public bool isBoomerang;
    public bool isArrow;
    public int extraArrows;
    public float angleBetweenArrows;
    public float maxArrowPullCharge;
    public float boomerangComeBackTimer;

    [Header("Misc")]
    public Sprite sprite;
    public Sprite Pull0Sprite;
    public Sprite Pull1Sprite;
    public Sprite Pull2Sprite;
    public RuntimeAnimatorController WeaponAnimation;

    [Header("Effects")]
    public bool hasSwingEffect;
    public Color32 swingColor;
    
}
