using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;   // if you’re using TextMeshPro for the dropdown

public class OptionsMenu : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [Header("Audio")]
    public AudioMixer mixer; 

    Resolution[] resolutions;

    void Awake()
    {
        // Subscribe to slider changes in code,
        // which automatically passes the current slider.value.
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        
    }

    void OnDestroy()
    {
        resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
        masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }

    void Start()
    {
        // 1) Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var res = resolutions[i];
            string option = $"{res.width} x {res.height} @ {res.refreshRateRatio}Hz";
            options.Add(option);

            if (res.width == Screen.currentResolution.width
             && res.height == Screen.currentResolution.height
             && res.refreshRate == Screen.currentResolution.refreshRate)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        float val;

        mixer.GetFloat("Master", out val);
        masterSlider.SetValueWithoutNotify(Mathf.InverseLerp(-80f, 20f, val));

        mixer.GetFloat("SFX", out val);
        sfxSlider.SetValueWithoutNotify(   Mathf.InverseLerp(-80f, 20f, val));

        mixer.GetFloat("Music", out val);
        musicSlider.SetValueWithoutNotify(Mathf.InverseLerp(-80f, 20f, val));
    }

    // Called by the UI Dropdown OnValueChanged
    public void SetResolution(int index)
    {
        var res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRateRatio);
        Debug.Log($"Resolution set to: {res.width}x{res.height} @ {res.refreshRateRatio}Hz");
    }

    // Called by UI Sliders OnValueChanged (range 0–1)
    public void SetMasterVolume(float sliderValue)
        => mixer.SetFloat("Master", Mathf.Lerp(-80f, 20f, sliderValue));

    public void SetSfxVolume(float sliderValue)
        => mixer.SetFloat("SFX", Mathf.Lerp(-80f, 20f, sliderValue));

    public void SetMusicVolume(float sliderValue)
        => mixer.SetFloat("Music", Mathf.Lerp(-80f, 20f, sliderValue));

    // Optionally toggle fullscreen
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}