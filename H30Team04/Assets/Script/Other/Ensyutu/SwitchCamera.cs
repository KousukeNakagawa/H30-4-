using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

    [SerializeField] private Transform target;
    private bool isSwitch = false; //ビルに移動し終わったかどうか
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float speed = 0.5f;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (isSwitch)
        {
            if (Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) < 1 || transform.position.z > target.position.z +0.5f) Destroy(transform.parent.gameObject);
        }
        else
        {
            if (Fade.IsFadeOutOrIn()&&Fade.IsFadeEnd()) //ヘリの移動が終わり、フェードが終わったら
            {
                isSwitch = true;
                transform.position = target.position + target.forward * distance;
                Vector3 angle = target.eulerAngles;
                angle.y += 180;
                transform.eulerAngles = angle;
                GetComponent<FollowCamera>().enabled = true;
                transform.Find("setup_beacongun").gameObject.SetActive(false);
                GetComponent<AudioListener>().enabled = false;
                target.gameObject.SetActive(true);
                Fade.FadeIn();
            }
        }
	}
    

}
