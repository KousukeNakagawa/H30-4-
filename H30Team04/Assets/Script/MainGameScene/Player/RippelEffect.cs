using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> レーザーポインターの命中エフェクト </summary>
public class RippelEffect : MonoBehaviour
{
    Material material;
    Color color;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        //装備中の武器によって色を変える
        color = (WeaponCtrl.WeaponBeacon) ? Color.blue : Color.red;
        material.SetColor("_TintColor", color);
    }
}