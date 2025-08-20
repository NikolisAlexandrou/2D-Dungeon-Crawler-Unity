using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationModifiers : MonoBehaviour
{
    [SerializeField] GameObject Player,weaponParent;


    public void MoveForward(float speed)
    {
        Player.GetComponent<PlayerController>().freeze = true;
        Player.GetComponent<Rigidbody2D>().AddForce(weaponParent.transform.right * speed, ForceMode2D.Impulse);
        StartCoroutine(unFreezePlayer(0.2f));
    }
    public IEnumerator unFreezePlayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Player.GetComponent<PlayerController>().freeze = false;
    }
}
