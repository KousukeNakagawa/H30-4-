using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

    [SerializeField] private Transform target;
    private bool isSwitch = false; //ビルの上に移動し終わったかどうか
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float speed = 0.5f;


    // Use this for initialization
    void Start () {
        //transform.position = Camera.main.transform.position;
        //transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (isSwitch)
        {
            //if (Fade.IsFadeEnd())
            //{

            //}

            //transform.position += transform.forward * speed * Time.deltaTime;
            if (Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) < 1 || transform.position.z > target.position.z +0.5f) Destroy(transform.parent.gameObject);
        }
        else
        {
            if (Fade.IsFadeOutOrIn()&&Fade.IsFadeEnd()) //梯子の移動が終わり、フェードが終わったら
            {
                isSwitch = true;
                transform.position = target.position + target.forward * distance;
                Vector3 angle = target.eulerAngles;
                angle.y += 180;
                transform.eulerAngles = angle;
                GetComponent<FollowCamera>().enabled = true;
                //target.Find("Camera").GetComponent<AudioListener>().enabled = true;
                GetComponent<AudioListener>().enabled = false;
                //GameObject.FindGameObjectWithTag("Player").SetActive(false);
                target.gameObject.SetActive(true);
                Fade.FadeIn();
            }
        }
	}
    

}
