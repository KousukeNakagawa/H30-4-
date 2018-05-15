using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを隠したり、武器の情報を持つ
/// </summary>
public class CameraController : MonoBehaviour
{
    //必須ゲームオブジェクト
    GameObject player;
    PlayerBase playerScript;

    bool isWeaponBeacon; //武器の切替用

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();

        isWeaponBeacon = true;
    }

    void Update()
    {
        if (!playerScript.GetIsEndSE()) return;

        WeaponChanger();
    }

    /// <summary>
    /// ＊武器の切替
    /// </summary>
    void WeaponChanger()
    {
        //武器の切替
        if (Input.GetButtonDown("WeaponChange")) isWeaponBeacon = !isWeaponBeacon;
    }

    /// <summary>
    /// ＊プレイヤーとカメラの表示を消す
    /// </summary>
    public void Hide()
    {
        player.SetActive(false);
    }

    /// <summary>
    /// 装備中の武器（true = beacon / false = snipe）
    /// </summary>
    public bool GetWeapon()
    {
        return isWeaponBeacon;
    }
}
