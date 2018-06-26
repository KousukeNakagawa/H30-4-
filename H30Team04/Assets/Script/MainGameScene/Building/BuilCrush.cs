using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuilCrush : MonoBehaviour {


    [SerializeField] private int builsize = 0;
    private bool isCrush = false;
    private Vector3 startPos;
    public float downSpeed = 2.0f;
    public GameObject crashSmoke;
    private GameObject currentSmoke = null;

    private AudioSource m_audio;
    [SerializeField] private GameObject breakBuild;
    private Vector3 m_size;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
        m_audio = GetComponent<AudioSource>();
        Array.ForEach(breakBuild.GetComponentsInChildren<MeshRenderer>(), (MeshRenderer f) => f.enabled = false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) return;
        m_size = GetComponent<BoxCollider>().size;
        breakBuild.transform.localScale = new Vector3(m_size.x / 8f, m_size.y / 16f, m_size.z / 8f);
        if (isCrush)
        {
            if(transform.position.y < -MainStageDate.BuildingHeight * builsize)
            {
                if (currentSmoke != null)
                    currentSmoke.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
                Destroy(currentSmoke, 2.5f);
                Destroy(gameObject);
            }
            Vector3 newPos = startPos;
            newPos.y = transform.position.y - ((MainStageDate.BuildingHeight * builsize) / downSpeed) * Time.deltaTime;
            newPos.x += UnityEngine.Random.Range(-1.0f, 1.0f);
            newPos.z += UnityEngine.Random.Range(-1.0f, 1.0f);
            transform.position = newPos;
        }
	}
    
    public int Builsize
    {
        set { builsize = value; }
        get { return builsize; }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "BigEnemy")
        {
            if(ScecnManager.NowSceneName() == "GamePlay")GameTextController.TextStart(2);
            if (m_audio != null) m_audio.Play();
            //isCrush = true;
            GetComponent<Collider>().enabled = false;
            //currentSmoke = Instantiate(crashSmoke, transform.Find("builunder").position, Quaternion.identity);
            //Vector3 size = GetComponent<BoxCollider>().size;
            //ParticleSystem.ShapeModule shape = currentSmoke.GetComponent<ParticleSystem>().shape;
            //shape.scale = new Vector3(size.x / 8.0f * 1.2f, size.z / 8.0f * 1.2f, 1);
            breakBuild.GetComponent<BreakBuilManager>().BreakAction(other,m_size);
            gameObject.SetActive(false);
        }
    }
}