using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSeacrch : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !BigEnemyScripts.missileLaunch.isMissile && !BigEnemyScripts.bigEnemyMove.isTurn)
        {
            BigEnemyScripts.missileLaunch.isMissile = true;
            BigEnemyScripts.searchObject.targetPos = other.transform.position;
            BigEnemyScripts.missileLaunch.LaunchSet();
        }
    }
}
