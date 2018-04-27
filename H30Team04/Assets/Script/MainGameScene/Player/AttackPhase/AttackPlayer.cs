using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour {

    private GameObject[] weekPoints;
    private Transform target;
    private int selectNum = 0;
    



	// Use this for initialization
	void Start () {

    }

    private void Awake()
    {
        weekPoints = GameObject.FindGameObjectsWithTag("WeekPoint");
        //Debug.Log(weekPoints[0]);
        GameObject t = Resources.Load("Prefab/PlayerSide/AttackTarget") as GameObject;
        //GameObject.Instantiate(t);
        target = GameObject.Instantiate(t).transform;
        target.position = weekPoints[selectNum].transform.position;
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            selectNum++;
            if (selectNum >= weekPoints.Length) selectNum = 0;
        }
        target.position = Vector3.Lerp(target.position, weekPoints[selectNum].transform.position, 0.5f);
        transform.LookAt(target);
    }
}
