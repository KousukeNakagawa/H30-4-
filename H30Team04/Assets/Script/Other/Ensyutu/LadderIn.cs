using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderIn : MonoBehaviour {

    [SerializeField] private Transform targetPos;
    [SerializeField] private float speed = 1.0f;

    private int num = 0;

	// Use this for initialization
	void Start () {
        targetPos = XrayMachines.xrayMachineObjects[num].transform;

    }
	
	// Update is called once per frame
	void Update () {
		if(targetPos.position != transform.position)
        {
            Vector3 direction = targetPos.position - transform.position;
            if (direction.sqrMagnitude < 0.1f)
            {
                num++;
                num %= XrayMachines.xrayMachineObjects.Count;
                targetPos = XrayMachines.xrayMachineObjects[num].transform;
                return;
            }
            direction = Vector3.Normalize(direction);
            transform.position += direction * speed * Time.deltaTime;
        }
	}
}
