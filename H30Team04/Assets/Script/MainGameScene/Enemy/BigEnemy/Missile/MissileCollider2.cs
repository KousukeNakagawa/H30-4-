using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider2 : MonoBehaviour
{
    [SerializeField] private Transform explosionPos;
    public GameObject explosion;
    public GameObject explosionEX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BreakTower"))
        {
            TowerBreak.BreakAction();
            Vector3 exPos = explosionPos.position;
            if (exPos == Vector3.zero) exPos = transform.position;
            GameObject ex = Instantiate(explosion, exPos, Quaternion.identity);
            ex.transform.localScale *= 7.5f;
            Destroy(ex, 3.0f);
            if (explosionEX != null)
            {
                GameObject exex = Instantiate(explosionEX, exPos, Quaternion.identity);
                Destroy(exex, 3.0f);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
