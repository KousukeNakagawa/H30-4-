using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayer : MonoBehaviour {

    private Rigidbody m_Rigid;
    [SerializeField] private float dashPawer = 100f;
    [SerializeField] private float seconddashPawer = 100f;
    [SerializeField] private int scenarioCount = 3;

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

        if (Input.GetAxisRaw("RT") < 0 && transform.parent != null)
        {
            scenarioCount--;
            if (scenarioCount == 0)
            {
                transform.parent = null;
                m_Rigid.AddForce(transform.forward * seconddashPawer, ForceMode.Impulse);
            }
        }

    }
}
