using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingFailure : MonoBehaviour
{

    public GameObject missilePrefab;
    public GameObject explosionPrefab;
    [Tooltip("ミサイルの目標座標")] public Transform targetPos;
    [Tooltip("ミサイルの本数")] public int missileCount = 6;
    [Tooltip("ミサイルの当たる範囲")] public float missileRange = 15.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FailureAction()
    {
        targetPos.gameObject.SetActive(true);
        if (!BigEnemyScripts.shootingPhaseMove.isShooting) return;

        for (int i = 0; i < missileCount; i++)
        {
            Vector3 rand = new Vector3(Random.Range(-missileRange * 2, -missileRange),
                Random.Range(-missileRange, missileRange), Random.Range(-missileRange, missileRange));
            Vector3 target = targetPos.position;
            //GameObject m = Instantiate(lossMissilePrefab, BigEnemyScripts.mTransform.position,
            //    Quaternion.Euler(-90.0f + Random.Range(-10.0f, 10.0f), Random.Range(-2.5f, 2.5f), 0));
            GameObject mm = Instantiate(missilePrefab, BigEnemyScripts.mTransform.position + new Vector3(-7f, 5, 0),
                Quaternion.Euler(-90.0f + Random.Range(-15.0f, 15.0f), Random.Range(-7.5f, 7.5f), 0f));
            MissileMove2 move2 = mm.GetComponent<MissileMove2>();
            if (i < missileCount - 1)
            {
                target += rand;
                move2.riseSpeed = Random.Range(17.5f, 22.5f);
                move2.TransSpeed = Random.Range(7.5f, 12.5f);
                move2.riseCount = Random.Range(1.5f, 3f);
                move2.rotationCount = Random.Range(2.5f, 5f);
            }
            else
            {
                mm.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
            move2.targetPos = target;
            move2.explosion = explosionPrefab;
        }
    }
}
