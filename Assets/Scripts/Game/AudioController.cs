using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource gameAudio;

    private void Awake()
    {
        gameAudio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Player.DeathEvent += StopMusic;
    }

    private void StopMusic()
    {
        gameAudio.Stop();
    }

    private void OnDisable()
    {
        Player.DeathEvent -= StopMusic;
    }
}
