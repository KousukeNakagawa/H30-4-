using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPhaseMove : MonoBehaviour {

    [SerializeField] private Behaviour[] contents;
    [Header("Y座標は関係ありません")]
    [SerializeField,Tooltip("射撃フェーズでの目標座標")] private Vector3 targetPos;
    [Tooltip("射撃フェーズでの移動スピード")]public float moveSpeed = 1.0f;
    [HideInInspector]public List<GameObject> makebyRobot = new List<GameObject>();

    private bool isShooting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isShooting) return;
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);
	}

    public void ShootingPhaseSet()
    {
        List<Behaviour> scripts = new List<Behaviour>(GetComponentsInChildren<Behaviour>());
        foreach (var script in contents)
        {
            scripts.RemoveAll((f => f.Equals(script)));
        }
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
        foreach (var make in makebyRobot)
        {
            Destroy(make);
        }
        isShooting = true;

        BigEnemyScripts.mTransform.rotation = Quaternion.Euler(BigEnemyScripts.bigEnemyMove.TurnAngleSet(targetPos));
    }
}
