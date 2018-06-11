using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichizureCollider : MonoBehaviour {

    public Collider targetColl;
    private Collider m_coll;

	// Use this for initialization
	void Start () {
        m_coll = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        m_coll.enabled = targetColl.enabled;
	}
}
