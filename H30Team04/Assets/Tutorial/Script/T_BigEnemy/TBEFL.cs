using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBEFL : MonoBehaviour
{
    /// <summary> 地に足がついているか </summary>
    public static bool IsHitGround { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Field")) IsHitGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Field")) IsHitGround = false;
    }
}
