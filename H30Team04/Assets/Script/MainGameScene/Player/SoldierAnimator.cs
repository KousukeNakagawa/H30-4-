using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimator : MonoBehaviour
{
    Animator animator;

    bool isStay = false;
    bool isMove = false;
    bool isBeacon = false;
    bool isSnipe = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimator();
        ChangeAnimator();
    }

    /// <summary> アニメーションのセッティング </summary>
    void SetAnimator()
    {
        //animator.SetBool("Move", isMove);
        animator.SetBool("Stay", isStay);
        animator.SetBool("SetupBeacon", isSnipe);
        animator.SetBool("SetupSnipe", isBeacon);

        //Debug.Log("Move::" + isMove);
        //Debug.Log("Stay::" + isStay);
        //Debug.Log("Beacon::" + isBeacon);
        //Debug.Log("Snipe::" + isSnipe);
    }

    /// <summary> アニメーションの条件処理 </summary>
    void ChangeAnimator()
    {
        ////移動しているなら
        //if (Soldier.IsMove)
        //{
        //    isMove = false;
        //    isStay = true;
        //    isBeacon = false;
        //    isSnipe = false;
        //}
        ////IsMoveがFalseで
        ////止まっているなら
        /*   else*/ if (!WeaponCtrl.IsSetup)
        {
            isMove = false;
            isStay = true;
            isBeacon = false;
            isSnipe = false;
        }
        //ビーコンを装備しているなら
        else if (WeaponCtrl.WeaponBeacon)
        {
            isMove = false;
            isStay = false;
            isBeacon = true;
            isSnipe = false;
        }
        else
        {
            isMove = false;
            isStay = false;
            isBeacon = false;
            isSnipe = true;
        }
    }
}
