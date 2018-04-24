using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour {

    public static List<MissileCollider> isHits = new List<MissileCollider>();
    private bool isHit = false;

	// Use this for initialization
	void Start () {
        isHits.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Xline") || other.CompareTag("Beacon"))
        {
            //テスト
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Field"))
        {
            //ミサイル破壊
            Destroy(transform.root.gameObject);
            isHit = true;
        }

        if (isHits.FindAll(f => !f.isHit).Count == 0)
        {
            BigEnemyScripts.missileLaunch.isMissile = false;
            BigEnemyScripts.searchObject.ResetTarget();
            BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
            isHits.Clear();
        }
    }
}
