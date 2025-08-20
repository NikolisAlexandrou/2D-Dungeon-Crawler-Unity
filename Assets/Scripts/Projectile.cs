using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public float AOEDistance;

    public GameObject shooter;

    public bool isBoomerang;

    Rigidbody2D rb;

    [SerializeField] bool isRotating;

    [SerializeField] float rotateSpeed;

    float timer;
    public float DestroyTimer;
    public float comebackTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    private void Update()
    {
        if (shooter.gameObject == null || shooter == null)
        {
            Destroy(gameObject);
        }
        if (shooter.gameObject == null)
            return;

        if (isBoomerang == true)
        {
            if (timer <= comebackTime)
            {
                timer += Time.deltaTime;
               
            }
            else
            {
                
                rb.AddForce((transform.position - shooter.transform.position) * -1, ForceMode2D.Impulse);
                
            }
        }
        else
        {
            if (timer < DestroyTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        Rotate();


    }
    public int totalDamage;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Health>() != null)
        {
            if (  timer > comebackTime && isBoomerang == true)
            {
                if (shooter.GetComponent<WeaponParent>() != null)
                {
                    if (shooter.transform.parent.gameObject == collider.gameObject)
                    {
                        shooter.GetComponent<WeaponParent>().attackBlocked = false;
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (shooter.transform.gameObject == collider.gameObject)
                        Destroy(gameObject);
                }

               
            }
            collider.gameObject.GetComponent<Health>().GetHit(totalDamage, shooter.gameObject);
        }
    }

   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = transform.position == null ? Vector3.zero : transform.position;
        Gizmos.DrawWireSphere(position, AOEDistance);
    }
    void Rotate()
    {
        if (isRotating == true)
        {
            gameObject.transform.eulerAngles += new Vector3(0, 0, rotateSpeed*Time.deltaTime );
        }
    }
}
