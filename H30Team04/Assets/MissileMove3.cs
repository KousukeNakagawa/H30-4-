using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove3 : MonoBehaviour {

    private Rigidbody m_rigid;
    public Vector3 targetPos;

    private static List<GameObject> failureMissiles = new List<GameObject>();

    private void Awake()
    {
        failureMissiles.Add(gameObject);
    }

    // Use this for initialization
    void Start () {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.useGravity = false;
        transform.LookAt(targetPos);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        m_rigid.AddForce(transform.forward * Time.deltaTime * 10.0f,ForceMode.Impulse);
	}
}
