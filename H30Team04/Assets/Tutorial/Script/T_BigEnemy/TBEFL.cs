using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBEFL : MonoBehaviour
{
    /// <summary> 地に足がついているか </summary>
    public static bool IsHitGround { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        // 地面についたら true
        if (other.CompareTag("Field")) IsHitGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        // 地面から離れたら false
        if (other.CompareTag("Field")) IsHitGround = false;
    }
}
