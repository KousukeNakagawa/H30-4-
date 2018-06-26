using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCollider : MonoBehaviour {

    private Collider m_coll;
    public GameManager gm;

    private bool isColliderOn = false;

	// Use this for initialization
	void Start () {
        m_coll = GetComponent<Collider>();
        m_coll.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_coll.enabled)
        {
            this.enabled = false;
            return;
        }
        if(gm != null)
        {
            if (gm.NowState() == GameManager.PhaseState.switchState)
            {
                m_coll.enabled = true;
            }
        }
        else
        {
            if (isColliderOn)
            {
                m_coll.enabled = true;
            }
        }
    }

    public void ColliderOn()
    {
        isColliderOn = true;
    }
}
