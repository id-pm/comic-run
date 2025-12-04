using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup sfx_mixer;
    [SerializeField] private AudioClip[] music_playlist;
    [SerializeField] private AudioSource playlist_source;
    [SerializeField] private Sound[] sounds;
    private static AudioManager am;
    private void Awake() {
        if(am == null) {
            am = this;
        }else {
            Destroy(this);
            Debug.LogError("There is another AudioManager...");
            Debug.Break();
        }

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        playlist_source.loop = false;
        playlist_source.clip = music_playlist[0];
        playlist_source.Play();
    }

    private void Update() {
        if(!playlist_source.isPlaying) {
            playlist_source.clip = GetRandomCip();
            playlist_source.Play();
        }
    }

    private AudioClip GetRandomCip()
    {
        AudioClip music = null;
        music = music_playlist[UnityEngine.Random.Range(0, music_playlist.Length)];
        
        while(music == playlist_source.clip)
        {
            music = music_playlist[UnityEngine.Random.Range(0, music_playlist.Length)];
        }
        return music;
    }

    public static void PlaySound(string sound_name) {
        Sound s = Array.Find(am.sounds, x => x.name == sound_name);
        if(s == null) {
            Debug.LogWarning($"Sound {sound_name} is not found!");
            return;
        }
        s.source.outputAudioMixerGroup = am.sfx_mixer;
        s.source.Play();
    } 

    public static void PlaySound(string sound_name, AudioSource source) {
        Sound s = Array.Find(am.sounds, x => x.name == sound_name);
        if(s == null) {
            Debug.LogWarning($"Sound {sound_name} is not found!");
            return;
        }
        //s.source.Play();
        source.outputAudioMixerGroup = am.sfx_mixer;
        source.clip = s.clip;
        source.volume = s.volume;
        source.pitch = s.pitch;
        source.Play();
    } 

    public static void PlaySound(int id, AudioSource source, float vol) {
        source.clip = am.sounds[id].clip;
        if(vol == -1f) {
            source.volume = am.sounds[id].volume;
        }else {
            source.volume = vol;
        }
        source.outputAudioMixerGroup = am.sfx_mixer;
        source.pitch = am.sounds[id].pitch;
        source.Play();
        //am.sounds[id].source.Play();
    } 
}

[Serializable] public class Sound{
    public string name;
    public AudioClip clip;
    [Range(0f,2f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
}
