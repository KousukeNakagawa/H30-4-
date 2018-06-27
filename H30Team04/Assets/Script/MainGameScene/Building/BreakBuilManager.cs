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
        Array.ForEach(m_rigids, (f) => 
        {
            var bre = f.GetComponent<BreakBuildDestroy>();
            if (bre != null) bre.enabled = false;
        });
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
        foreach (var r in m_rigids)
        {
            var bre = r.GetComponent<BreakBuildDestroy>();
            if (bre != null) bre.enabled = true;
            else Destroy(r.gameObject, 4f);
        }
    }

    public void BreakAction(Collision other,Vector3 size)
    {
        Array.ForEach(GetComponentsInChildren<MeshRenderer>(), (MeshRenderer m) => m.enabled = true);
        Array.ForEach(GetComponentsInChildren<MeshCollider>(), (MeshCollider f) => f.enabled = true);
        //GetComponent<Collider>().enabled = false;
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
        var shape = g.GetComponent<ParticleSystem>().shape;
        shape.scale = new Vector3(size.x / 8.0f * 1.2f, size.z / 8.0f * 1.2f, 1);
        Destroy(gameObject);
    }
}
