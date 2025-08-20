using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlessingListener : MonoBehaviour
{
    [SerializeField] Blessings[] blessings;
    private Blessings blessing;
    [SerializeField] float movingSpeed;
    [SerializeField] float maxPointY;
    [SerializeField] float minPointY;
    private float normal;
    private bool hasReachedPoint = false;

    void Start()
    {
        blessing = blessings[Random.Range(0, blessings.Length )];


        GetComponent<SpriteRenderer>().sprite = blessing.sprite;
        maxPointY = transform.position.y + maxPointY;
        minPointY = transform.position.y - minPointY;
        normal = transform.position.x;
    }

    void Move()
    {
        if (hasReachedPoint == false)
        {
            transform.position += new Vector3(0, movingSpeed * Time.deltaTime, 0);
            if (transform.position.y >= maxPointY)
            {
                hasReachedPoint = true;
            }
        }
        if (hasReachedPoint == true)
        {
            transform.position += new Vector3(0, -movingSpeed * Time.deltaTime, 0);
            if (transform.position.y <= minPointY)
            {
                hasReachedPoint = false;
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().wp.tempStrength += (int)Random.Range(blessing.minStrengthMultiplier, blessing.maxStrengthMultiplier);
            collision.GetComponent<PlayerController>().wp.tempCritChance += (int)Random.Range(blessing.minCritChanceMultiplier, blessing.maxCritChanceMultiplier);
            collision.GetComponent<PlayerController>().wp.tempCritDamage += (int)Random.Range(blessing.minCritDamageMultiplier, blessing.maxCritDamageMultiplier);
            collision.GetComponent<PlayerController>().wp.tempDefense += (int)Random.Range(blessing.minDefenseMultiplier, blessing.maxDefenseMultiplier);
            collision.GetComponent<PlayerController>().wp.tempHealth += (int)Random.Range(blessing.minHealthMultiplier, blessing.maxHealthMultiplier);
            collision.GetComponent<PlayerController>().wp.tempMana += (int)Random.Range(blessing.minManaMultiplier, blessing.maxManaMultiplier);
            collision.GetComponent<PlayerController>().UpdateHealth();
            collision.GetComponent<PlayerController>().UpdateMana();
            Destroy(gameObject);
        }
    }

    public void FixedUpdate()
    {
        Move();
    }
}
