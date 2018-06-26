using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class BreakBuilManager : MonoBehaviour
{

    [SerializeField] private GameObject crashSmoke;
    private Rigidbody[] m_rigids;
    private float distance = 7.5f;

    private Vector3 hitp;

    // Use this for initialization
    void Start()
    {
        m_rigids = GetComponentsInChildren<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hitp, 0.5f);
    }

    public void Break(Vector3 position, float range, float power)
    {
        Array.ForEach(m_rigids, (c) => c.mass = float.MaxValue);
        Array.ForEach(m_rigids, (c) => c.transform.parent = null);
        Array.ForEach(m_rigids, (c) => c.isKinematic = false);
        Rigidbody[] rigids = Array.FindAll(m_rigids, (c) => Vector3.Distance(c.transform.position, position) < distance);
        Array.ForEach(rigids, (c) => c.AddExplosionForce(power, position, range));
        Array.ForEach(rigids, (c) => c.AddTorque(UnityEngine.Random.insideUnitSphere * power / 15.0f, ForceMode.Force));
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("BigEnemy")) return;
        GetComponent<Collider>().enabled = false;
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
        Break(hit, 10.0f, 10.0f);
        hitp = hit;
        GameObject g = Instantiate(crashSmoke, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //Array.ForEach(m_rigids, (f) => Destroy(f.gameObject, 5f));
    }
}
