using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHelicopter : MonoBehaviour {

    //[SerializeField] private Vector3 rotatePoint = new Vector3(-40, 0, -20);
    //[SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform enemyT;
    [SerializeField] private float rotateSpeed = 60.0f;
    [SerializeField] private float byebyeSpeed = 5.0f;
    [SerializeField] private float byebyeRotateY;
    [SerializeField] private float byebyeRotateZ;
    //[SerializeField] private int scenarioCount = 3;
    [SerializeField] private ViewOutChange m_view;
    [SerializeField] private GameObject titleUI;

    private bool is_byebye = false;

    [SerializeField] private Transform direction;

    private Vector3 startAngle;
    private Quaternion startAngle2;
    private float lerpTime = 0.0f;
    [SerializeField] private float endrotateSpeed = 1.0f;

    // Use this for initialization
    void Start () {
        //enemyT = GameObject.FindGameObjectWithTag("BigEnemy").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (is_byebye)
        {
            transform.position += direction.forward * byebyeSpeed * Time.deltaTime;
            if(lerpTime < 1.0f)
            {
                lerpTime += Time.deltaTime / endrotateSpeed;
                //transform.eulerAngles = Vector3.Lerp(startAngle, direction.eulerAngles, lerpTime);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, byebyeRotateY, byebyeRotateZ), lerpTime);
                transform.rotation = Quaternion.Slerp(startAngle2, direction.rotation, lerpTime);
            }
            
        }
        else
        {
            transform.LookAt(enemyT.position);
        }

    }

    public void SetScene(string name)
    {
        m_view.SetSceneName(name);
        is_byebye = true;
        Vector3 a = Camera.main.transform.eulerAngles;
        a.x = 0;
        a.y += byebyeRotateY;
        a.z += byebyeRotateZ;
        direction.eulerAngles = a;

        transform.parent.GetComponent<FollowCamera>().enabled = false;
        transform.parent.parent = null;

        startAngle = transform.eulerAngles;
        startAngle2 = transform.rotation;

        titleUI.SetActive(false);
    }
}
