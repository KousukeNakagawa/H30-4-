using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> カメラに映ったら色が変わる回転チュートリアル用オブジェクト </summary>
public class LookPoint : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Color startColor;
    [SerializeField] Color lookColor;

    Color color;
    Material material;

    /// <summary> プレイヤーの視野に入っているか </summary>
    public bool IsLook { get; private set; }

    void Start()
    {
        material = GetComponent<Renderer>().material;
        IsLook = false;
    }

    void Update()
    {
        IsLook = (color == lookColor) ? true : false;

        material.SetColor("_EmissionColor", color * 10);
        color = startColor;
    }

    void OnWillRenderObject()
    {
        //プレイヤーカメラに映ったら
        if (Camera.current.name != "SceneCamera" &&
            Camera.current.name != "Preview Camera")
            //色を変更
            if (playerCamera) color = lookColor;
    }
}
