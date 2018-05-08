using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour {

    private static List<MissileCollider> isHits = new List<MissileCollider>();
    private bool isHit = false;

	// Use this for initialization
	void Start () {
        isHits.Add(this);  //全て当たったかを判定するために自分自身を代入する
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Xline") || other.CompareTag("Beacon"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Field") || other.CompareTag("SnipeBullet"))
        {
            //ミサイル破壊
            Destroy(transform.root.gameObject);
            isHit = true;
        }

        //全てのミサイルのisHitがtrueなら巨大ロボットのターゲットをリセットする
        if (isHits.Count > 0 && isHits.FindAll(f => !f.isHit).Count == 0)
        {
            BigEnemyScripts.missileLaunch.isMissile = false;
            BigEnemyScripts.searchObject.ResetTarget();
            BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
            isHits.Clear();
        }
    }
}
