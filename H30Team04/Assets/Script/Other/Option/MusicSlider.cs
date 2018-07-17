using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{

    // Use this for initialization

    public UnityEngine.Audio.AudioMixer mixer;




    public void masterVol(Slider slider)
    {
        mixer.SetFloat("Master", slider.value);
    }

    public void BGMVol(Slider slider1)
    {
        mixer.SetFloat("BGM", slider1.value);
    }
    public void SEVol(Slider slider2)
    {
        mixer.SetFloat("SE", slider2.value);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}