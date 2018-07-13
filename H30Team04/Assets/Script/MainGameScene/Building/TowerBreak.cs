using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBreak : MonoBehaviour {

    [SerializeField] private Animator m_animator;
    private static Animator _Animator;

    void Awake()
    {
        _Animator = m_animator;
    }

    public static void BreakAction()
    {
        _Animator.SetTrigger("Break");
    }
}
