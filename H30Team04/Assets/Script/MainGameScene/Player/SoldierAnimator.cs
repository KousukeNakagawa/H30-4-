using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimator : MonoBehaviour
{
    Animator animator;
    AudioSource audio;
    [SerializeField] AudioClip se;

    bool isStay = false;
    bool isFront = false;
    bool isSide = false;
    bool isBeacon = false;
    bool isSnipe = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audio = gameObject.AddComponent<AudioSource>();
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
        if (Soldier.IsMove)
        {
            isFront = true;
            isStay = false;
            isBeacon = false;
            isSnipe = false;
        }
        //IsMoveがFalseで
        //止まっているなら
        else if (!WeaponCtrl.IsSetup)
        {
            isFront = false;
            isStay = WeaponCtrl.IsSetup;
            isBeacon = false;
            isSnipe = false;
        }
        //ビーコンを装備しているなら
        else if (WeaponCtrl.WeaponBeacon)
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
    void Step()
    {
        audio.PlayOneShot(se);
    }
}
