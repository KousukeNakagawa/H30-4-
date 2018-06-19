using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneCreate : MonoBehaviour
{
    [SerializeField,Tooltip("ドローンのプレファブ")] private GameObject dronePrefab;
    private int droneCount = 2;

    private List<GameObject> drones = new List<GameObject>();
    [SerializeField, Tooltip("ドローンが再生成される間隔")] private float droneCreateCount = 20.0f;
    private float droneCreateTime;
    [HideInInspector] public bool isEnd { get; private set; }

    // Use this for initialization
    void Awake()
    {
        isEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (droneCreateTime > 0) droneCreateTime -= Time.deltaTime;
        else droneCreateTime = 0;
        if (drones.Count == 0 || isEnd) return;
        isEnd = true;
        foreach (var drone in drones)
        {
            if (!drone.GetComponent<DroneMove2>().IsStart)
            {
                isEnd = false;
            }
        }
    }

    public void DroneSet()
    {  //ドローンを生成する
        if (drones.Count != 0 || droneCreateTime > 0 || dronePrefab == null) return;
        //GameTextController.TextStart(6);
        for (int i = 0; i < droneCount; i++)
        {
            drones.Add(Instantiate(dronePrefab, BigEnemyScripts.droneInstantiatePos.position, Quaternion.identity));
        }
        drones[0].GetComponent<DroneMove2>().droneDirection = DroneMove2.DroneDirection.Advance;
        drones[1].GetComponent<DroneMove2>().droneDirection = DroneMove2.DroneDirection.Recession;
        droneCreateTime = droneCreateCount;
        isEnd = false;
    }

    public void RemoveDrone(GameObject drone)
    {  //ドローンを削除する
        drones.Remove(drone);
        if (drones.Count == 0) drones.Clear();
    }
}
