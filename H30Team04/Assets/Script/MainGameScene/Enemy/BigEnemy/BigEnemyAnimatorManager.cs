//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BigEnemyAnimatorManager : MonoBehaviour
{
    private Animator m_animator;
    private float speed = 0.0f;
    [HideInInspector] public bool isWalk = false;
    [HideInInspector] public float moveSpeed = 0.0f;
    [HideInInspector] public bool isStep;
    [Tooltip("骨組みのアニメーター"),SerializeField] private Animator born_Animator;

    private int dir = 1;
    [HideInInspector] public bool isDash = false;
    [Range(1.0f,5.0f),Tooltip("ダッシュ時のスピード"),SerializeField] private float dashSpeed = 2.0f;

    public float animatorSpeed
    {
        get
        {
            return m_animator.speed;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        AnimatorInitialize();
        WalkStart();
    }

    // Update is called once per frame
    void Update()
    {
        //歩いている時以外処理を行わない
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk") ||
            !born_Animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk")) return;
        speed = Mathf.Clamp01(speed + Time.deltaTime * dir);
        m_animator.SetFloat("Speed", speed);
        born_Animator.SetFloat("Speed", speed);
        if (speed < 1.0f)
        {
            m_animator.speed = 0.4f;
            born_Animator.speed = 0.4f;
        }
        else
        {
            m_animator.speed = (isDash) ? dashSpeed : 1.0f;
            born_Animator.speed = (isDash) ? dashSpeed : 1.0f;
        }
    }

    public void AnimatorInitialize()
    {  //アニメーションの初期化
        speed = 0;
    }

    public void AnimatorReset()
    {
        m_animator.speed = 1.0f;
        m_animator.SetTrigger("Reset");
        born_Animator.speed = 1.0f;
        born_Animator.SetTrigger("Reset");
    }

    public void WalkStart()
    {  //歩き行動を開始する 
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk") ||
            born_Animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk")) return;
        m_animator.SetTrigger("WalkStart");
        born_Animator.SetTrigger("WalkStart");
        dir = 1;
    }

    public void SpeedChange(float speed)
    {
        m_animator.speed = speed;
        born_Animator.speed = speed;
    }

    public void WalkStop()
    {  //歩くのをやめる
        dir = -1;
    }

    public void LaunchStart()
    {
        m_animator.SetTrigger("LaunchStart");
        m_animator.speed = 1.0f;
        born_Animator.SetTrigger("LaunchStart");
        born_Animator.speed = 1.0f;
    }

    public void Launch()
    {
        m_animator.SetTrigger("Launch");
        born_Animator.SetTrigger("Launch");
    }

    public void LaunchReset()
    {
        m_animator.SetTrigger("LaunchLoop");
        born_Animator.SetTrigger("LaunchLoop");
    }

    public void LaunchEnd()
    {
        m_animator.SetTrigger("LaunchEnd");
        born_Animator.SetTrigger("LaunchEnd");
    }

    public void ShootingMove()
    {
        m_animator.SetTrigger("ShootingMove");
        born_Animator.SetTrigger("ShootingMove");
    }
}
