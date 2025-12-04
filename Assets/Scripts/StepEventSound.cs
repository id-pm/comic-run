using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class StepEventSound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private float defalut_volume = -1;
    private void Start() {
        source = GetComponent<AudioSource>();    
    }

    [SerializeField] private int step_sound_id_min=3, step_sound_id_max=8;
    public void StepSound() {
        int step_id = Random.Range(step_sound_id_min, step_sound_id_max);
        AudioManager.PlaySound(step_id, source, defalut_volume);
    }
}
