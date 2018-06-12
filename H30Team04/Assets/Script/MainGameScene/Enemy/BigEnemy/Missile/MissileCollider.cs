using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{

    private static List<MissileCollider> isHits = new List<MissileCollider>();
    private bool isHit = false;
    [SerializeField] private Transform explosionPos;
    public GameObject explosion;

    // Use this for initialization
    void Awake()
    {
        isHits.Add(this);  //全て当たったかを判定するために自分自身を代入する
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (transform.position.y < -2)
        {  //地面より下に入ったら強制的に消す
            Destroy(transform.parent.gameObject);
            isHit = true;
            HitCheck();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Field")
            || other.CompareTag("Building") || other.transform.Equals(BigEnemyScripts.shootingFailure.targetPos))
        {
            HitDestroy(other);
        }
        else if (other.CompareTag("Beacon"))
        {
            Destroy(other.gameObject);
        }
        HitCheck();
    }

    private void HitDestroy(Collider other)
    {
        //ミサイル破壊
        Destroy(transform.parent.gameObject);
        Vector3 exPos = explosionPos.position;
        if (other.transform.Equals(BigEnemyScripts.shootingFailure.targetPos))  //1発だけカメラに当たるミサイルの場合
        {
            exPos = other.transform.position + new Vector3(-3f, -1f, 0);
        }
        //爆発追加
        if (exPos == Vector3.zero) exPos = transform.position;
        GameObject ex = Instantiate(explosion, exPos, Quaternion.identity);
        Destroy(ex, 3.0f);
        isHit = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("SnipeBullet"))
        {
            HitDestroy(other.collider);
        }
        HitCheck();
    }

    private void HitCheck()
    {
        //全てのミサイルのisHitがtrueなら巨大ロボットのターゲットをリセットする
        if (isHits.Count > 0 && isHits.FindAll(f => !f.isHit).Count == 0)
        {
            BigEnemyScripts.missileLaunch.isMissile = false;
            BigEnemyScripts.searchObject.ResetTarget();
            BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
            BigEnemyScripts.bigEnemyAnimatorManager.LaunchEnd();
            BigEnemyScripts.bigEnemyAnimatorManager.AnimatorInitialize();
            isHits.Clear();
        }
    }
}