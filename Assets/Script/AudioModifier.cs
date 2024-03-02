using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioModifier : MonoBehaviour
{

    public Slider musicSlider;
    public Slider effectsSlider;
    public Toggle toggleMusic;
    public Toggle toggleEffects;
    public TextMeshProUGUI sliderTextMusic;
    public TextMeshProUGUI sliderTextEffects;
    private AudioSource sourceZombie;
    public AudioSource[] sourceZombieAll;
    public float volumeTemp = 1f;
    private bool zombieTemp = false;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", SoundManager.Instance.musicSource.volume);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", SoundManager.Instance.effectsSource.volume);

        musicSlider.value = SoundManager.Instance.musicSource.volume;
        effectsSlider.value = SoundManager.Instance.effectsSource.volume;

        toggleMusic.onValueChanged.AddListener(ToggleMusic);
        toggleEffects.onValueChanged.AddListener(ToggleEffects);

        // Assigner les fonctions aux événements des sliders
        musicSlider.onValueChanged.AddListener(AdjustMusicVolume);
        effectsSlider.onValueChanged.AddListener(AdjustEffectsVolume);
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderTextMusic.text = musicSlider.value.ToString("F1");
        sliderTextEffects.text = effectsSlider.value.ToString("F1");
    }

    private void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.musicSource.volume = 0f;
        }
        else
        {
            SoundManager.Instance.musicSource.volume = musicSlider.value;
        }
    }
    private void ToggleEffects(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.effectsSource.volume = 0f;
            SoundManager.Instance.reloadSource.volume = 0f; 
            volumeTemp = 0;
        }
        else
        {
            SoundManager.Instance.effectsSource.volume = effectsSlider.value;
            SoundManager.Instance.reloadSource.volume = effectsSlider.value;
            volumeTemp = effectsSlider.value;
        }
    }
    
    void AdjustMusicVolume(float volume)
    {
        SoundManager.Instance.musicSource.volume = volume;
        musicSlider.value = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
    }

    // Fonction pour ajuster le volume des effets sonores
    void AdjustEffectsVolume(float volume)
    {
        SoundManager.Instance.effectsSource.volume = volume;
        SoundManager.Instance.reloadSource.volume = volume;
        effectsSlider.value = volume;
        volumeTemp = volume;
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        PlayerPrefs.Save();
    }

    public void resetSound(){
        SoundManager.Instance.musicSource.volume = 1;
        SoundManager.Instance.effectsSource.volume = 1;
        SoundManager.Instance.reloadSource.volume = 1;
        PlayerPrefs.SetFloat("MusicVolume", 1f);
        PlayerPrefs.SetFloat("EffectsVolume", 1f);
        PlayerPrefs.Save();

        volumeTemp = 1;
    }
}
