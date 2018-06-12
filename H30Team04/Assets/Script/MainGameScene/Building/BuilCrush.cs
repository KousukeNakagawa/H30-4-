using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilCrush : MonoBehaviour {


    [SerializeField] private int builsize = 0;
    private bool isCrush = false;
    private Vector3 startPos;
    public float downSpeed = 2.0f;
    public GameObject crashSmoke;
    private GameObject currentSmoke;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (isCrush)
        {
            if(transform.position.y < -MainStageDate.BuildingHeight * builsize)
            {
                Destroy(gameObject);
            }
            Vector3 newPos = startPos;
            newPos.y = transform.position.y - ((MainStageDate.BuildingHeight * builsize) / downSpeed) * Time.deltaTime;
            newPos.x += Random.Range(-1.0f, 1.0f);
            newPos.z += Random.Range(-1.0f, 1.0f);
            transform.position = newPos;
        }
	}
    
    public int Builsize
    {
        set { builsize = value; }
        get { return builsize; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "BigEnemy")
        {
            GameTextController.TextStart(2);
            isCrush = true;
            GetComponent<Collider>().enabled = false;
            currentSmoke = Instantiate(crashSmoke, transform.Find("builunder").position, Quaternion.identity);
            Vector3 size = GetComponent<BoxCollider>().size;
            ParticleSystem.ShapeModule shape = currentSmoke.GetComponent<ParticleSystem>().shape;
            shape.scale = new Vector3(size.x / 8.0f * 1.2f, size.z / 8.0f * 1.2f, 1);
        }
    }

    void OnDestroy()
    {
        if (currentSmoke == null) return;
        currentSmoke.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
        Destroy(currentSmoke, 2.5f);
    }
}