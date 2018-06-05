using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyEffectManager : MonoBehaviour
{
    [Tooltip("走る際のエフェクト")] public ParticleSystem runEffect;
    public GameObject[] feets;
    private Direction previous;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        Direction dir = BigEnemyScripts.bigEnemyEffect.direction_;
        if (dir != previous && dir != Direction.None)
        {
            StartCoroutine(RunEffectCreate(dir));
            BigEnemyScripts.bigEnemyEffect.direction_ = Direction.None;
        }
        previous = dir;
    }

    IEnumerator RunEffectCreate(Direction dir)
    {
        GameObject effect = Instantiate(runEffect.gameObject, feets[(int)dir].transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f), feets[(int)dir].transform);
        yield return null;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.parent = null;
        effect.transform.localScale = Vector3.one;
    }
}
