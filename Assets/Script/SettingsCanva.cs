using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SettingsCanva : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject settingsKeyboard;
    [SerializeField] GameObject settingsController;
    [SerializeField] GameObject settingsChangeName;
    [SerializeField] GameObject settingsSounds;

    public GameObject settingsFirstSelected, keyboardSettingsFirstSelected, controllerSettingsFirstSelected, changeNameSettingsFirstSelected, SoundsSettingsFirstSelected;
    public TMP_InputField inputPseudo; 
    // public bool gamepad = true;GameManager.Instance.
    public TextMeshProUGUI textLastUsername;
    private string playerLastUsername;

    public bool isSettingsMenu = true, KMSettings = false, ControllerSettings = false, ChangeNameSettings = false, SoundSettings = false;

    void Start(){
        StartCoroutine(GetPlayerName("https://joudcazeaux.fr/ZomBriCaz/zombricazGetUser.php")); 
    }
    void LateUpdate(){
        
        if(UserInput.instance.CancelInput && settingsMenu.activeSelf){
            // returnHome();
            GameManager.Instance.loadHome();
        }
        else if((UserInput.instance.CancelInput && settingsKeyboard.activeSelf) || (UserInput.instance.CancelInput && settingsController.activeSelf) || (UserInput.instance.CancelInput && settingsChangeName.activeSelf) || (UserInput.instance.CancelInput && settingsSounds.activeSelf)){
            loadSettings();
        }

        if(isSettingsMenu){
            if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameManager.Instance.gamepad = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsFirstSelected);
            }
            else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.Instance.gamepad = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else if(SoundSettings){
                if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManager.Instance.gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(SoundsSettingsFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    GameManager.Instance.gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        else if(KMSettings){
                if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManager.Instance.gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(keyboardSettingsFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    GameManager.Instance.gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        else if(ControllerSettings){
                if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManager.Instance.gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(controllerSettingsFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    GameManager.Instance.gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        else if(ChangeNameSettings){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !gamepad){
            //         Cursor.lockState = CursorLockMode.Locked;
            //         Cursor.visible = false;
            //         gamepad = true;
            //         EventSystem.current.SetSelectedGameObject(null);
            //         EventSystem.current.SetSelectedGameObject(changeNameSettingsFirstSelected);
            //     }
            //     else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
            //         Cursor.lockState = CursorLockMode.None;
            //         Cursor.visible = true;
            //         gamepad = false;
            //         EventSystem.current.SetSelectedGameObject(null);
            //     }
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
    public void loadSettingsKeyboard(){
        GameManager.Instance.gamepad = false;
        isSettingsMenu = false;
        KMSettings = true;
        ControllerSettings = false;
        ChangeNameSettings = false;

        settingsMenu.SetActive(false);
        settingsKeyboard.SetActive(true);
        
        
    }
     public void loadSettingsSounds(){
        GameManager.Instance.gamepad = false;
        isSettingsMenu = false;
        KMSettings = false;
        SoundSettings = true;
        ControllerSettings = false;
        ChangeNameSettings = false;
        settingsMenu.SetActive(false);
        settingsSounds.SetActive(true);
        
        
    }
    public void loadSettingsController(){
        GameManager.Instance.gamepad = false;
        isSettingsMenu = false;
        KMSettings = false;
        ControllerSettings = true;
        ChangeNameSettings = false;
        settingsMenu.SetActive(false);
        settingsController.SetActive(true);
        
        
    }

    public void loadChangeName(){
        
        GameManager.Instance.gamepad = false;
        settingsMenu.SetActive(false);
        settingsChangeName.SetActive(true);
        isSettingsMenu = false;
        KMSettings = false;
        ControllerSettings = false;
        ChangeNameSettings = true;

        
    }

    public void loadSettings()
    {
        StartCoroutine(GetPlayerName("https://joudcazeaux.fr/ZomBriCaz/zombricazGetUser.php")); 
        GameManager.Instance.gamepad = false;
        isSettingsMenu = true;
        KMSettings = false;
        ControllerSettings = false;
        ChangeNameSettings = false;
        // StartCoroutine(GetPlayerName("https://joudcazeaux.fr/ZomBriCaz/zombricazGetUser.php"));
        settingsMenu.SetActive(true);
        settingsController.SetActive(false);
        settingsKeyboard.SetActive(false);
        settingsChangeName.SetActive(false);
        settingsSounds.SetActive(false);
        
    }

    public void returnHome(){
        GameManager.Instance.loadHome();
    }

    public void submitSettingsUpdateUsername(){
        if(!string.IsNullOrWhiteSpace(inputPseudo.text))//inputPseudo.text != ""
        {
            loadSettings();
            StartCoroutine(PostRequestChangeUser("https://joudcazeaux.fr/ZomBriCaz/zombricazChangeUser.php", PlayerPrefs.GetString("PlayerID"), inputPseudo.text));
       
        }
        
    }
    IEnumerator PostRequestChangeUser(string uri, string userId, string nameUser)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("nameUser", nameUser);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                loadChangeName();
                Debug.LogError("Erreur lors de la requête POST : " + webRequest.error);
            }
            else
            {
                Debug.Log("Requête POST réussie : " + webRequest.downloadHandler.text);
                if(webRequest.downloadHandler.text.Contains("Erreur")){
                    loadChangeName();
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
    
    IEnumerator GetPlayerName(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("idUser", PlayerPrefs.GetString("PlayerID")); // Remplacez "your_user_id" par l'idUser que vous souhaitez interroger

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string playerLastUsername = www.downloadHandler.text;
                textLastUsername.text = "Last Username : " + playerLastUsername;
                Debug.Log("Player Name: " + playerLastUsername);
            }
            else
            {
                Debug.LogError("Erreur de requête : " + www.error);
            }
        }
    }
}
