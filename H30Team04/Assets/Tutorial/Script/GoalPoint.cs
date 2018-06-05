using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    GameObject player;
    [SerializeField, Range(1, 90)] float range;

    public static bool Goal { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Goal = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (player) Goal = true;
    }

    /// <summary> 範囲 </summary>
    bool IsPlayerInVierAngle()
    {
        //方向
        var dir = transform.forward;
        //差分角度
        var angle = Vector3.Angle(transform.forward, dir);
        //見える視野角なら true
        return (Mathf.Abs(angle) <= range);
    }
}
