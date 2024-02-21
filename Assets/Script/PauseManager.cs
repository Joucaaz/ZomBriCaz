using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private PlayerInput defaultPlayerActions;
    public GameObject pauseFirstSelected, keyboardFirstSelected, controllerFirstSelected;
    public TextMeshProUGUI zombieKilled;
    public TextMeshProUGUI waveNumber;
    public TextMeshProUGUI duration;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseSettingsKeyboard;
    [SerializeField] GameObject pauseSettingsController;
    [SerializeField] GameObject UICameraCursor;
    [SerializeField] GameObject UICamera;
    [SerializeField] Camera cameraFlou;
    public ChargerUI chargerUI;
    public ChargerUI canvaChargerUI;
    bool isPaused;

    public bool isGamePaused;
    public bool KMPaused = false;
    public bool ControllerPaused = false;
    public bool gamepad = true;
    public bool boolShowSkins = false;
    

    private void Awake(){
        defaultPlayerActions = GetComponent<PlayerInput>();
        chargerUI = canvaChargerUI.GetComponent<ChargerUI>();
        // defaultPlayerActions = new ActionZombieBriCazV2();
        // defaultPlayerActions.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.PauseInput && !boolShowSkins) //defaultPlayerActions.actions["Pause"].triggered || defaultPlayerActions.actions["Cancel"].triggered
        {
            Debug.Log("testpause");
            loadPause();
        }
        else if(UserInput.instance.CancelInput && isGamePaused){
            Debug.Log("testpausecancel");
            loadPause();
        }
        // if(Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel")){
        //     loadPause();
        // }
        zombieKilled.text = chargerUI.zombieSpawner.nbKills.ToString();
        waveNumber.text = chargerUI.zombieSpawner.numberOfWave.ToString();
        duration.text = chargerUI.timerText.text;
        
        if(isGamePaused && !KMPaused && !ControllerPaused){
            if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !gamepad){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gamepad = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
            }
            else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gamepad = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else if(KMPaused){
                if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(keyboardFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        else if(ControllerPaused){
                if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(controllerFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

    }

    
    public void loadPause(){    
            gamepad = false;
            KMPaused = false;
            ControllerPaused = false;
            isPaused = !isPaused;

            // Debug.Log(isPaused);
            
            // isPaused ? defaultPlayerActions.Disable() : defaultPlayerActions.Enable();
            Time.timeScale = isPaused ? 0 : 1;


		    pauseMenu.SetActive(isPaused);
            UICameraCursor.SetActive(!isPaused);
            UICamera.SetActive(!isPaused);
            PostProcessVolume ppVolume = cameraFlou.GetComponent<PostProcessVolume>();
            if(pauseMenu.activeSelf){
                isGamePaused = true;
                ppVolume.enabled = true;
                
            }
            else{
                isGamePaused = false;
                ppVolume.enabled = false;
                
            }
            pauseSettingsKeyboard.SetActive(false);
            pauseSettingsController.SetActive(false);
    }

    public void loadSettingsKeyboard(){
        gamepad = false;
        KMPaused = true;
        pauseMenu.SetActive(!isPaused);
        pauseSettingsKeyboard.SetActive(isPaused);
        isPaused = !isPaused;
        
        if(UserInput.instance.CancelInput && pauseSettingsKeyboard.activeSelf){
            loadPause();
        }
    }
     public void loadSettingsController(){
        gamepad = false;
        ControllerPaused = true;
        pauseMenu.SetActive(!isPaused);
        pauseSettingsController.SetActive(isPaused);
        isPaused = !isPaused;
        
        if(UserInput.instance.CancelInput && pauseSettingsController.activeSelf){
            loadPause();
        }
    }
    public void returnHome(){
        GameManager.Instance.loadHome();
    }


}
