using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{

    public UnityEvent OnFlipLever;
    bool flipped;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && flipped == false)
        {
            flipped = true;
            GetComponent<SpriteRenderer>().flipX = true;
            OnFlipLever?.Invoke();
        }
    }
}
