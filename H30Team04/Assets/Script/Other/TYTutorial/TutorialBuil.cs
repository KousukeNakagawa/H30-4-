using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuil : MonoBehaviour {

    [SerializeField] private int builsize = 0;
    private bool isCrush = false;
    private Vector3 startPos;
    public float downSpeed = 2.0f;
    public GameObject crashSmoke;
    private GameObject currentSmoke;

    private AudioSource m_audio;

    public bool isNoCrush = false;
    public TutorialManager_T tmane;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        m_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrush && !isNoCrush)
        {
            if (transform.position.y < -MainStageDate.BuildingHeight * builsize)
            {
                gameObject.SetActive(false);
                if (currentSmoke == null) return;
                currentSmoke.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
                Destroy(currentSmoke, 2.5f);
            }
            Vector3 newPos = startPos;
            newPos.y = transform.position.y - ((MainStageDate.BuildingHeight * builsize) / downSpeed) * Time.deltaTime;
            newPos.x += Random.Range(-1.0f, 1.0f);
            newPos.z += Random.Range(-1.0f, 1.0f);
            transform.position = newPos;
        }
        else if (isCrush && isNoCrush)
        {
            gameObject.SetActive(false);
            if (currentSmoke == null) return;
            currentSmoke.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
            Destroy(currentSmoke, 2.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "BigEnemy")
        {
            if (ScecnManager.NowSceneName() == "GamePlay") GameTextController.TextStart(2);
            if (m_audio != null) m_audio.Play();
            isCrush = true;
            GetComponent<Collider>().enabled = false;
            currentSmoke = Instantiate(crashSmoke, transform.Find("builunder").position, Quaternion.identity);
            Vector3 size = GetComponent<BoxCollider>().size;
            ParticleSystem.ShapeModule shape = currentSmoke.GetComponent<ParticleSystem>().shape;
            shape.scale = new Vector3(size.x / 8.0f * 1.2f, size.z / 8.0f * 1.2f, 1);
        }
    }

    public void BuilReset()
    {
        GetComponent<Collider>().enabled = true;
        isCrush = false;
        transform.position = startPos;

        foreach(GameObject child in gameObject.GetAllChildren())
        {
            if (child.transform.tag == "Beacon") Destroy(child);
        }

        if (currentSmoke == null) return;
        currentSmoke.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
        Destroy(currentSmoke, 2.5f);
    }
}
