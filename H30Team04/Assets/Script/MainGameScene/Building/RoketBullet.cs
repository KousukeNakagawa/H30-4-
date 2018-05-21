using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoketBullet : MonoBehaviour {

    [Header("弾速"), SerializeField] private float bulletSpeed = 1.0f;
    [Header("加速度"), SerializeField, Range(1.0f, 1.5f)] private float bulletAccel = 1.0f;
    //private GameObject target;

    // Use this for initialization
    void Start () {
        //Destroy(gameObject, 10.0f);//テスト中限界位置まで飛んでいくから
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        bulletSpeed *= bulletAccel;
	}

    public void SetTarget(GameObject tar)
    {
        //target = tar;
        transform.LookAt(tar.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BigEnemy")
        {
            transform.parent.GetComponent<AttackPlayer>().Damege();
            Destroy(gameObject);
        }
    }


}
