using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBreak : MonoBehaviour {

    [SerializeField] private Animator m_animator;
    private static Animator _Animator;

    [SerializeField] private GameObject tower;
    private static GameObject _Tower;

    [SerializeField] private GameObject breakTower;
    private static GameObject _BreakTower;

    [SerializeField] private BoxCollider box;
    private static BoxCollider _Box;

    private static bool isBreak;
    private bool previous;

    void Awake()
    {
        _Animator = m_animator;
        _BreakTower = breakTower;
        _Tower = tower;
        _Box = box;
    }

    void Update()
    {
        if (BigEnemyScripts.shootingPhaseMove.isShooting) _Box.enabled = true;
        if (!previous && isBreak) Invoke("BoxDisble", 3.0f);
        previous = isBreak;
    }

    public static void BreakAction()
    {
        if (isBreak) return;
        _Tower.SetActive(false);
        _BreakTower.SetActive(true);
        _Animator.SetTrigger("Break");
        isBreak = true;
    }

    private void BoxDisble()
    {
        _Box.enabled = false;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Missile") && BigEnemyScripts.shootingPhaseMove.isShooting)
        {
            BreakAction();
        }
    }
}
