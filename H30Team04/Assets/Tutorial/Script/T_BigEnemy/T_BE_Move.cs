using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_BE_Move : MonoBehaviour
{
    [SerializeField, Range(0, 5)] float speed = 1;

    void Start()
    {

    }

    void Update()
    {
        Look();
        Advance();
    }

    /// <summary> 前進処理 </summary>
    void Advance()
    {
        var force = T_BE_Serch.Forward * speed * Time.deltaTime;
        transform.Translate(force);
    }

    /// <summary> 進行方向変更処理 </summary>
    void Look()
    {
        if (T_BE_Serch.Forward != transform.forward)
            transform.LookAt(T_BE_Serch.Forward);
    }
}
