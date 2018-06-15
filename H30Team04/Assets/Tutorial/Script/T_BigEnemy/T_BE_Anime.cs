using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_BE_Anime : MonoBehaviour
{
    Animator animator;
    bool isMove = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("WalkStart", isMove);
        animator.SetBool("WalkStop", !isMove);
    }

    void Update()
    {
        isMove = true;
    }
}
