using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    bool entered;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && entered == false)
        {
            entered = true;
            GameManager.i.NextRoom();
        }
    }
}
