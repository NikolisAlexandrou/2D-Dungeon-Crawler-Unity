using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] float seconds;
    [SerializeField] float secondsToStart;
    [SerializeField] GameObject borders;

    [SerializeField] int maxTries;
    int tries;

    public GameObject FakeObject;

    public GameObject[] Pots;
    [SerializeField] Vector2 StartPos;
    [SerializeField] float distanceBwPots;

    public bool GameOver;

    public void FlippedLever()
    {
        maxTries = Random.Range(maxTries, maxTries + 5);
        SpawnPots();
    }
    
    void SpawnPots()
    {
        for (int i = 0; i <= Pots.Length - 1; i++)
        {
            Pots[i] = Instantiate(Pots[i], new Vector2(StartPos.x + i * distanceBwPots, StartPos.y + 1), Quaternion.identity);
        }        
        StartCoroutine(StartAnimation());
    }
    public bool StartAnimating;

    IEnumerator StartAnimation()
    {
        int chance = Random.Range(0, Pots.Length);
        FakeObject = Instantiate(FakeObject, new Vector2(Pots[chance].transform.position.x, Pots[chance].transform.position.y - 1), Quaternion.identity);
        
        foreach (GameObject ob in Pots)
        {
            ob.GetComponent<Pot>().puzzleManager = this;
            ob.transform.DOMoveY(ob.transform.position.y - 1, 1);

        }
        yield return new WaitForSeconds(1);
        StartAnimating = true;
        FakeObject.transform.parent = Pots[chance].transform;
    }

    private void Update()
    {
        if (StartAnimating == true)
        {
            if (tries < maxTries)
            {
                if (retry == false)
                {
                    tries += 1;
                    retry = true;
                    SwapPos();
                }
            }
            else
            {
                StartAnimating = false;
                GameOver = true;
                borders.SetActive(false);
                //let player choose
            }
        }
    }

    

    void SwapPos()
    {
        int chance = Random.Range(0, Pots.Length );
        int chance1 = Random.Range(0, Pots.Length );
        SwapAnimation(Pots[chance].transform, Pots[chance1].transform.position);
        SwapAnimation(Pots[chance1].transform, Pots[chance].transform.position);
        StartCoroutine(Reset(seconds));
    }
    public bool retry = false;
    IEnumerator Reset(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        retry = false;
    }

    public void SwapAnimation(Transform currentPos, Vector2 PosToGo)
    {
        currentPos.DOMoveX(PosToGo.x, seconds);
    }


}
