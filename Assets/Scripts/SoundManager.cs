using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource attackSound0;
    [SerializeField] AudioSource attackSound1;
    [SerializeField] AudioSource attackSound2;

    [SerializeField] AudioSource arrowShoot;

    [SerializeField] AudioSource swing0;
    [SerializeField] AudioSource swing1;

    [SerializeField] AudioSource step0;
    [SerializeField] AudioSource step1;

    public static SoundManager i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public AudioSource PlayAttackSound(int sound)
    {
        AudioSource audio = attackSound0;
        switch (sound)
        {
            case 0:
                audio = AttackSound0;
                break;
            case 1:
                audio = AttackSound1;
                break;
            case 2:
                audio = AttackSound2;
                break;
        }
        return audio;
    }

    public AudioSource PlaySwingSound(int sound)
    {
        AudioSource audio = swing0;
        switch (sound)
        {
            case 0:
                audio = Swing0;
                break;
            case 1:
                audio = Swing1;
                break;
        }
        return audio;
    }

    public AudioSource AttackSound0
    {
        get => attackSound0;
    }
    public AudioSource AttackSound1
    {
        get => attackSound1;
    }
    public AudioSource AttackSound2
    {
        get => attackSound2;
    }

    public AudioSource Swing0
    {
        get => swing0;
    }
    public AudioSource Swing1
    {
        get => swing1;
    }

    public AudioSource Step0
    {
        get => step0;
    }
    public AudioSource Step1
    {
        get => step1;
    }

    public AudioSource ArrowShoot
    {
        get => arrowShoot;
    }
}
