using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBigEnemy
{
    public class BigEnemyAnimatorManager : UpdateRoot
    {
        [HideInInspector] public float moveSpeed = 0.0f;
        [HideInInspector] public bool isStep;

        private float animeSpeed = 0.0f;
        private Animator m_animator;
        private float animeDir = 1;

        void Start()
        {
            m_animator = GetComponent<Animator>();
            AnimeInitialize();
            WalkStart();
        }

        public override void UpdateMe()
        {
            //歩いている時以外処理を行わない
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk")) return;
            animeSpeed = Mathf.Clamp01(animeSpeed + Time.deltaTime * animeDir);
            m_animator.SetFloat("Speed", animeSpeed);
            if (animeSpeed < 1.0f) m_animator.speed = 0.4f;
            else m_animator.speed = 1.0f;
        }

        public void AnimeInitialize()
        {
            animeSpeed = 0.0f;
        }

        public void WalkStart()
        {  //歩き行動を開始する
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("WaitToWalk")) return;
            m_animator.SetTrigger("WalkStart");
            animeDir = 1;
        }

        public void WalkStop()
        {  //歩くのをやめる
            animeDir = -1;
            print("stop");
        }

        public void LaunchStart()
        {  //待機モーションへ移行
            m_animator.SetTrigger("LaunchStart");
            m_animator.speed = 1.0f;
        }

        public void Launch()
        {  //発射
            m_animator.SetTrigger("Launch");
        }

        public void LaunchReset()
        {  //ループするためのTrigger
            m_animator.SetTrigger("LaunchLoop");
        }

        public void LaunchEnd()
        {  //発射を終了する
            m_animator.SetTrigger("LaunchEnd");
        }
    }
}
