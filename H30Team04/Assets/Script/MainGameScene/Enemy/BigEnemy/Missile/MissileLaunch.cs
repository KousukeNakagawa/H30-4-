using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : MonoBehaviour
{

    public bool isMissile;  //ミサイル発射対象と当たった瞬間から
    public bool isLaunch;  //予備動作中
    public GameObject missilePrefab;  //ミサイルのプレファブ

    private float launchTime;  //ミサイルを発射するまでのカウンター
    public float launchSense = 1.0f;  //ミサイルを発射するまでの間隔

    public Vector3[] instantiateYPoss;
    public Vector3[] instantiateAngles;
    public float launchCount = 0.5f;  //複数ミサイルを出す場合の間隔
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaunch) return;
        if (launchTime < Time.time)
        {
            Launch();
        }
    }

    public void LaunchSet()
    {
        isLaunch = true;
        launchTime = Time.time + launchSense;
    }

    public void Launch()
    {
        StartCoroutine("LaunchWait");
        isLaunch = false;
    }

    IEnumerator LaunchWait()
    {
        for (int i = 0; i < instantiateYPoss.Length; i++)
        {
            GameObject m = Instantiate(missilePrefab, BigEnemyScripts.mTransform.position + instantiateYPoss[i], Quaternion.Euler(instantiateAngles[i]));
            yield return new WaitForSeconds(launchCount);
        }
    }
}
