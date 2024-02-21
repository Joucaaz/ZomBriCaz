using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] public AudioSource musicSource, effectsSource;
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

    public void PlaySound(AudioClip clip){
        effectsSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip){
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic(){
        musicSource.Stop();
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
