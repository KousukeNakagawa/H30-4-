using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{

    public static List<MissileCollider> isHits = new List<MissileCollider>();  //全てのミサイルが当たったか
    private bool isHit = false;
    [Tooltip("爆発位置"), SerializeField] private Transform explosionPos;
    [Tooltip("爆発のプレファブ")] public GameObject explosion;

    // Use this for initialization
    void Awake()
    {
        isHits.Add(this);  //全て当たったかを判定するために自分自身を代入する
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (transform.position.y < -2 || (explosionPos.position - BigEnemyScripts.searchObject.targetPos).sqrMagnitude <= 3.0f)
        {  //地面より下に入ったら強制的に消す
            isHit = true;
            HitCheck();
            //ミサイル破壊
            Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Field") || other.CompareTag("Building") || other.gameObject.CompareTag("SnipeBullet"))
        {
            HitDestroy(other);
        }
        else if (other.gameObject.CompareTag("Beacon"))
        {
            Destroy(other.gameObject);
        }
    }

    private void HitDestroy(Collider other)
    {
        Vector3 exPos = explosionPos.position;
        if (other.transform.Equals(BigEnemyScripts.shootingFailure.targetPos))  //1発だけカメラに当たるミサイルの場合
        {
            exPos = other.transform.position + new Vector3(-3f, -1f, 0);
        }
        //爆発追加
        if (exPos == Vector3.zero) exPos = transform.position;
        GameObject ex = Instantiate(explosion, exPos, Quaternion.identity);
        isHit = true;
        Destroy(ex, 3.0f);
        HitCheck();
        //ミサイル破壊
        Destroy(transform.parent.gameObject);
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