using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponParent : MonoBehaviour
{

    public WeaponScript weapon;
    public SpriteRenderer characterRenderer, weaponRenderer;
    public int Combo;
    public TrailRenderer trail;


    public Vector2 Pointerposition { get; set; }

    public Animator animator;
    public float AttackDelay = 0.3f;
    public bool attackBlocked;

    public bool IsAttacking { get; private set; }

    [SerializeField] Transform circleOrigin;

    [Header("Buffed Stats")]
    public int tempStrength;
    public int tempCritChance;
    public int tempCritDamage;
    public float tempHealth;
    public int tempDefense;

    public float tempMana;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }
    private float tempDPS;
    float sec = 1f;

    public float CalculateDPS(int TotalDamage)
    {
        if (sec > 0)
        {
            sec -= Time.deltaTime;
            
        }
        else
        {
            tempDPS += TotalDamage;
            DamageIndicator.GetComponent<Text>().text = "DPS: " +tempDPS.ToString();
            sec = 1;
            tempDPS = 0;
        }

        return tempDPS;
    }

    public void ChangeWeaponVisualisation()
    {
        if (weapon != null)
        {
            if (weapon.weaponType == WeaponType.RANGED && attackBlocked == true && weapon.isBoomerang == true)
            {
                weaponRenderer.sprite = null;
            }
            else
            {
                
                if (charge == 0)
                {
                    weaponRenderer.sprite = weapon.sprite;
                }
            }
            animator.runtimeAnimatorController = weapon.WeaponAnimation;

        }
        
    }
    public int TotalDamage;
    public void Update()
    {
        ChangeWeaponVisualisation();
        TotalDamage = (Mathf.RoundToInt(DamageCalculator(weapon.damage, weapon.strength * (tempStrength/100 + 1), weapon.critChance * (tempCritChance / 100 + 1), weapon.critDamage * (tempCritDamage / 100 + 1), characterRenderer.gameObject.GetComponentInParent<PlayerController>().mana, characterRenderer.gameObject.GetComponentInParent<PlayerController>().maxMana + weapon.mana )));
        CalculateDPS(TotalDamage);



        if (IsAttacking)
            return;
        if (characterRenderer.GetComponentInParent<PlayerController>().isRolling == true)
            return;
        Vector2 direction = (Pointerposition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;

        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 4;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 4;
        }
    }
    public void ResetCombo()
    {
        if (Combo >= weapon.maxCombo || Combo <= 0)
        {
            Combo = 1;
        }
        else
        {
            
            Combo += 1;
        }
    }

    [SerializeField] GameObject DamageIndicator;
    public float DamageCalculator(int damage, int strength, int critchance, int critdamage, float mana, float maxmana)
    {
        float TotalDamage = 5;

        if (weapon.weaponType == WeaponType.MAGIC)
        {
            TotalDamage = damage * (maxmana/100) ;
        }
        else
        {
            TotalDamage = (damage + strength) * (1 + strength / 100);
        }
        int chance = UnityEngine.Random.Range(0, 100);
        if (chance <= critchance)
        {
            TotalDamage = TotalDamage * (2 + (critdamage / 100));
        }

        return TotalDamage;
    }

    public GameObject ShootProjectile(float _speed, int damage, float rotation)
    {
        GameObject projectile = Instantiate(weapon.Projectile, trail.transform.position, transform.rotation);
        projectile.transform.eulerAngles += new Vector3(0,0,rotation);
        projectile.GetComponent<Projectile>().shooter = gameObject;
        projectile.GetComponent<Projectile>().speed = _speed;
        projectile.GetComponent<Projectile>().isBoomerang = weapon.isBoomerang;
        projectile.GetComponent<Projectile>().AOEDistance = weapon.AOEDistance;
        projectile.GetComponent<Projectile>().comebackTime = weapon.boomerangComeBackTimer;
        projectile.GetComponent<Projectile>().totalDamage = damage;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * projectile.GetComponent<Projectile>().speed, ForceMode2D.Impulse);
        return projectile;
    }

    public bool checkForEnoughMana(int mana)
    {
        bool hasMana = false;
        if (characterRenderer.gameObject.GetComponentInParent<PlayerController>().mana - mana >= 0)
        {
            characterRenderer.gameObject.GetComponentInParent<PlayerController>().mana -= mana;
            hasMana = true;
        }
        else
        {
            hasMana = false;
        }

        return hasMana;
    }


    public GameObject swing;
    public GameObject swingPos;


    public void Attack()
    {
        if (attackBlocked)
            return;
        ResetCombo();
        animator.SetTrigger("Attack"+Combo);
        IsAttacking = true;
        attackBlocked = true;
        if (weapon.weaponType == WeaponType.MAGIC)
        {
            if (checkForEnoughMana(weapon.manaCost) == true)
            {
                ShootProjectile(weapon.projectileSpeed, TotalDamage,0);
                StartCoroutine(DelayAttack());
            }
        }
        else if(weapon.weaponType == WeaponType.RANGED)
        {
            ShootProjectile(weapon.projectileSpeed, TotalDamage, 0);
        }
        else
        {
            int i = Random.Range(0,1);
            if (i == 0)
            {
                SoundManager.i.Swing0.Play();
            }
            else
            {
                SoundManager.i.Swing1.Play();
            }
            
            StartCoroutine(DelayAttack());
        }
       
    }
    [SerializeField] float charge;

    public void ChargeArrowAttack()
    {
        float percentage = (charge / weapon.maxArrowPullCharge) * 100;


        if (charge < weapon.maxArrowPullCharge)
        {
            charge += Time.deltaTime;
            if (percentage >= 25 && percentage < 50)
            {
                weaponRenderer.sprite = weapon.Pull0Sprite;
                Debug.Log("25%");
            }else if (percentage >= 50 && percentage < 75)
            {
                weaponRenderer.sprite = weapon.Pull1Sprite;
                Debug.Log("50%");
            }
            else if (percentage >= 75)
            {
                Debug.Log("75%");
                weaponRenderer.sprite = weapon.Pull2Sprite;
            }
        }

    }
    public void ShootArrow()
    {
        float percentage = (charge / weapon.maxArrowPullCharge) * 100;
        
        if (percentage >= 25)
        {
            SoundManager.i.ArrowShoot.Play();
            ShootProjectile(weapon.projectileSpeed * (percentage / 100 + 1), (int)(TotalDamage * (percentage / 100 + 1)), 0);
            for (int i = 1; i <= weapon.extraArrows -1; i++)
            {
                if (i <= weapon.extraArrows/2)
                {
                    ShootProjectile(weapon.projectileSpeed * (percentage / 100 + 1), (int)(TotalDamage * (percentage / 100 + 1)), weapon.angleBetweenArrows * -i);
                    Debug.Log("Negative");
                }
                else
                {
                    ShootProjectile(weapon.projectileSpeed * (percentage / 100 + 1), (int)(TotalDamage * (percentage / 100 + 1)), weapon.angleBetweenArrows *(weapon.extraArrows - i));
                    Debug.Log("positive");
                }

            }
            charge = 0;
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds((float)(100 - weapon.attackSpeed) / 100);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, weapon.AOEDistance);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, weapon.AOEDistance))
        {
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(TotalDamage, transform.parent.gameObject);
            }
        }
        if (weapon.hasSwingEffect == true)
        {
            GameObject swin = Instantiate(swing, swingPos.transform.position, transform.rotation);
            swin.GetComponent<SpriteRenderer>().color = weapon.swingColor;
            Destroy(swin, 0.1f);
        }
    }
    

}
