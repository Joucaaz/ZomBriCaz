using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public GameObject homeFirstSelected, creditsFirstSelected, bestFirstSelected, onlineRankingsFirstSelected, settingsFirstSelected;
    public AudioSource musicSource;
    public AudioClip ambianceClip;
    public AudioClip backgroundMusic;
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    public bool isHomeMenu = true;
    public bool isCreditMenu = false;
    public bool isRankings = false;
    public bool isSettings = false;
    public bool logMenu = false;
    public bool isBestScoreMenu = false;
    public bool gamepad = true;

    public Canvas canvaConnected;
    public TMP_InputField inputPseudo; 
    string playerName;
    private bool findInBDD = false;
    void Awake()
    {
        if(instance == null){
            // PlayerPrefs.DeleteKey("PlayerID");
            
            instance = this;
            playerConnected();
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        
       
    }
    void Start(){
        Time.timeScale = 1;
        
        
        if(backgroundMusic != null && !SoundManager.Instance.musicSource.isPlaying){
                SoundManager.Instance.PlayMusic(backgroundMusic);
        }
        
        // if(backgroundMusic != null){
        //     SoundManager.Instance.PlayMusic(backgroundMusic);
        // }
        // if (musicSource != null && backgroundMusic != null)
        // {
        //     musicSource.clip = backgroundMusic;
        //     musicSource.loop = true;
        //     musicSource.Play();
        // }
        // resetBestScore();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gamepad = false;
        if(scene.name == "GameScene"){
            isHomeMenu = false;
            isBestScoreMenu = false;
            isCreditMenu = false;
            isRankings = false;
            isSettings = false;
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlayMusic(ambianceClip);
            GameObject.Find("CanvasVideo").GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            SoundManager.Instance.StopMusic();
            GameObject.Find("CanvasVideo").GetComponent<CanvasGroup>().alpha = 1;
            if(backgroundMusic != null && !SoundManager.Instance.musicSource.isPlaying){
                SoundManager.Instance.PlayMusic(backgroundMusic);
            }
            
            // if (videoBackground != null)
            // {
            //     videoBackground.Play(); // Reprend la lecture de la vidéo lors du chargement d'une nouvelle scène
            // }
            // if (backgroundMusic != null && !musicSource.isPlaying)
            // {
            //     musicSource.clip = backgroundMusic;
            //     musicSource.loop = true;
            //     musicSource.Play();
            // }
            
        }
        if (scene.name == "HomeMenu")
            {
                isHomeMenu = true;
                isBestScoreMenu = false;
                isCreditMenu = false;
                isRankings = false;
                isSettings = false;
                homeFirstSelected = GameObject.Find("HomeFirstSelected"); 
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(homeFirstSelected);
                loadingScreen = GameObject.Find("LoaderScreen"); 
                slider = GameObject.Find("SliderScreen").GetComponent<Slider>(); 
                progressText = GameObject.Find("SliderValueScreen").GetComponent<TextMeshProUGUI>(); 
            }
            else if (scene.name == "Credits")
            {
                isHomeMenu = false;
                isBestScoreMenu = false;
                isCreditMenu = true;
                isRankings = false;
                isSettings = false;
                creditsFirstSelected = GameObject.Find("CreditsFirstSelected"); 
                // EventSystem.current.SetSelectedGameObject(null);
                // EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
            }
            else if (scene.name == "BestScore")
            {
                isHomeMenu = false;
                isBestScoreMenu = true;
                isCreditMenu = false;
                isRankings = false;
                isSettings = false;
                bestFirstSelected = GameObject.Find("BestFirstSelected"); 
                // EventSystem.current.SetSelectedGameObject(null);
                // EventSystem.current.SetSelectedGameObject(bestFirstSelected);
            }
            else if (scene.name == "OnlineRankings")
            {
                isHomeMenu = false;
                isBestScoreMenu = false;
                isCreditMenu = false;
                isRankings = true;
                isSettings = false;
                onlineRankingsFirstSelected = GameObject.Find("RankingsFirstSelected"); 
                // EventSystem.current.SetSelectedGameObject(null);
                // EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
            }
            else{
                isHomeMenu = false;
                isBestScoreMenu = false;
                isCreditMenu = false;
                isRankings = false;
                isSettings = true;
            }
            // else if (scene.name == "Settings")
            // {
            //     isHomeMenu = false;
            //     isBestScoreMenu = false;
            //     isCreditMenu = false;
            //     isRankings = false;
            //     isSettings = true;
            //     settingsFirstSelected = GameObject.Find("SettingsFirstSelected"); 
            //     // EventSystem.current.SetSelectedGameObject(null);
            //     // EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
            // }

    }
    

    void Update()
    {
        

    }

   
    private void resetBestScore(){
        
        string jsonData = PlayerPrefs.GetString("HighScores", "{}");
        HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(jsonData);

        //  Debug.Log("Avant réinitialisation : " + JsonUtility.ToJson(highScoreData));

        HighScoreData highScoreData1= new HighScoreData{
            bestScore1 = new GameData{ time = "00:00", kills = 0, waves = 0 },
            bestScore2 = new GameData{ time = "00:00", kills = 0, waves = 0 },
            bestScore3 = new GameData{ time = "00:00", kills = 0, waves = 2 },
        };
        PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(highScoreData1));
        PlayerPrefs.Save();


    }

    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }

    public void loadGameScreen(){
        StartCoroutine(LoadAsync(1));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.GetComponent<CanvasGroup>().alpha = 1;
        // loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            int progressPercentage = Mathf.FloorToInt(progress * 100);
            progressText.text = progressPercentage.ToString() + "%";

            yield return null;
        }
    }

    public void loadGame(){
        
        SceneManager.LoadScene(1);
    }
    public void loadHome(){
        
        SceneManager.LoadScene(0);
        if(loadingScreen != null){
            loadingScreen.GetComponent<CanvasGroup>().alpha = 0;
        }
        
        Time.timeScale = 1;
    }
    public void loadCredits(){
        
        SceneManager.LoadScene(3);
        
    }
    public void loadOnlineRankings(){
    
        SceneManager.LoadScene(4);
        
    }
    public void loadSettings(){
    
        SceneManager.LoadScene(5);
        
    }
    public void loadScores(){
        
        SceneManager.LoadScene(2);
        
    }
    public void quit(){
        Application.Quit();
    }

    public void playSoundMenu(AudioClip clip){
        SoundManager.Instance.PlaySound(clip);
    }

    public void playerConnected(){
        if (PlayerPrefs.HasKey("PlayerID") && !string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerID")))
        {
            Debug.Log("player id exist");
            StartCoroutine(VerifyPlayerInBDD());
            

        }
        else
        {
            AskPlayerName();
            Debug.Log("player id no exist");
        
        }
    }

    public void AskPlayerName(){
        logMenu = true;
        canvaConnected.gameObject.SetActive(true);
    }

    public void AddUserToBDD(){
        if(!string.IsNullOrWhiteSpace(inputPseudo.text))//inputPseudo.text != ""
        {
            logMenu = false;
            canvaConnected.gameObject.SetActive(false);
            StartCoroutine(SendUserInfoToBDD("https://joudcazeaux.fr/ZomBriCaz/zombricazAddUser.php", inputPseudo.text));
        }
        
    }
    
    IEnumerator VerifyPlayerInBDD(){
        Debug.Log("Player Name: " + PlayerPrefs.GetString("PlayerName") + "Player ID : " + PlayerPrefs.GetString("PlayerID"));
        yield return StartCoroutine(VerifyUserIDExistInBDD("https://joudcazeaux.fr/ZomBriCaz/zombricazVerifyUserExist.php", PlayerPrefs.GetString("PlayerID")));
        if(findInBDD)
        {
            Debug.Log("Le PlayerID existe dans la base de données.");
        }
        else
        {
            Debug.Log("Le PlayerID existe pas dans la base de données.");
            AskPlayerName();
        }
    }
    
    IEnumerator VerifyUserIDExistInBDD(string uri, string idUser)
    {
        WWWForm form = new WWWForm();
        form.AddField("idName", idUser);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la requête POST : " + webRequest.error);
                findInBDD = false;
            }
            else
            {
                Debug.Log("Requête POST réussie : " + webRequest.downloadHandler.text);
                if(webRequest.downloadHandler.text.Contains("find")){
                    Debug.Log("true");
                    findInBDD = true;
                }
                else{
                    Debug.Log("false");
                    findInBDD = false;
                }
            }
        }
    }
    IEnumerator SendUserInfoToBDD(string uri, string nameUser)
    {
        WWWForm form = new WWWForm();
        form.AddField("nameUser", nameUser);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la requête POST : " + webRequest.error);
                AskPlayerName();
            }
            else
            {
                Debug.Log("Requête POST réussie : " + webRequest.downloadHandler.text);
                if(webRequest.downloadHandler.text.Contains("Erreur")){
                    AskPlayerName();
                }
                else{
                    StartCoroutine(GetUserInfoFromBDD("https://joudcazeaux.fr/ZomBriCaz/zombricazGetUserInfo.php", nameUser));
                }
                
            }
        }
    }

    IEnumerator GetUserInfoFromBDD(string url, string nameUser)
    {
        WWWForm form = new WWWForm();
        form.AddField("nameUser", nameUser);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string playerID = www.downloadHandler.text;
                if(!string.IsNullOrEmpty(playerID)){
                    PlayerPrefs.SetString("PlayerID", playerID);
                    PlayerPrefs.SetString("PlayerName", nameUser);
                }
                Debug.Log("Player Name: " + PlayerPrefs.GetString("PlayerName") + "Player ID : " + PlayerPrefs.GetString("PlayerID"));
            }
            else
            {
                Debug.LogError("Erreur de requête : " + www.error);
            }
        }
    }


}
