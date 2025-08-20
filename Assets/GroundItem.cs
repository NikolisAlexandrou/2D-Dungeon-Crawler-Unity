using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public WeaponScript weapon;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = weapon.sprite;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //add to inventory
            if (weapon == null)
                return;
            if (collision.GetComponentInChildren<WeaponParent>().IsAttacking == true)
                return;
            Debug.Log("Cannot pickup while attacking");
            collision.GetComponentInChildren<WeaponParent>().weapon = weapon;
            Destroy(gameObject);
        }
    }
}
