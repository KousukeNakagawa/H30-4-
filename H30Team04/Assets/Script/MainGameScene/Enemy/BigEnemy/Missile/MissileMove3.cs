using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove3 : MonoBehaviour {

    /********** ただ前方向に進むだけのミサイル **********/
    
    private Rigidbody m_rigid;
    public Vector3 targetPos;
    public float deadTime = 3.0f;

    private static List<GameObject> failureMissiles = new List<GameObject>();

    [SerializeField] private AudioSource m_audio;

    void Awake()
    {
        failureMissiles.Add(gameObject);
    }

    // Use this for initialization
    void Start () {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.useGravity = false;
        transform.LookAt(targetPos);
        m_audio.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        m_rigid.AddForce(transform.forward * Time.deltaTime * 10.0f,ForceMode.Impulse);
	}

    void Update()
    {
        if (TowerBreak.isBreak) Check();
    }

    private void Check()
    {
        Destroy(gameObject, deadTime);
    }

    void OnDestroy()
    {
        failureMissiles.Remove(gameObject);
    }
}
