using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> プレイヤーの行動範囲 </summary>
public class MoveRange : MonoBehaviour
{
    static SphereCollider range;
    static Transform _transform;

    /// <summary> プレイヤーが範囲から出たか </summary>
    public static bool RangeExit { get; private set; }

    void Awake()
    {
        range = GetComponent<SphereCollider>();
        _transform = gameObject.transform;
        RangeExit = false;
    }

    void OnTriggerExit(Collider other)
    {
        //プレイヤーが範囲外にでたら
        if (other.gameObject.CompareTag("Player"))
            RangeExit = true;
    }

    /// <summary> 行動範囲の設定 </summary>
    public static void SetRange(Vector3 position, float radius)
    {
        _transform.position = position;
        range.radius = radius;
    }
}