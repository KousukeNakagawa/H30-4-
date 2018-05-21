using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの切替（同時にカメラが２つ以上になることはない）
/// </summary>
public class CameraManager : MonoBehaviour
{
    GameObject player;
    PlayerBase playerScript;
    List<GameObject> cameras = new List<GameObject>();
    [SerializeField] GameObject SE_camera;
    [SerializeField] GameObject playerCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();

        //全てのカメラをリスト化
        cameras.Add(SE_camera);
        cameras.Add(playerCamera);

        SetActiveCamera(SE_camera);
    }

    void Update()
    {
        //開始演出終了時
        if (playerScript.GetIsEndSE()) SetActiveCamera(playerCamera);
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
