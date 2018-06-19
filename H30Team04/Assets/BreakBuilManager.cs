using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreakBuilManager : MonoBehaviour {

    private Rigidbody[] m_rigids;

	// Use this for initialization
	void Start () {
        m_rigids = GetComponentsInChildren<Rigidbody>();
        //gameObject.SetActive(false);
        Vector3 dir = transform.position - BigEnemyScripts.mTransform.position;
        //Break(BigEnemyScripts.mTransform.position, 50.0f, 1000.0f);
        //Break(transform.position + dir.normalized * dir.magnitude, 50.0f, 1000.0f);
	}

    public void Break(Vector3 position,float range,float power)
    {
        Array.ForEach(m_rigids, (c) => c.transform.parent = null);
        Array.ForEach(m_rigids, (c) => c.isKinematic = false);
        Array.ForEach(m_rigids, (c) => c.AddExplosionForce(power, position, range));
        Array.ForEach(m_rigids, (c) => c.AddTorque(UnityEngine.Random.insideUnitSphere * power / 15.0f, ForceMode.Force));
    }

    void OnCollisionEnter(Collision other)
    {
        float distance = float.MaxValue;
        Vector3 hit = new Vector3();
        foreach (var c in other.contacts)
        {  //一番近いポイントを見つける
            float d = Vector3.Distance(transform.position, c.point);
            if (d < distance)
            {
                distance = d;
                hit = c.point;
            }
        }
        //どーせねぇけど
        if (hit == Vector3.zero) return;

        Break(hit, 100.0f, 1000.0f);
        Vector3 dir = transform.position - hit;
        Break(hit, 100.0f, 1000.0f);
        Break(transform.position + dir.normalized * dir.magnitude, 100.0f, 1000.0f);
    }
}
