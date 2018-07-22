using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoketBullet : MonoBehaviour {

    [Header("弾速"), SerializeField] private float bulletSpeed = 1.0f;
    [Header("加速度"), SerializeField, Range(1.0f, 1.5f)] private float bulletAccel = 1.0f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        bulletSpeed *= bulletAccel;
	}

    public void SetTarget(GameObject tar)
    {
        transform.LookAt(tar.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "BigEnemy")
        {
            transform.parent.GetComponent<AttackPlayer>().Damege();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }


}
