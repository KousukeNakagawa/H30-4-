using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    //Rigidbody rb;
    Vector3 playerPos;
    Quaternion playreRotate;

    [Range(0.1f, 10f)] public float rotate = 1.8f; //回転量
    [Range(0.1f, 10f)] public float speed = 0.8f; //移動速度

    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        transform.position = gameObject.transform.position;

        #region デバック用
        playerPos = gameObject.transform.position;
        playreRotate = gameObject.transform.rotation;
        #endregion
    }

    void Update()
    {
        Drive();
    }

    /// <summary>
    /// ＊移動処理
    /// </summary>
    void Drive()
    {
        float axel = Input.GetAxis("Axel");
        float curve = Input.GetAxis("Curve");

        transform.position += transform.forward * axel * speed; //移動
        if (Mathf.Abs(axel) >= 0.1f) transform.Rotate(new Vector3(0.0f, curve * rotate, 0.0f)); //カーブ

        //if (Mathf.Abs(axel) >= 0.1f) //カーブ
        //{
        //    if (axel > 0) transform.Rotate(new Vector3(0.0f, curve * rotate, 0.0f)); //前進時
        //    else transform.Rotate(new Vector3(0.0f, -curve * rotate, 0.0f)); //後進時
        //}

        #region デバック用（初期位置へワープ）
        if (Input.GetButtonDown("Restart")) //初期位置へワープ（デバック用）
        {
            transform.position = playerPos;
            transform.rotation = playreRotate;
        }
        #endregion
    }
}