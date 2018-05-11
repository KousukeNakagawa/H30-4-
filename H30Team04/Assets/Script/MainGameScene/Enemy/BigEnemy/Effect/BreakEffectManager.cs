using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffectManager : MonoBehaviour
{

    public ParticleSystem breakLight;
    public ParticleSystem explosionEffect;
    public GameObject explosionPrefab;
    public float lightEndTime = 0.15f;
    public float randomExplosionTime = 1.0f;
    public int explosionCount = 5;

    private float countTime;
    private List<GameObject> explosions = new List<GameObject>();

    private enum BreakEffectType
    {
        RandomExplosion,
        Light,
        Explosion,
    }

    private BreakEffectType effectType;

    // Use this for initialization
    void Start()
    {
        //テスト
        ChangeType();
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime == 0) return;
        if (countTime < Time.time)
        {
            effectType++;
            ChangeType();
        }
    }

    private void ChangeType()
    {
        switch (effectType)
        {
            case BreakEffectType.RandomExplosion:
                StartCoroutine(RandomExplosion());
                countTime = Time.time + randomExplosionTime;
                break;
            case BreakEffectType.Light:
                ExplosionEnd();
                Time.timeScale = 0.5f;
                countTime = Time.time + lightEndTime;
                breakLight.gameObject.SetActive(true);
                break;
            case BreakEffectType.Explosion:
                Time.timeScale = 0.25f;
                StartCoroutine(BreakLightFadeOut(breakLight));
                explosionEffect.gameObject.SetActive(true);
                countTime = 0;
                break;
        }
    }

    private void ExplosionEnd()
    {
        foreach (var item in explosions)
        {
            item.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(item, 0.5f);
        }
    }

    IEnumerator RandomExplosion()
    {
        float loopTime = Time.time + randomExplosionTime;
        while (loopTime > Time.time)
        {
            int count = Random.Range(1, 5);
            for (int i = 0; i < count; i++)
            {
                explosions.Add(Instantiate(explosionPrefab, transform.position + new Vector3(Random.Range(-3.0f,-6.0f),
                    Random.Range(-7, 7), Random.Range(-7, 7)), Quaternion.identity));
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }

    IEnumerator BreakLightFadeOut(ParticleSystem pa)
    {
        ParticleSystem.TrailModule trail = pa.trails;
        ParticleSystem.MinMaxCurve minmax = trail.widthOverTrail;

        while (minmax.constant > 0)
        {
            minmax.constant -= Time.deltaTime * 4;
            trail.widthOverTrail = minmax;
            yield return null;
        }
        Destroy(pa);
    }
}
