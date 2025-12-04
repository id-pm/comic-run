using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Light day_light;
    [SerializeField] private Light[] night_lights;
    [SerializeField] private Material day_skybox, night_skybox;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private TMP_Dropdown resolution_dropdown, graphics;
    [SerializeField] private Slider music_volume, sfx_volume;
    public static bool isGeneralMenu = true;
    public static bool isActive = false;
    private Resolution[] resolutions;
    private void Start() {
        resolutions = Screen.resolutions;

        resolution_dropdown.ClearOptions();

        List<string> options = new List<string>();

        int current_res = 0;
        for(int i =0; i< resolutions.Length; i++) {
            options.Add($"{resolutions[i].width}x{resolutions[i].height}");

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                current_res = i;
            }
        }

        resolution_dropdown.AddOptions(options);
        resolution_dropdown.value = current_res;
        resolution_dropdown.RefreshShownValue();

        LoadSettings();
        ChangeNightMode(false);

        gameObject.SetActive(false);
    }

    public void SetResolution(int res_id) {
        Resolution resolution = resolutions[res_id];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    [SerializeField] private AudioMixer audio_mixer;
    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ChangeNightMode(bool night_mode) {
        if(night_mode) {
            RenderSettings.ambientIntensity = 0.3f;
            RenderSettings.skybox = night_skybox;
            day_light.enabled = false;
            for(int i = 0; i < night_lights.Length; i++) {
                night_lights[i].enabled = true;
            }
        }else {
            RenderSettings.ambientIntensity = 1;
            RenderSettings.skybox = day_skybox;
            day_light.enabled = true;
            for(int i = 0; i < night_lights.Length; i++) {
                night_lights[i].enabled = false;
            }
        }
    }

    public void ChangeMusicVolume(float volume) {
        Debug.Log(volume);
        audio_mixer.SetFloat("Music", volume);
    }
    public void ChangeSFXVolume(float volume) {
        audio_mixer.SetFloat("SFX", volume);
        Debug.Log(volume);
    }
    public void LoadSettings() {
        string json = PlayerPrefs.GetString("settings");
        if(json == string.Empty) {
            Debug.Log("json empty");
            return;
        }
        Settings settings = JsonUtility.FromJson<Settings>(json);
        
        fullscreen.isOn = settings.fullScreen;
        resolution_dropdown.value = settings.resolution;
        graphics.value = settings.graphics;
        music_volume.value = settings.music_volume;
        sfx_volume.value = settings.sfx_volume;

    }
    public void SaveSettings() {
        Settings settings = new Settings(fullscreen.isOn, resolution_dropdown.value, graphics.value, music_volume.value, sfx_volume.value);
        PlayerPrefs.SetString("settings", JsonUtility.ToJson(settings));
        UIController.CloseSettings(isGeneralMenu);
    }
    public void SetMenu(bool isGeneral)
    {
        isGeneralMenu = isGeneral;
    }
    [Serializable] public class Settings{
        public bool fullScreen;
        public int resolution, graphics;
        public float music_volume, sfx_volume;
        public Settings(bool full, int res, int graphics, float music, float sfx) {
            fullScreen = full;
            resolution = res;
            this.graphics = graphics;
            music_volume = music;
            sfx_volume = sfx;
        }
    }
}
