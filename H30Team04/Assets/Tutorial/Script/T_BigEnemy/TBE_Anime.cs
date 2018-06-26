using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBE_Anime : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("IsAdvance", TBE_Move.IsAdvance);
    }
}
