using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    CLOSE_COMBAT,
    MAGE,
    ARCHER,
    HEALDER,
    TANK

}
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField] EnemyType type;
    public float animSPeed;
    public float attackAnimSpeed;

    [Header("Stats")]
    public float speed;
    public int totalDamage;
    [SerializeField] int maxCombo;
    private int Combo;
    public SpriteRenderer[] AllSprites;
    public Transform player;
    public Transform WeaponHolder;


    [Header("Attacking")]
    [SerializeField] Transform circleOrigin;
    [SerializeField] float attackRadius;
    public Animator anim;
    public Animator Attackanim;
    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private float attackDelay = 1f;
    private float passedTime = 1f;
    [SerializeField]
    private float chargeAttackDelay = 1f;
    private float chargeAttackTimer = 1f;
    private bool performedFirstAttack;

    [Header("Magic Attacking")]
    [SerializeField] GameObject ProjectileToShoot;
    [SerializeField] float projSpeed;
    [SerializeField] bool isBoomerang;
    [SerializeField] float boomerangComeBackTimer;


    [Header("Moving")]
    [SerializeField] GameObject footsteps;
    [SerializeField] GameObject feet;
    bool spawnSteps;
    [SerializeField]
    private float chaseDistanceThreshold = 3, attackDistanceThreshold = 0.8f;
    public bool IsStunned;

  


    public Color originalColour;
    bool following = false;
    private void Start()
    {
        anim.speed = animSPeed;
        Attackanim.speed = attackAnimSpeed;
        originalColour = GetComponentInChildren<SpriteRenderer>().color;
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }
    private void FixedUpdate()
    {
        
        if (player == null)
            return;
        if (IsStunned == true)
            return;
        if (GetComponent<BossAbilityClass>() != null && GetComponent<BossAbilityClass>().isUsingAbility == true)
            return;

        //Move();
        RotateEnemy();
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }

        if (chargeAttackTimer < chargeAttackDelay)
        {
            chargeAttackTimer += Time.deltaTime;
        }

        RotateWeaponToPlayer();
        if (aiData.currentTarget != null)
        {
            //Looking at the Target
            //OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the Agent
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movementInput.x * Time.deltaTime, speed * movementInput.y * Time.deltaTime);
        if (anim.GetBool("isMoving") == true)
        {
            ChangeLegAnimation();
        }
        else
        {
            legSpriteRenderer.sprite = legSprites[0];
            turn = 0;
        }
        //OnMovementInput?.Invoke(movementInput);
    }

    [SerializeField] SpriteRenderer legSpriteRenderer;
    [SerializeField] Sprite[] legSprites;
    public float ticks = 10;
    public float timer = 0;
    public int turn;
    public int maxturns;

    public void RotateWeaponToPlayer()
    {
        var dx = WeaponHolder.position.x - player.transform.position.x;
        var dy = WeaponHolder.position.y - player.transform.position.y;
        var radians = Mathf.Atan2(dy, dx);
        var angle = radians * 180 / Mathf.PI - 90;

        var rotateZ = Mathf.LerpAngle(WeaponHolder.rotation.z, angle, Time.time);

        WeaponHolder.eulerAngles = new Vector3(WeaponHolder.rotation.x, WeaponHolder.rotation.y, rotateZ - 95);
    }

    private void ChangeLegAnimation()
    {
        maxturns = legSprites.Length;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = ticks;

            if (turn < (maxturns - 1))
            {
                turn += 1;
            }
            else
            {
                turn = 0;
            }
            legSpriteRenderer.sprite = legSprites[turn];
        }
    }
    private void RotateEnemy()
    {
        Vector2 mousePos = new Vector2(player.position.x, player.position.y);
        float dist = mousePos.x;


        if (dist > transform.position.x)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (dist < transform.position.x)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            anim.SetBool("isMoving", false);
            yield break;
        }
        else
        {

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistanceThreshold)
            {
                //Attack logic
                movementInput = Vector2.zero;
                anim.SetBool("isMoving", false);
                if (IsStunned == false)
                {
                    PerformAttack();
                }
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                if (performedFirstAttack == true)
                {
                    performedFirstAttack = false;
                }
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                anim.SetBool("isMoving", true);
                if (spawnSteps == false)
                {

                    spawnSteps = true;
                    int i = Random.Range(0, 1);
                    if (i == 0)
                    {
                        SoundManager.i.Step0.Play();
                    }
                    else
                    {
                        SoundManager.i.Step1.Play();
                    }
                    GameObject ob = Instantiate(footsteps, feet.transform.position, Quaternion.identity);

                    Destroy(ob, 0.4f);
                    StartCoroutine(SpawnFootSteps());
                }
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
                
            }

        }

    }
    public void Move()
    {
      
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance < chaseDistanceThreshold && isAttacking == false)
        {
            if (distance <= attackDistanceThreshold)
            {
                //attack
                if (passedTime >= attackDelay)
                {
                    passedTime = 0;
                    if (IsStunned == true)
                        return;
                    PerformAttack();
                }
            }
            else
            {
               
                if (performedFirstAttack == true)
                {
                    performedFirstAttack = false;
                }
                //chasing player
                if (isAttacking == true)
                    return;
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            }
        }
        if (Vector2.Distance(player.position, transform.position) <= attackDistanceThreshold)
        {
            legSpriteRenderer.sprite = legSprites[0];
            turn = 0;
            anim.SetBool("isMoving", false);
        }
        else
        {
            if (isAttacking == true)
                return;

            ChangeLegAnimation();
            anim.SetBool("isMoving", true);
            if (spawnSteps == false)
            {
                
                spawnSteps = true;
                int i = Random.Range(0, 1);
                if (i == 0)
                {
                    SoundManager.i.Step0.Play();
                }
                else
                {
                    SoundManager.i.Step1.Play();
                }
                GameObject ob = Instantiate(footsteps, feet.transform.position, Quaternion.identity);
               
                Destroy(ob, 0.4f);
                StartCoroutine(SpawnFootSteps());
            }
        }
    }

    IEnumerator SpawnFootSteps()
    {
        yield return new WaitForSeconds(0.2f);
        spawnSteps = false;
    }
    public void ResetCombo()
    {
        if (Combo >= maxCombo || Combo <= 0)
        {
            Combo = 1;
        }
        else
        {

            Combo += 1;
        }
    }
    public void PerformAttack()
    {
        ResetCombo();
        if (type == EnemyType.CLOSE_COMBAT)
        {
            if (performedFirstAttack == false)
            {
                performedFirstAttack = true;
                Attackanim.SetTrigger("HeavyAttack");
                StartCoroutine(resetAttack(chargeAttackDelay));
            }
            else
            {
                Attackanim.SetTrigger("Attack" + Combo);
                StartCoroutine(resetAttack(attackDelay));
            }
        }
        else if (type == EnemyType.MAGE)
        {
            Attackanim.SetTrigger("MagicAttack");
            //SPawn Projectile
            ShootProjectile();
            StartCoroutine(resetAttack(attackDelay));
        }
       
        isAttacking = true;
        //DetectColliders();
        
    }

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(ProjectileToShoot, circleOrigin.transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().shooter = gameObject;
        projectile.GetComponent<Projectile>().speed = projSpeed;
        projectile.GetComponent<Projectile>().isBoomerang = isBoomerang;
        projectile.GetComponent<Projectile>().AOEDistance = attackRadius;
        projectile.GetComponent<Projectile>().comebackTime =boomerangComeBackTimer;
        projectile.GetComponent<Projectile>().totalDamage = totalDamage;
        projectile.GetComponent<Rigidbody2D>().AddForce(WeaponHolder.right * projSpeed, ForceMode2D.Impulse);
    }

    IEnumerator resetAttack(float sec)
    {
        yield return new WaitForSeconds(sec);
        isAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, attackRadius);
    }
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, attackRadius))
        {
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                if (performedFirstAttack == true)
                {
                    health.GetHit(totalDamage * 2, transform.gameObject);
                }
                else
                {
                    health.GetHit(totalDamage, transform.gameObject);
                }
            }
        }
    }
}
