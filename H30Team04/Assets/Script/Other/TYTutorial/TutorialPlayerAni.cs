using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerAni : MonoBehaviour {

    Animator animator;
    AudioSource audio;
    [SerializeField] AudioClip se;
    [SerializeField, Range(0.1f, 3)] float firstPich;
    [SerializeField, Range(0.1f, 3)] float nextPitch;

    bool isStay = false;
    bool isFront = false;
    bool isSide = false;
    bool isBeacon = false;
    bool isSnipe = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        SetAnimator();
        ChangeAnimator();
    }

    /// <summary> アニメーションのセッティング </summary>
    void SetAnimator()
    {
        animator.SetBool("RunFront", isFront);
        animator.SetBool("Stay", isStay);
        animator.SetBool("SetupBeacon", isSnipe);
        animator.SetBool("SetupSnipe", isBeacon);
    }

    /// <summary> アニメーションの条件処理 </summary>
    void ChangeAnimator()
    {
        //移動しているなら
        if (TutorialPlayer.IsMove)
        {
            isFront = true;
            isStay = false;
            isBeacon = false;
            isSnipe = false;
        }
        //IsMoveがFalseで
        //止まっているなら
        else if (!TutorialWepon.IsSetup)
        {
            isFront = false;
            isStay = true;
            isBeacon = false;
            isSnipe = false;
        }
        //ビーコンを装備しているなら
        else if (TutorialWepon.WeaponBeacon)
        {
            isFront = false;
            isStay = false;
            isBeacon = true;
            isSnipe = false;
        }
        else
        {
            isFront = false;
            isStay = false;
            isBeacon = false;
            isSnipe = true;
        }
    }

    /// <summary> アニメーションイベント用（歩くSE） </summary>
    void FirstStep()
    {
        audio.pitch = firstPich;
        audio.PlayOneShot(se);
    }

    /// <summary> アニメーションイベント用（歩くSE） </summary>
    void NextStep()
    {
        audio.pitch = nextPitch;
        audio.PlayOneShot(se);
    }
}
