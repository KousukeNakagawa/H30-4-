using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoket : MonoBehaviour {

    [Header("弾速"), SerializeField] private float bulletSpeed = 1.0f;
    [Header("加速度"), SerializeField, Range(1.0f, 1.5f)] private float bulletAccel = 1.0f;
    //private GameObject target;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        bulletSpeed *= bulletAccel;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "BigEnemy")
        {
            transform.parent.GetComponent<TutorialAttackPlayer>().Damege();
            bulletSpeed = 2.0f;
            gameObject.SetActive(false);

        }
    }

}
