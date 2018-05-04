using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilCrush : MonoBehaviour {


    private int builsize = 0;
    private bool isCrush = false;
    private Vector3 startPos;
    public float downSpeed = 2.0f;

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
        if (collision.transform.tag == "BigEnemy"){
            isCrush = true;
            GetComponent<Collider>().enabled = false;
            //Destroy(gameObject);
        }
    }


}
