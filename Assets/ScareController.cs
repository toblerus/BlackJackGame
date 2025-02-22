using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareController : MonoBehaviour
{
    public Camera MainCamera;
    public Camera ScareCamera;

    public List<AudioClip> ScareSounds1;
    public List<AudioClip> ScareSounds2;
    public AudioSource ScareSound1;
    public AudioSource ScareSound2;
    public AudioSource Ambience;

    public void OnEnable()
    {
        MainCamera.enabled = false;
        ScareCamera.enabled = true;

        if (ScareSounds1.Count > 0)
        {
            ScareSound1.clip = ScareSounds1[Random.Range(0, ScareSounds1.Count)];
            ScareSound1.Play();
        }

        if (ScareSounds2.Count > 0)
        {
            ScareSound2.clip = ScareSounds2[Random.Range(0, ScareSounds2.Count)];
            ScareSound2.Play();
        }

    }

    public void OnDisable()
    {
        MainCamera.enabled = true;
        ScareCamera.enabled = false;

        ScareSound1.Stop();
        ScareSound2.Stop();

    }
}
