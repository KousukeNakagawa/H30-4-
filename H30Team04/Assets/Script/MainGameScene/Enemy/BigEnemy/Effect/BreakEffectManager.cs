using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffectManager : MonoBehaviour
{
    [Tooltip("光のエフェクト")]public ParticleSystem breakLight;
    [Tooltip("爆発するエフェクト")]public ParticleSystem explosionEffect;
    [Tooltip("爆発するエフェクト")]public GameObject explosionPrefab;
    [Tooltip("光が消える間隔")]public float lightEndTime = 0.15f;
    [Tooltip("ランダムで爆発を行う感覚")]public float randomExplosionTime = 1.0f;
    [Tooltip("1発で出る最大の爆発数")]public int explosionCount = 5;
    [Tooltip("エフェクトを遅延させる時間")] public float actionDelay = 0.5f;

    private float countTime;
    private List<GameObject> explosions = new List<GameObject>();
    //破壊エフェクトが全て消えたか
    [HideInInspector] public bool isEnd = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime == 0) return;
        if (countTime < Time.time)
        {
            effectType++;
            Change();
        }
    }

    public void ChangeType()
    {
        Invoke("Change", actionDelay);
    }

    private void Change()
    {
        switch (effectType)
        {
            case BreakEffectType.RandomExplosion:
                StartCoroutine(RandomExplosion());
                countTime = Time.time + randomExplosionTime;
                break;
            case BreakEffectType.Light:
                ExplosionEnd();
                Time.timeScale = 0.35f;
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
            int count = Random.Range(5, 10);
            for (int i = 0; i < count; i++)
            {
                GameObject ex = Instantiate(explosionPrefab, transform.position + new Vector3(Random.Range(-3.0f, -6.0f),
                    Random.Range(-8.0f, 8.0f), Random.Range(-8.0f, 8.0f)), Quaternion.identity, transform);
                explosions.Add(ex);
                BigEnemyScripts.bigEnemyAudioManager.Play(BigEnemyAudioType.Explosion,ex.transform);
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

        yield return null;

        List<ParticleSystem> particles = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        Time.timeScale = 0.35f;
        while (!isEnd)
        {
            if (particles.Find(f => f == null || f.particleCount != 0) == null) isEnd = true;
            yield return null;
        }
    }
}
