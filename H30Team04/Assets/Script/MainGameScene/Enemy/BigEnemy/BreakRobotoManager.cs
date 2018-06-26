using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreakRobotoManager : MonoBehaviour
{

    [SerializeField] private GameObject bodyMain;
    [SerializeField] private GameObject breakRobot;
    private Rigidbody[] m_rigids;
    public float explosionPower = 10.0f;
    public float explosionRange = 5.0f;
    public float rotateSpeed = 2.0f;
    private bool isBreak;

    void Start()
    {
        m_rigids = breakRobot.GetComponentsInChildren<Rigidbody>();
    }

    void Update()
    {
        if (!isBreak) return;
        Array.ForEach(m_rigids, (c) => c.AddForce(Physics.gravity * 3.0f));
    }

    public void BreakRobotAction()
    {
        bodyMain.SetActive(false);
        breakRobot.SetActive(true);
        Array.ForEach(m_rigids, (c) => c.isKinematic = false);
        Array.ForEach(m_rigids, (c) => c.transform.parent = null);
        Array.ForEach(m_rigids, (c) => c.AddExplosionForce(explosionPower, 
            BigEnemyScripts.mTransform.position + new Vector3(-10.0f, 0f, 0f), explosionRange));
        Array.ForEach(m_rigids, (c) => c.AddTorque(UnityEngine.Random.insideUnitSphere * rotateSpeed, ForceMode.VelocityChange));
        isBreak = true;
    }
}
