using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcHelicopter : MonoBehaviour {
    
    [SerializeField] private Transform targetPos;
    [SerializeField] private float speed = 2.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (targetPos.position != transform.position)
        {
            Vector3 direction = targetPos.position - transform.position;
            if (direction.sqrMagnitude < 0.1f)
            {
                transform.position = targetPos.position;
                Fade.FadeOut();
                Destroy(gameObject, 2.0f);
                return;
            }
            direction = Vector3.Normalize(direction);
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
