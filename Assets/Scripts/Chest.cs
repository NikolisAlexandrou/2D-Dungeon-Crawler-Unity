using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chest : MonoBehaviour
{
    public WeaponScript[] floor1PossibleLoot;
    public GameObject blessing;
    public int floorNumber;
    [SerializeField] WeaponScript reward;
    [SerializeField] GameObject GroundItemPrefab;
    [SerializeField] bool opened;
    Animator anim;
    void PlaySpawnAnimation()
    {
        anim.SetTrigger("ChestAppear");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the chest hasnt been opened yet
        if (opened == true)
            return;
        
        opened = true;
        //choose random reward
        chooseRandomRewards();
        //spawn item
        GameObject ob = Instantiate(GroundItemPrefab, transform.position, Quaternion.identity);
        ob.GetComponent<GroundItem>().weapon = reward;
        //move towards player animation

        anim.SetTrigger("ChestOpen");
        var sequence = DOTween.Sequence();
        sequence.Append(ob.transform.DOMove(GameManager.i.player.position, .5f));
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        PlaySpawnAnimation();
    }

    void chooseRandomRewards()
    {
        int chance = Random.Range(0, floor1PossibleLoot.Length - 1);
        reward = floor1PossibleLoot[chance];
    }

}
