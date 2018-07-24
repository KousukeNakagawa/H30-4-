using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour {

    private ParticleSystem m_particle;

    private bool isStart = false;

    void Start()
    {
        m_particle = GetComponent<ParticleSystem>();
    }

    void Update () {
        if (m_particle.particleCount > 0) isStart = true;
		if (m_particle.particleCount <= 0 && isStart)
        {  //再生が終わったら消す
            Destroy(gameObject);
        }
	}
}
