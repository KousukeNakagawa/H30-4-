using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : MonoBehaviour
{

    [HideInInspector] public bool isMissile;  //ミサイル発射対象と当たった瞬間から
    [HideInInspector] public bool isLaunch;  //予備動作中
    [SerializeField] private GameObject missilePrefab;  //ミサイルのプレファブ

    private float launchTime;  //ミサイルを発射するまでのカウンター
    [Tooltip("ミサイルを発射するまでの時間")] public float launchSense = 1.0f;
    [Tooltip("撃つミサイルの本数")] public float missileCount = 3;

    [Header("配列の要素数を同じにしないとエラーが発生します")]
    [Tooltip("初期角度の設定")] public Vector3[] instantiateAngles;
    [Tooltip("ミサイルを撃つ場所")] public Transform[] missileLaunchPos;
    [Tooltip("複数ミサイルを出す場合の間隔")] public float launchCount = 0.5f;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaunch) return;
        if (launchTime < Time.time)
        {  //ミサイルを発射する
            Launch();
            //BigEnemyScripts.missileGroupManager.MissileSet();
            //isLaunch = false;
        }
    }

    public void LaunchSet()
    {
        isLaunch = true;
        launchTime = Time.time + launchSense;
        BigEnemyScripts.bigEnemyAnimatorManager.LaunchStart();
    }

    public void Launch()
    {
        StartCoroutine(LaunchWait());
        isLaunch = false;
    }

    IEnumerator LaunchWait()
    {
        for (int i = 0; i < missileCount; i++)
        {
            yield return null;
            BigEnemyScripts.bigEnemyAnimatorManager.Launch();
            if (!BigEnemyScripts.shootingPhaseMove.isShooting)
            {
                GameObject m = Instantiate(missilePrefab,
                    missileLaunchPos[i % missileLaunchPos.Length].position,
                    missileLaunchPos[i % missileLaunchPos.Length].rotation);
            }
            yield return new WaitForSeconds(launchCount);
            if (missileCount - 1 != i) BigEnemyScripts.bigEnemyAnimatorManager.LaunchReset();
        }
    }
}
