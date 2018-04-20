using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] float speed = 2f; //移動速度
    [SerializeField] [Range(1, 500f)] float power = 20; //制動力（移動に影響）
    [SerializeField] [Range(0.1f, 10f)] float rotate = 2; //回転量
    [SerializeField] [Range(0.1f, 10f)] float drift = 1.5f; //ドリフト時の回転倍率

    Rigidbody rb;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;
    int residue; //残機

    //移動とカーブの入力受け取り用
    float axel;
    float curve;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //ドリフト時回転量計算
        drift *= rotate;
        //初期情報
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
        residue = 3;
    }

    void Update()
    {
        GetInputDrive(); //運転入力の取得

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) Respawn();
    }

    void FixedUpdate()
    {
        Drive(); //運転
    }

    /// <summary>
    /// ＊運転処理
    /// </summary>
    void Drive()
    {
        //移動計算変数
        Vector3 move = rb.transform.forward * axel * speed;
        Vector3 force = power * move - rb.velocity;
        float rotation = (axel == 0) ? drift : rotate; //旋回力
        
        rb.AddForce(force); //移動

        //動いていればカーブ可能
        if (Mathf.Abs(force.x) > 0.05f)
            rb.transform.Rotate(new Vector3(0.0f, curve * rotation, 0.0f));
    }

    /// <summary>
    /// ＊運転操作取得（左スティック）
    /// </summary>
    void GetInputDrive()
    {
        axel = Input.GetAxis("Axel");
        curve = Input.GetAxis("Curve");
    }

    /// <summary>
    /// ＊初期位置にリスポーン
    /// </summary>
    void Respawn()
    {
        if (Annihilation()) return;

        transform.position = startPosition;
        transform.rotation = startRotation;
        residue--;
        Debug.Log("残機：" + residue);
    }

    /// <summary>
    /// ＊残機が-1になったら「全滅」＝True
    /// </summary>
    public bool Annihilation()
    {
        return (residue < 0) ? true : false;
    }
}