using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRipple : MonoBehaviour {

    [SerializeField, Range(0.1f, 0.5f)] float size = 0.1f;

    ParticleSystem particle;
    ParticleSystemRenderer particleRenderer;
    Material material;
    Color color;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        particle = GetComponent<ParticleSystem>();
        particleRenderer = GetComponent<ParticleSystemRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            particle.Stop();
            return;
        }
        else particle.Play();

        ColorChange();
        SizeChange();
    }

    /// <summary> 装備中の武器によって色を変える </summary>
    void ColorChange()
    {
        color = (TutorialWepon.WeaponBeacon) ? Color.blue : Color.red;
        material.SetColor("_TintColor", color);
    }

    /// <summary> 地面を映すとき小さくなる </summary>
    void SizeChange()
    {
        var size = (TutorialWepon.IsFloorHit) ? this.size : 1;
        particleRenderer.maxParticleSize = size;
    }
}
