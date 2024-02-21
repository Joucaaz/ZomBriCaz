using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    private PlayerInput defaultPlayerActions;
    // private ActionZombieBriCazV2 defaultPlayerActions;
    public float mouseSensitivity;
    private float initialMouseSensitivity;
    private float aimMouseSensitivity;
    private float initialControllerSensitivity;
    private float aimControllerSensitivity;
    public float basicSensitivity = 100f;
    public Transform playerBody;
    public PlayerMovement2 player;
    float xRotation = 0f;
    public Slider sliderMouse;
    public Slider sliderMouseAim;
    public TextMeshProUGUI textMouse;
    public TextMeshProUGUI textMouseAIm;

    public Slider sliderController;
    public Slider sliderControllerAim;
    public TextMeshProUGUI textController;
    public TextMeshProUGUI textControllerAIm;
public string lastUsedDevice;
    private const string InitialMouseSensitivityKey = "InitialMouseSensitivity";
    private const string AimMouseSensitivityKey = "AimMouseSensitivity";
    private const string InitialControllerSensitivityKey = "InitialControllerSensitivity";
    private const string AimControllerSensitivityKey = "AimControllerSensitivity";
    
    private void Awake(){
        defaultPlayerActions = GetComponent<PlayerInput>();
        LoadSensitivities(out initialMouseSensitivity, out aimMouseSensitivity, out initialControllerSensitivity, out aimControllerSensitivity);

    }
    void Start(){
        player = playerBody.GetComponent<PlayerMovement2>();
        mouseSensitivity = initialMouseSensitivity;
        lastUsedDevice = "keyboard";
        // initialMouseSensitivity = basicSensitivity;
        // aimMouseSensitivity = initialMouseSensitivity/2;
        // aimControllerSensitivity = initialControllerSensitivity/2;
        initializeAllSlider();
    }

    void Update(){

        // var gamepad = Gamepad.current;
        // if (gamepad != null)
        // {
        //     if (gamepad is DualShockGamepad)
        //     {
        //         print("PlayStation gamepad");
        //     }
        //     else if (gamepad is XInputController)
        //     {
        //         print("Xbox gamepad");
        //     }
        // }
        // else if (Keyboard.current != null || Mouse.current != null)
        // {
        //     print("Mouse and keyboard");
        // }
        
        if(player.isAiming && UserInput.instance.DetectAllUserInputs() == "gamepad" && player.inventory.GetItem(player.primarySecondary) == player.ListWeapons[5]){
            // lastUsedDevice = "gamepad";
            mouseSensitivity = aimControllerSensitivity / 3;
            // Debug.Log("gamepad");
        }
        else if(player.isAiming && UserInput.instance.DetectAllUserInputs() == "gamepad"){
            mouseSensitivity = aimControllerSensitivity;
        }
        else if(UserInput.instance.DetectAllUserInputs() == "gamepad"){
            // lastUsedDevice = "gamepad";
            mouseSensitivity = initialControllerSensitivity;
            // Debug.Log("gamepad");
        }
        else if(player.isAiming && player.isAiming && UserInput.instance.DetectAllUserInputs() == "keyboard" && player.inventory.GetItem(player.primarySecondary) == player.ListWeapons[5]){
            // lastUsedDevice = "keyboard";
            // Debug.Log("keyboard");
            mouseSensitivity = aimMouseSensitivity/3;
        }
        else if(player.isAiming && player.isAiming && UserInput.instance.DetectAllUserInputs() == "keyboard"){
            // lastUsedDevice = "keyboard";
            // Debug.Log("keyboard");
            mouseSensitivity = aimMouseSensitivity;
        }
        else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
            // lastUsedDevice = "keyboard";
            mouseSensitivity = initialMouseSensitivity;
            // Debug.Log("keyboard");
        }
        // Debug.Log(UserInput.instance.DetectAllUserInputs());
        
    }


    void LateUpdate()
    {
        if(!player.pauseManager.boolShowSkins){
            Vector2 mouseDelta = UserInput.instance.LookInput;
            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 

            playerBody.Rotate(Vector3.up * mouseX);
        }
        

    }
    private void initializeAllSlider()
    {
        sliderMouse.value = initialMouseSensitivity;
        sliderMouseAim.value = aimMouseSensitivity;
        sliderController.value = initialControllerSensitivity / 40 ;
        sliderControllerAim.value = aimControllerSensitivity / 40;
    }
    public void changeMouseSensitivity(){
        initialMouseSensitivity = sliderMouse.value;
        SaveSensitivities(initialMouseSensitivity, aimMouseSensitivity, initialControllerSensitivity, aimControllerSensitivity);
        UpdateTextValues();
    }
     public void changeAimMouseSensitivity(){
        aimMouseSensitivity = sliderMouseAim.value;
        SaveSensitivities(initialMouseSensitivity, aimMouseSensitivity, initialControllerSensitivity, aimControllerSensitivity);
        UpdateTextValues();
    }
    public void changeControllerSensitivity(){
        initialControllerSensitivity = sliderController.value * 40;
        SaveSensitivities(initialMouseSensitivity, aimMouseSensitivity, initialControllerSensitivity, aimControllerSensitivity);
        UpdateTextValues();
    }
     public void changeAimControllerSensitivity(){
        aimControllerSensitivity = sliderControllerAim.value * 40;
        SaveSensitivities(initialMouseSensitivity, aimMouseSensitivity, initialControllerSensitivity, aimControllerSensitivity);
        UpdateTextValues();
    }
    void UpdateTextValues()
    {
        // Mettez à jour les textes avec les valeurs actuelles
        textMouse.text = initialMouseSensitivity.ToString("F0");
        textMouseAIm.text = aimMouseSensitivity.ToString("F0");
        float textDiv1 = initialControllerSensitivity / 40;
        float textDiv2 = aimControllerSensitivity / 40;
        textController.text = textDiv1.ToString("F0");
        textControllerAIm.text = textDiv2.ToString("F0");
    }

    private static void SaveSensitivity(string key, float sensitivity)
    {
        PlayerPrefs.SetFloat(key, sensitivity);
        Debug.Log("Saved " + key + ": " + sensitivity);
    }

    private static float LoadSensitivity(string key, float defaultValue = 1.0f)
    {
        float sensitivity = PlayerPrefs.GetFloat(key, defaultValue);
        Debug.Log("Loaded " + key + ": " + sensitivity);
        return sensitivity;
    }

    public static void SaveSensitivities(float initialMouse, float aimMouse, float initialController, float aimController)
    {
        SaveSensitivity(InitialMouseSensitivityKey, initialMouse);
        SaveSensitivity(AimMouseSensitivityKey, aimMouse);
        SaveSensitivity(InitialControllerSensitivityKey, initialController);
        SaveSensitivity(AimControllerSensitivityKey, aimController);

        PlayerPrefs.Save();
    }

    public static void LoadSensitivities(out float initialMouse, out float aimMouse, out float initialController, out float aimController)
    {
        initialMouse = LoadSensitivity(InitialMouseSensitivityKey);
        aimMouse = LoadSensitivity(AimMouseSensitivityKey);
        initialController = LoadSensitivity(InitialControllerSensitivityKey);
        aimController = LoadSensitivity(AimControllerSensitivityKey);
    }

}
