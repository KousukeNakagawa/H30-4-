using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject player;
    PlayerBase playerScript;
    List<GameObject> cameras = new List<GameObject>();
    [SerializeField] GameObject SE_camera;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject freeCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();

        //全てのカメラをリスト化
        cameras.Add(SE_camera);
        cameras.Add(playerCamera);
        cameras.Add(freeCamera);

        SetActiveCamera(SE_camera);
    }

    void Update()
    {
        //開始演出終了時
        if (playerScript.GetIsEndSE()) SetActiveCamera(playerCamera);

        //プレイヤー消滅時
        if (player == null) SetActiveCamera(freeCamera);
    }

    /// <summary>
    /// 使用するカメラの選択
    /// </summary>
    void SetActiveCamera(GameObject activCamera)
    {
        //選択されたカメラを表示
        activCamera.SetActive(true);

        //選択されなかったカメラを全て非表示
        foreach (var camera in cameras) if (camera != activCamera) camera.SetActive(false);
    }
}
