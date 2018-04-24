using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] float speed = 2f; //移動速度
    [SerializeField, Range(1, 500f)] float power = 20; //制動力（移動に影響）
    [SerializeField, Range(0.1f, 10f)] float rotate = 2; //回転量
    [SerializeField, Range(0.1f, 10f)] float driftRotate = 3f; //ドリフト時の回転量
    [SerializeField, Range(1, 5)] int residue = 3; //死亡可能回数

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    //運転操作用
    float axel;
    float curve;

    void Start()
    {
        //リスポーン用初期情報
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        GetInputDrive(); //運転操作入力の取得

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) Respawn();
    }

    void FixedUpdate()
    {
        Drive(); //運転
    }

    /// <summary>
    /// ＊運転操作入力の取得
    /// </summary>
    void GetInputDrive()
    {
        axel = Input.GetAxis("Axel");
        curve = Input.GetAxis("Curve");
    }

    /// <summary>
    /// ＊運転操作
    /// </summary>
    void Drive()
    {
        var rb = gameObject.GetComponent<Rigidbody>();

        //移動計算用
        Vector3 move = rb.transform.forward * axel * speed;
        Vector3 force = power * move - rb.velocity;

        rb.AddForce(force); //移動

        float rotation = (axel == 0) ? driftRotate : rotate; //旋回力の選択

        //動いていればカーブ可能
        if (Mathf.Abs(force.x) > 0.05f)
            rb.transform.Rotate(new Vector3(0.0f, curve * rotation, 0.0f));
    }

    /// <summary>
    /// ＊初期位置にリスポーン
    /// </summary>
    public void Respawn()
    {
        if (Annihilation()) return;
        //初期位置へ
        transform.position = startPosition;
        transform.rotation = startRotation;
        residue--; //死亡可能回数の減少
    }

    /// <summary>
    /// ＊残機が0になったら「全滅」＝True
    /// </summary>
    public bool Annihilation()
    {
        return (residue <= 0) ? true : false;
    }

    /// <summary>
    /// ＊死亡
    /// </summary>
    public void Death()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}