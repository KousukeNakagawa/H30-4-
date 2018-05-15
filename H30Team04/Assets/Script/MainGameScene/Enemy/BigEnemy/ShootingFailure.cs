using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingFailure : MonoBehaviour {

    public GameObject missilePrefab;
    public Vector3 targetPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void FailureAction()
    {
        if (!BigEnemyScripts.shootingPhaseMove.isShooting) return;
        BigEnemyScripts.searchObject.targetPos = targetPos;
    }
}
