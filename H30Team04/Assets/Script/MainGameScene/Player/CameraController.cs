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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();
    }

    /// <summary>
    /// ＊プレイヤーとカメラの表示を消す
    /// </summary>
    public void Hide()
    {
        player.SetActive(false);
    }
}
