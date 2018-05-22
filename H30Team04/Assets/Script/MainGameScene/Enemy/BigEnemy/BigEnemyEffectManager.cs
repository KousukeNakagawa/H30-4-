using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyEffectManager : MonoBehaviour
{
    [Tooltip("走っている際のエフェクト")]public ParticleSystem runEffect;
    [Tooltip("回転している際のエフェクト")]public ParticleSystem turnEffect;

    // Use this for initialization
    void Start()
    {
        EnabledChange(runEffect, true);
        EnabledChange(turnEffect);
    }


    public void ChangeEffect(bool isTurn)
    {
        EnabledChange(runEffect, !isTurn);
        EnabledChange(turnEffect, isTurn);
    }

    private void EnabledChange(ParticleSystem particle, bool? isEnable = null)
    {
        var emission = particle.emission;
        bool isE = (isEnable == null) ? !emission.enabled : isEnable.Value;
        emission.enabled = isE;
    }
}
