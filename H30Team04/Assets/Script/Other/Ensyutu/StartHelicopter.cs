using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHelicopter : MonoBehaviour {
    
    [SerializeField] private Transform enemyT;
    [SerializeField] private float rotateSpeed = 60.0f;
    [SerializeField] private float byebyeSpeed = 5.0f;
    [SerializeField] private float byebyeRotateY;
    [SerializeField] private float byebyeRotateZ;
    [SerializeField] private ViewOutChange m_view;
    [SerializeField] private GameObject titleUI;

    private bool is_byebye = false;

    [SerializeField] private Transform direction;

    private Vector3 startAngle;
    private Quaternion startAngle2;
    private float lerpTime = 0.0f;
    [SerializeField] private float endrotateSpeed = 1.0f;

    public AudioClip kettei;
    private AudioSource audioSourse;
    private AudioSource firstAudioSourse;

    // Use this for initialization
    void Start () {
        firstAudioSourse = gameObject.GetComponent<AudioSource>();
        audioSourse = gameObject.AddComponent<AudioSource>();
        audioSourse.outputAudioMixerGroup = firstAudioSourse.outputAudioMixerGroup;
    }
	
	// Update is called once per frame
	void Update () {
        if (is_byebye)
        {
            transform.position += direction.forward * byebyeSpeed * Time.deltaTime;
            if(lerpTime < 1.0f)
            {
                lerpTime += Time.deltaTime / endrotateSpeed;
                transform.rotation = Quaternion.Slerp(startAngle2, direction.rotation, lerpTime);
            }
            
        }
        else
        {
            transform.LookAt(enemyT.position);
        }

    }

    public void SetScene(string name)
    {
        audioSourse.PlayOneShot(kettei);
        m_view.SetSceneName(name);
        is_byebye = true;
        Vector3 a = Camera.main.transform.eulerAngles;
        a.x = 0;
        a.y += byebyeRotateY;
        a.z += byebyeRotateZ;
        direction.eulerAngles = a;

        transform.parent.GetComponent<FollowCamera>().enabled = false;
        transform.parent.parent = null;

        startAngle = transform.eulerAngles;
        startAngle2 = transform.rotation;

        titleUI.SetActive(false);
    }
}
