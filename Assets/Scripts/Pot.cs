using UnityEngine;
using DG.Tweening;

public class Pot : MonoBehaviour
{
    public PuzzleManager puzzleManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (puzzleManager.GameOver == true)
        {
            if (collision.transform.tag == "Player" && puzzleManager.StartAnimating == false)
            {
                GameManager.i.alreadyCleared = true;
                puzzleManager.GameOver = false;
                if (transform.childCount >= 1 && transform.GetChild(0).transform.position == puzzleManager.FakeObject.transform.position)
                {
                    transform.GetChild(0).transform.parent = null;
                    //win
                    //add power up
                }
                else
                {
                    //lose

                }
                transform.DOMoveY(transform.position.y + 1, 1);
            }
        }
        
    }

}
