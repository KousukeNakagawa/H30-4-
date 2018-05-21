using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHelicopter : MonoBehaviour {

    //[SerializeField] private Vector3 rotatePoint = new Vector3(-40, 0, -20);
    //[SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform enemyT;
    [SerializeField] private float rotateSpeed = 60.0f;
    [SerializeField] private float byebyeSpeed = 5.0f;
    [SerializeField] private float byebyeRotate;
    //[SerializeField] private int scenarioCount = 3;
    [SerializeField] private ViewOutChange m_view;
    [SerializeField] private GameObject titleUI;

    private bool is_byebye = false;

	// Use this for initialization
	void Start () {
        //enemyT = GameObject.FindGameObjectWithTag("BigEnemy").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (is_byebye)
        {
            transform.position += transform.forward * byebyeSpeed * Time.deltaTime;
        }
        else
        {
            //transform.RotateAround(enemyT.position, Vector3.up, -rotateSpeed * Time.deltaTime);
            transform.LookAt(enemyT.position);

            //if (Input.GetKeyDown(KeyCode.Z)|| Input.GetAxisRaw("RT") < 0)
            //{
            //    scenarioCount--;
            //    if(scenarioCount <= 0)
            //    {
            //        is_byebye = true;
            //        Vector3 a = transform.eulerAngles;
            //        a.x = 0;
            //        a.y += byebyeRotate;
            //        transform.eulerAngles = a;
            //    }

            //}
        }

    }

    public void SetScene(string name)
    {
        m_view.SetSceneName(name);
        is_byebye = true;
        Vector3 a = transform.eulerAngles;
        a.x = 0;
        a.y += byebyeRotate;
        transform.eulerAngles = a;
        titleUI.SetActive(false);
    }
}
