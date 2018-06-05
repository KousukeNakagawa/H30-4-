using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyAnimatorManager : MonoBehaviour {

    private Animator m_animator;
    private float speed = 0.0f;
    [HideInInspector] public bool isWalk = false;
    [HideInInspector] public float moveSpeed = 0.0f;

    private int dir = 1;

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
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk")) return;
        speed = Mathf.Clamp01(speed + Time.deltaTime * dir);
        m_animator.SetFloat("Speed", speed);
        if (speed == 0) m_animator.SetTrigger("WalkStop");
        else if (speed < 1.0f) m_animator.speed = 0.4f;
        else m_animator.speed = 1.0f;
    }

    public void AnimatorInitialize()
    {  //アニメーションの初期化
        speed = 0;
    }

    public void WalkStart()
    {  //歩き行動を開始する
        m_animator.SetTrigger("WalkStart");
        dir = 1;
    }

    public void WalkStop()
    {  //歩くのをやめる
        dir *= -1;
    }
}
