using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] GameObject beacon;
    [SerializeField] [Range(1, 100)] float speed = 50;
    [SerializeField] [Range(5, 100)] float rangeDistance = 50;

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

    void OnCollisionEnter(Collision collision)
    {
        //ビルにぶつかったら止まる
        if (collision.collider.CompareTag("Building"))
        {
            Cling();
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
            Destroy(beacon);
    }

    /// <summary>
    /// ＊ビーコンの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        rb.velocity = direction * speed;
    }

    /// <summary>
    /// ＊張り付く
    /// </summary>
    void Cling()
    {
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.tag = "Beacon";
    }
}
