using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_zk : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] float fireTime = 10;
    float FireTime;

    void Start()
    {
        FireTime = fireTime;
    }

    void Update()
    {
        MissileFire();
    }

    void MissileFire()
    {
        missile.transform.position = transform.position;
        missile.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);

        if (fireTime >= 0) fireTime -= Time.deltaTime;
        else
        {
            fireTime = FireTime;
            Instantiate(missile);
            missile.GetComponent<Missile_zk>().Fire();
        }
    }
}
