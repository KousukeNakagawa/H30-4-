using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyEffectManager : MonoBehaviour {

    [Tooltip("走る際のエフェクト")] public ParticleSystem runEffect;
    [Header("右、左の順番で入れる")]
    [Tooltip("巨大ロボの足")]
    public GameObject[] feets;
    private Direction previous;

    void Update()
    {
        if (Time.timeScale == 0) return;
        Direction dir = TutorialEnemyScripts.bigEnemyEffect.direction_;
        if (dir != previous && dir != Direction.None)
        {
            StartCoroutine(RunEffectCreate(dir));
            TutorialEnemyScripts.bigEnemyEffect.direction_ = Direction.None;
            //print((int)dir);
            TutorialEnemyScripts.bigEnemyAudioManager.Play(BigEnemyAudioType.Step, feets[(int)dir].transform);
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
