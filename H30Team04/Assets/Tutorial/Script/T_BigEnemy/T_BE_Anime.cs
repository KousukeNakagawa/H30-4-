using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_BE_Anime : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAdvance", T_BE_Move.IsAdvance);
    }

    void Update()
    {

    }
}
