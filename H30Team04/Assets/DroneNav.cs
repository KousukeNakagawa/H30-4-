using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneNav : MonoBehaviour {

    public enum DroneDirection
    {
        Advance,  //前
        Recession  //後ろ
    }

    public DroneDirection m_direction;
    private Collider m_collider;
    private NavMeshAgent m_nav;
    [SerializeField] private GameObject targetPoses;
    [SerializeField] private float m_speed = 4.0f;
    private List<Vector3> poses = new List<Vector3>();

    private int currentNum = -1;
    private bool isEnd = false;

	void Start () {
        m_collider = GetComponent<Collider>();
        m_nav = GetComponent<NavMeshAgent>();

        m_nav.speed = m_speed;

        Vector3 dir = Vector3.zero;
        if (m_direction == DroneDirection.Recession) dir.y = 180.0f;

        GameObject target = Instantiate(targetPoses, transform.position, Quaternion.Euler(dir));
        foreach (Transform pos in target.transform)
        {
            poses.Add(pos.position);
        }
        NextTarget();
	}

	void FixedUpdate () {
		if (targetDistance <= 0.5f)
        {
            NextTarget();
        }
        if (isEnd)
        {
            transform.Translate(Vector3.right * m_speed * Time.deltaTime, Space.Self);
        }
	}

    private void NextTarget()
    {
        currentNum++;
        if (currentNum >= poses.Count)
        {
            isEnd = true;
            m_nav.enabled = false;
            transform.parent = BigEnemyScripts.mTransform;
            transform.rotation = Quaternion.identity;
            return;
        }

        m_nav.destination = poses[currentNum];
    }

    private float targetDistance
    {
        get
        {
            return Vector2.Distance(transform.position.ToTopView(), m_nav.destination.ToTopView());
        }
    }
}
