using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRippel : MonoBehaviour {
    [SerializeField, Range(0.1f, 0.5f)] float size = 0.1f;

    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        GetComponent<Renderer>().material.SetColor("_TintColor", Color.blue);
        particle.Stop();
    }

    void Update()
    {
        //if (Time.timeScale == 0)
        //{
        //    particle.Stop();
        //    return;
        //}
        //else particle.Play();
    }

    public void StartEffect()
    {
        particle.Play();
    }

    public void EndEffect()
    {
        particle.Stop();
    }
}
