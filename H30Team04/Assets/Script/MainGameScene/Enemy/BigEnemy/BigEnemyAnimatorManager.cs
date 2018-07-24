//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BigEnemyAnimatorManager : MonoBehaviour
{
    //アニメーター
    private Animator m_animator;
    //骨のアニメーター
    [Tooltip("骨組みのアニメーター"), SerializeField] private Animator born_Animator;
    //ブレンド用のスピード
    private float speed = 0.0f;
    //歩いているか
    [HideInInspector] public bool isWalk = false;
    //動くスピード
    [HideInInspector] public float moveSpeed = 0.0f;
    //足が地面についたか
    [HideInInspector] public bool isStep;

    //speedを上げるか下げるか
    private int dir = 1;
    //ダッシュをしているか
    [HideInInspector] public bool isDash = false;
    [Range(1.0f,5.0f),Tooltip("ダッシュ時のスピード"),SerializeField] private float dashSpeed = 2.0f;

    // Use this for initialization
    void Start()
    {
        //初期化
        m_animator = GetComponent<Animator>();
        AnimatorInitialize();
        //最初は歩きモーション
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
        speed = 0;
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
        m_animator.speed = 1.0f;
        born_Animator.speed = 1.0f;
        speed = 1.0f;
        isDash = false;
        m_animator.SetFloat("Speed", speed);
        born_Animator.SetFloat("Speed", speed);
    }

    public void End()
    {
        m_animator.SetTrigger("End");
        born_Animator.SetTrigger("End");
    }
}
