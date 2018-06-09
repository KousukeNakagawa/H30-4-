using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDown : MonoBehaviour
{
    private Rigidbody m_rigid;
    private Vector3 primaryPos;
    [SerializeField, Range(0.0f, 5.0f)] private float torquePower = 2.0f;
    private bool isHit = false;
    [SerializeField,Range(0.0f,6.0f)] private float dropdownPower = 3.0f;
    public float deleteTime = 2.0f;
    private float deleteCount;

    // Use this for initialization
    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        primaryPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = primaryPos;
        //要調整
        if (m_rigid.velocity.sqrMagnitude > dropdownPower)
        {
            gameObject.layer = LayerMask.NameToLayer("StageObject");
        }
        if (deleteCount > 0)
        {
            deleteCount -= Time.deltaTime;
            if (deleteCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isHit) return;
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("BigEnemy"))
        {
            BreakDownAction(other.transform);
        }
    }
    public void BreakDownAction(Transform target)
    {
        Vector3 dir = -(target.position - transform.position).normalized;
        m_rigid.useGravity = true;
        m_rigid.AddForce(dir * torquePower, ForceMode.VelocityChange);
        isHit = true;
        deleteCount = deleteTime;
    }
}
