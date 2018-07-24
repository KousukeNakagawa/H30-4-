using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingFailure : MonoBehaviour
{
    [Tooltip("ミサイルのプレファブ")]public GameObject missilePrefab;
    [Tooltip("爆発のエフェクト")]public GameObject explosionPrefab;
    [Tooltip("大爆発のエフェクト")] public GameObject explosionEXPrefab;
    [Tooltip("ミサイルの目標座標")] public Transform targetPos;
    [Tooltip("ミサイルの本数")] public int missileCount = 6;
    [Tooltip("ミサイルの当たる範囲")] public float missileRange = 15.0f;

    [Tooltip("射撃失敗時に発射するまでの時間"),SerializeField] private float launchTime = 0.5f;
    private float launchCount = int.MinValue;
    private bool isFailure = false;

    public Transform[] launchPos;  //肩

    void Update()
    {
        if (!isFailure || Time.timeScale == 0) return;
        if (launchCount > 0)
        {
            launchCount -= Time.deltaTime;
            if (launchCount <= 0)
            {
                launchCount = 0;
            }
        }
        if (launchCount == 0)
        {
            Launch();
            launchCount = int.MinValue;
        }
    }

    public void FailureAction()
    {
        if (!BigEnemyScripts.shootingPhaseMove.isShooting) return;

        BigEnemyScripts.bigEnemyAnimatorManager.AnimatorReset();
        targetPos.gameObject.SetActive(true);
        launchCount = launchTime;
        isFailure = true;
        BigEnemyScripts.shootingPhaseMove.isShooting = false;
    }

    private void Launch()
    {
        for (int i = 0; i < missileCount; i++)
        {
            Vector3 rand = new Vector3(Random.Range(-missileRange * 2, -missileRange),
                Random.Range(-missileRange, missileRange), Random.Range(-missileRange, missileRange));
            Vector3 target = targetPos.position;
            GameObject mm = Instantiate(missilePrefab, 
                launchPos[i % launchPos.Length].position,
                Quaternion.Euler(-90.0f + Random.Range(-15.0f, 15.0f), Random.Range(-7.5f, 7.5f), 0f));
            mm.GetComponentInChildren<MissileCollider2>().explosion = explosionPrefab;
            if (i == 0) mm.GetComponentInChildren<MissileCollider2>().explosionEX = explosionEXPrefab;
            MissileMove3 move3 = mm.GetComponent<MissileMove3>();
            if (i < missileCount - 1)
            {
                target += rand;
            }
            move3.targetPos = target;
        }
    }
}
