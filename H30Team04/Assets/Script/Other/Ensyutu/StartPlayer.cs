using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayer : MonoBehaviour {

    private Rigidbody m_Rigid;
    [SerializeField] private float dashPawer = 100f;

	// Use this for initialization
	void Start () {
        m_Rigid = GetComponent<Rigidbody>();
        m_Rigid.AddForce(transform.forward * dashPawer, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if(m_Rigid.velocity.x <= 1)
        {
            transform.parent = Camera.main.transform;
        }

        if (Input.GetKeyDown(KeyCode.P) && transform.parent != null)
        {
            transform.parent = null;
            m_Rigid.AddForce(transform.forward * dashPawer, ForceMode.Impulse);
        }

    }
}
