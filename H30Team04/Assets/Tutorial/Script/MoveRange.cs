using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRange : MonoBehaviour
{
    static SphereCollider range;

    public bool RangeEnter { get; set; }

    void Awake()
    {
        //range = GetComponent<SphereCollider>();
        RangeEnter = false;
    }

    void OnTriggerExit(Collider other)
    {
        //プレイヤーが範囲外にでたら
        if (other.gameObject.CompareTag("Player"))
            RangeEnter = true;
    }

    /// <summary> 行動範囲の設定 </summary>
    public void SetRange(Vector3 position, float radius)
    {
        range.center = position;
        range.radius = radius;
    }
}
