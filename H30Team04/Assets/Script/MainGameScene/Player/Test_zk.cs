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
        missile.transform.localEulerAngles = new Vector3(270, 0, 180);

        //missile.GetComponent<Missile_zk>().SetRotation(transform.localEulerAngles + new Vector3(1, 1, 1));

        if (fireTime >= 0) fireTime -= Time.deltaTime;
        else
        {
            fireTime = FireTime;
            Instantiate(missile);
            missile.GetComponent<Missile_zk>().Fire();
        }
    }
}
