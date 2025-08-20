using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    [SerializeField] Vector2 movePos;
    [SerializeField] float MoveDuration;


    public void MoveToPosition()
    {
        transform.DOMove(movePos, MoveDuration);
    }
}
