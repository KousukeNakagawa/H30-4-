using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDroneMove : MonoBehaviour {

    [SerializeField] private float horizontalMoveTime = 1.0f;
    [SerializeField] private float verticalMoveTime = 1.0f;
    [SerializeField] private float horizontalMoveSpeed = 3.0f;
    [SerializeField] private float verticalMoveSpeed = 3.0f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (horizontalMoveTime > 0)
        {
            horizontalMoveTime -= Time.deltaTime;
            transform.position += transform.forward * horizontalMoveSpeed * Time.deltaTime;
        }
        else if (verticalMoveTime > 0)
        {
            verticalMoveTime -= Time.deltaTime;
            transform.position += Vector3.Normalize(transform.forward - transform.right + transform.up) * verticalMoveSpeed * Time.deltaTime;
        }
        else
        {
            TutorialEnemyScripts.bigEnemyMove.IsMove = true;
            this.enabled = false;
        }
    }
}
