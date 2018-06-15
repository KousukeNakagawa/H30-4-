using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> チュートリアルロボ用対象検索 </summary>
public class T_BE_Serch : MonoBehaviour
{
    GameObject beacon;

    /// <summary> 進行方向 </summary>
    public static Vector3 Forward { get; private set; }

    void Start()
    {
        Forward = transform.forward;
    }

    void Update()
    {
        Forward = (beacon != null) ?
            beacon.transform.position.normalized : transform.forward;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) beacon = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) beacon = null;
    }
}