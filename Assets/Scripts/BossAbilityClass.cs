using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAbilityClass : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    public Ability spawnMinions;
    public Ability dashAbility;
    public List<Ability> abilities;

    public EnemyAI enemy;


    public bool hasCooldown;
    public bool isUsingAbility;
    public float abilityCooldown;
    public float abilityCooldownTimer;


    public float StartAttacks;
    public bool canStartAttacks;
    public bool isCollider;
    float abilityDurationTimer;
    bool attackCD;


    private void Start()
    {
        abilityDurationTimer = 0;
        uiManager.bossBarHolder.SetActive(true);
    }

    private void OnDisable()
    {
        uiManager.bossBarHolder.SetActive(false);
    }

    public void UpdateBossBar()
    {
        float percHealth = GetComponent<Health>().currentHealth / GetComponent<Health>().maxHealth;
        uiManager.bossBar.GetComponent<Image>().fillAmount = percHealth;
    }

    private void FixedUpdate()
    {
        UpdateBossBar();
        if (canStartAttacks == false)
        {
            StartCoroutine(StartAttack());
        }
        else
        {
            if (hasCooldown == false)
            {
                if (isUsingAbility == false)
                {
                    enemy.IsStunned = true;
                    isUsingAbility = true;
                    int i = Random.Range(0, abilities.Count);
                    enemy.Attackanim.SetTrigger(abilities[i].name);

                }
            }
            if (abilityDurationTimer > 0)
            {
                abilityDurationTimer -= Time.deltaTime;

                gameObject.GetComponent<Rigidbody2D>().AddForce(enemy.WeaponHolder.right * dashAbility.abilitySpeed, ForceMode2D.Impulse);
                if (attackCD == false)
                {
                    enemy.Attackanim.SetTrigger("Attack1");
                    enemy.Attackanim.speed = 2f;
                    attackCD = true;
                    //enemy.DetectColliders();
                    StartCoroutine(AttackCD(dashAbility));
                }
            }
            else
            {
                enemy.Attackanim.speed = enemy.attackAnimSpeed;
            }
        }
    }

    public void SpawnMinions()
    {
        if (hasCooldown == false)
        {
            if (isUsingAbility == true)
            {
                hasCooldown = true;
                isUsingAbility = false;
                enemy.IsStunned = false;
                StartCoroutine(cooldown());
                for (int i = 0; i <= 2; i++)
                {

                    var position = new Vector3(Random.Range(transform.position.x - 5, transform.position.x + 5), Random.Range(transform.position.y - 5, transform.position.y + 5), 0);
                    /*if (Physics2D.Raycast(position, Vector2.up).collider != null)
                    {
                        isCollider = false;
                    }
                    else
                    {
                        isCollider = true;
                    }
                    if (isCollider == true)
                    {
                        
                    }*/
                    int chance = Random.Range(0 , spawnMinions.minions.Length);
                    GameObject ob1 = Instantiate(spawnMinions.minions[chance], position, Quaternion.identity);
                    ob1.GetComponent<EnemyAI>().player = enemy.player;
                }                
            }
        }
    }
    //FIX THE FUCKING PROBLEM
    public void DashForward()
    {
        abilityDurationTimer = dashAbility.abilityDuration;
        if (hasCooldown == false)
        {
            if (isUsingAbility == true)
            {
                hasCooldown = true;
                
                StartCoroutine(ResetAbility(dashAbility));
                StartCoroutine(cooldown());
            }
        }
    }
    IEnumerator ResetAbility(Ability ability)
    {
        yield return new WaitForSeconds(ability.abilityDuration);
        isUsingAbility = false;
        enemy.IsStunned = false;
    }
    IEnumerator AttackCD(Ability ability)
    {
        yield return new WaitForSeconds(ability.attackCDTimer);
        attackCD = false;
    }
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(StartAttacks);
        canStartAttacks = true;
        
    }
    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        hasCooldown = false;
    }
}
