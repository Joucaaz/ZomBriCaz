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

    void Start()
    {
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
        }
        else
        {
            SoundManager.Instance.effectsSource.volume = effectsSlider.value;
            SoundManager.Instance.reloadSource.volume = effectsSlider.value;
        }
    }
    
    void AdjustMusicVolume(float volume)
    {
        SoundManager.Instance.musicSource.volume = volume;
    }

    // Fonction pour ajuster le volume des effets sonores
    void AdjustEffectsVolume(float volume)
    {
        SoundManager.Instance.effectsSource.volume = volume;
        SoundManager.Instance.reloadSource.volume = volume;
    }

    public void resetSound(){
        SoundManager.Instance.musicSource.volume = 1;
        SoundManager.Instance.effectsSource.volume = 1;
        SoundManager.Instance.reloadSource.volume = 1;
    }
}
