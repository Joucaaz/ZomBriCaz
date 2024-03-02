using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] public AudioSource musicSource, effectsSource, reloadSource, zombieSound;
    // Start is called before the first frame update
    private bool isFading;
    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start(){
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", SoundManager.Instance.musicSource.volume);
        effectsSource.volume = PlayerPrefs.GetFloat("EffectsVolume", SoundManager.Instance.effectsSource.volume);
        reloadSource.volume = PlayerPrefs.GetFloat("EffectsVolume", SoundManager.Instance.effectsSource.volume);
    }

    public void PlaySound(AudioClip clip){
        effectsSource.PlayOneShot(clip);
    }
    public void StopSound(){
        effectsSource.Stop();
    }
    public void PlayMusic(AudioClip clip){
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic(){
        musicSource.Stop();
    }

    public void PlayReloadSound(AudioClip clip, float pitch){
        reloadSource.clip = clip;
        reloadSource.pitch = pitch;
        reloadSource.loop = false;
        reloadSource.Play();
    }
    public void StopReloadSound(){
        reloadSource.Stop();
    }

    public void PlayAmbiance(AudioClip ambiance, float fadeDuration = 1.0f)
    {
        StartCoroutine(FadeOutAndIn(ambiance, fadeDuration));
    }

    private IEnumerator FadeOutAndIn(AudioClip newAmbiance, float fadeDuration)
    {
        if (isFading) yield break; // Évitez de lancer plusieurs fondu simultanément
        isFading = true;

        float timer = 0f;
        float startVolume = musicSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;

        musicSource.clip = newAmbiance;
        musicSource.loop = true;
        musicSource.Play();

        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
        isFading = false;
    }
   
}
