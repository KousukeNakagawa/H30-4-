using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_zk : MonoBehaviour
{
    [SerializeField] GameObject missile;

    [SerializeField] [Range(1, 100)] float speed = 100; //弾速
    [SerializeField] [Range(5, 30)] float workTime = 10; //稼働時間
    [SerializeField, Range(1, 10)] float homingAbility = 1; //ホーミング性能
    [SerializeField, Range(1, 5)] float aimingTime = 2;

    Rigidbody rb;
    Transform car;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        car = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        OverRangeDistance(); //射程距離外消滅
    }

    void FixedUpdate()
    {
        Fire();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(missile);
            missile = null;
        }

        //プレイヤーかビルにヒットしたら、それを消滅させる
        if (collision.collider.CompareTag("Building"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(missile);
            missile = null;
        }
    }

    /// <summary>
    /// ＊射程外消滅
    /// </summary>
    void OverRangeDistance()
    {
        workTime -= Time.deltaTime;

        //稼働時間が来たら消滅
        if (workTime <= 0) Destroy(missile);
    }

    /// <summary>
    /// ＊ミサイルの発射
    /// </summary>
    public void Fire()
    {
        Start();

        HomingPlayer();
    }

    void HomingPlayer()
    {
        Vector3 direction = car.position + Vector3.up * 1.5f - rb.position;

        transform.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(direction), homingAbility * Time.deltaTime);

        //rb.position += transform.forward * speed * Time.deltaTime;
        //speed += 0.1f;

        if (aimingTime >= 0)
        {
            rb.AddForce(transform.forward * 10 * Time.deltaTime, ForceMode.Acceleration);
            aimingTime -= Time.deltaTime;
        }
        else
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }
}