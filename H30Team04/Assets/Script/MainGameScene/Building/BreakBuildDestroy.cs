using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBuildDestroy : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("BreakBuild") && !other.collider.CompareTag("BigEnemy")) Destroy(gameObject, 0.75f);
    }
}
