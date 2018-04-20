using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [SerializeField] GameObject snipeBullet;
    [SerializeField] [Range(1, 300)] float speed = 100;
    [SerializeField] [Range(5, 300)] float rangeDistance = 100;

    Rigidbody rb;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
    }

    void Update()
    {
        OverRangeDistance();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SmallEnemy"))
        {
            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// ＊射程距離外消滅
    /// </summary>
    void OverRangeDistance()
    {
        Vector3 FlyDistance = rb.position - startPos;

        if (FlyDistance.x > rangeDistance ||
            FlyDistance.y > rangeDistance ||
            FlyDistance.z > rangeDistance)
            Destroy(snipeBullet);
    }

    /// <summary>
    /// ＊スナイパーバレットの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        rb.velocity = direction * speed;
    }
}
