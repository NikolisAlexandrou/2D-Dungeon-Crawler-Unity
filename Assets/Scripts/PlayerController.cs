using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;
    public float maxMana;
    public float mana;
    [SerializeField] bool isMoving;
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject Footsteps;
    [SerializeField] GameObject Feet;
    [SerializeField] Sprite[] legSprites;

    

    Rigidbody2D rb;
    Animator anim;
    public WeaponParent wp;
    float x;
    float y;

    public void UpdateHealth()
    {
        GetComponent<Health>().maxHealth *= (wp.tempHealth / 100) + 1;
    }
    public void UpdateMana()
    {
        maxMana *= (wp.tempMana / 100) + 1;
    } 

    public void UpdateStatsOnUI()
    {
        
        float percHealth = GetComponent<Health>().currentHealth / GetComponent<Health>().maxHealth;
        uiManager.healthBar.GetComponent<Image>().fillAmount = percHealth;

        float percMana = mana / maxMana;
        uiManager.ManaBar.GetComponent<Image>().fillAmount = percMana;
    }

    [SerializeField] SpriteRenderer legSpriteRenderer;
    public float ticks = 10;
    public float timer = 0;
    public int turn;
    public int maxturns;
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


    private void Start()
    {
        wp = GetComponentInChildren<WeaponParent>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mana = maxMana;
    }

    private void FixedUpdate()
    {
        
        UpdateStatsOnUI();
        wp.Pointerposition = GetPointerInput();
        Roll();
        if (isRolling == true)
            return;
        Move();
        RotatePlayer();
        PerformAttack();


    }

    [SerializeField] float RollSpeed;
    [SerializeField] float RollCooldown;
    [SerializeField] float RollCooldownTimer;
    bool hasRollCooldown;
    public bool isRolling;
    [SerializeField] float rollDuration;
    [SerializeField] float rollTimer = 1f;
    public void Roll()
    {
        if (hasRollCooldown == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isRolling == false)
                {
                    rb.velocity = new Vector2(0, 0);
                    RollCooldownTimer = RollCooldown;
                    hasRollCooldown = true;
                    wp.ShootArrow();
                    isRolling = true;
                    rollTimer = 0f;
                }

            }
        }
        if (hasRollCooldown == true)
        {
            if (RollCooldownTimer > 0)
            {
                RollCooldownTimer -= Time.deltaTime;
            }
            else
            {
                hasRollCooldown = false;
            }
        }
        if (isRolling == true)
        {
            
            if (rollTimer >= 0 && rollTimer <= rollDuration)
            {
                rb.AddForce(wp.transform.right * RollSpeed, ForceMode2D.Impulse);
                rollTimer += Time.deltaTime;
            }
            else
                 if (rollTimer > rollDuration)
            {
                isRolling = false;
            }
        }
        float percRoll = RollCooldownTimer / RollCooldown;
        uiManager.RollBar.GetComponent<Image>().fillAmount = percRoll;
    }

    private void RotatePlayer()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.z, 10);
        Vector3 dist = Camera.main.ScreenToWorldPoint(mousePos);
        dist = dist - transform.position;


        if (dist.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (dist.x < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    public bool freeze;
    bool spawnSteps;
    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (freeze == false && isRolling == false)
        {
            rb.velocity = new Vector2(speed * x * Time.deltaTime, speed * y * Time.deltaTime);
        }
        if (x != 0 || y != 0)
        {
            isMoving = true;
            ChangeLegAnimation();
            anim.SetBool("isMoving", true);
            if (spawnSteps == false)
            {
                spawnSteps = true;
                GameObject ob = Instantiate(Footsteps, Feet.transform.position, Quaternion.identity);
                Destroy(ob, 0.4f);
                StartCoroutine(SpawnFootSteps());
            }
        }
        else if (x < 1 && y < 1)
        {
            isMoving = false;
            anim.SetBool("isMoving", false);
            turn = 0;
            legSpriteRenderer.sprite = legSprites[0];
        }
    }

    IEnumerator SpawnFootSteps()
    {
        yield return new WaitForSeconds(0.2f);
        spawnSteps = false;
    }

    private void PerformAttack()
    {
        if (wp.weapon.isArrow == true)
        {
            if (Input.GetMouseButton(0))
            {
                //charge attack
                wp.ChargeArrowAttack();
            }
            if (Input.GetMouseButtonUp(0))
            {
                //shoot arrow
                wp.ShootArrow();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                wp.Attack();
            }
        }
    }

}