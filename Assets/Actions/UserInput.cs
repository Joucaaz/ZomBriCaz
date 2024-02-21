using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool RunningInput { get; private set; }
    public bool ChangingWeaponInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool FireReleasedInput { get; private set; }
    public bool ReloadInput { get; private set; }
    public float MouseScrollWheelInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool AimingInput { get; private set; }
    public bool CrouchingInput { get; private set; }
    public bool PauseInput { get; private set; }
    public bool CancelInput { get; private set; }
    public bool BuyWeaponInput { get; private set; }
    public bool InspectInput { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _runAction;
    private InputAction _changeWeaponAction;
    private InputAction _fireAction;
    private InputAction _reloadAction;
    private InputAction _mouseScrollWheelAction;
    private InputAction _jumpAction;
    private InputAction _aimingAction;
    private InputAction _crouchingAction;
    private InputAction _pauseAction;
    private InputAction _cancelAction;
    private InputAction _buyWeaponAction;
    private InputAction _inspectAction;
    private string lastDevice = "gamepad";

    private void Awake(){
        if(instance == null){
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
        List<string> keys = GetKeysForAction("Reload");

        if (keys != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Debug.Log("Key for Reload action: " + key);
            }
        }
        else
        {
            Debug.Log("No keys found for Reload action.");
        }
    }

    void Update()
    {
        UpdateInputs();
        
        // Debug.Log(DetectAllUserInputs());
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _lookAction = _playerInput.actions["Look"];
        _runAction = _playerInput.actions["Run"];
        _changeWeaponAction = _playerInput.actions["ChangeWeapon"];
        _fireAction = _playerInput.actions["Fire"];
        _reloadAction = _playerInput.actions["Reload"];
        _mouseScrollWheelAction = _playerInput.actions["MouseScrollWheel"];
        _jumpAction = _playerInput.actions["Jump"];
        _aimingAction = _playerInput.actions["Aiming"];
        _crouchingAction = _playerInput.actions["Crouching"];
        _pauseAction = _playerInput.actions["Pause"];
        _cancelAction = _playerInput.actions["Cancel"];
        _buyWeaponAction = _playerInput.actions["BuyButton"];
        _inspectAction = _playerInput.actions["Inspect"];
    }

    private void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();
        RunningInput = _runAction.IsPressed();
        ChangingWeaponInput = _changeWeaponAction.WasPressedThisFrame();
        FireInput = _fireAction.WasPressedThisFrame();
        FireReleasedInput = _fireAction.IsPressed();
        ReloadInput = _reloadAction.WasPressedThisFrame();
        MouseScrollWheelInput = _mouseScrollWheelAction.ReadValue<float>();
        JumpInput = _jumpAction.WasPressedThisFrame();
        AimingInput = _aimingAction.IsPressed();
        CrouchingInput = _crouchingAction.IsPressed();
        PauseInput = _pauseAction.WasPressedThisFrame();
        CancelInput = _cancelAction.WasPerformedThisFrame();
        BuyWeaponInput = _buyWeaponAction.WasPressedThisFrame();
        InspectInput = _inspectAction.WasPressedThisFrame();
        
    }

public string DetectAllUserInputs()
{
    string lastUsedDevice;

    if(DetectAllKeysPressed() || DetectMouseButtonsPressed()){
        lastDevice = "keyboard";
        lastUsedDevice = lastDevice;
        return lastUsedDevice;
    }
    else if(DetectGamepadButtonsPressed()){
        lastDevice = "gamepad";
        lastUsedDevice = lastDevice;
        return lastUsedDevice;
    }
    else{
        lastUsedDevice = lastDevice;
        return lastUsedDevice;
    }
    // return lastUsedDevice;
}

private bool DetectAllKeysPressed()
{
    // Obtenez la liste de toutes les touches disponibles
    var allKeys = Keyboard.current.allKeys;

    // Parcourez chaque touche pour voir si elle a été pressée ou relâchée pendant cette frame
    foreach (var key in allKeys)
    {
        if (key.isPressed)
        {
            return true;
        }
    }
    return false;
}

private bool DetectMouseButtonsPressed()
{
    Vector2 mouseDelta = Mouse.current.delta.ReadValue();
    if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed || mouseDelta != Vector2.zero)
    {
        return true;
    }

    return false;
}


private bool DetectGamepadButtonsPressed()
{
    if (Gamepad.current == null)
    {
        return false;
    }
    // Obtenez la liste de tous les boutons du gamepad disponibles
    var allGamepadButtons = Gamepad.current.allControls;

    // Parcourez chaque bouton du gamepad pour voir s'il a été pressé ou relâché pendant cette frame
    foreach (var gamepadButton in allGamepadButtons)
    {
        if (gamepadButton is ButtonControl buttonControl)
        {
            if (buttonControl.isPressed)
            {
                return true;
            }
        }
    }
    return false;
}
    public List<string> GetKeysForAction(string actionName)
    {
        List<string> keys = new List<string>();
        var action = _playerInput.actions[actionName];

        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                string key = binding.ToString();
                string keyboard = "Keyboard";
                string gamepad = "Gamepad";
                // Debug.Log(key);

                // Trouver l'index du premier '/'
                int indexOfSlash = key.IndexOf('/');

                // Trouver l'index du premier '[' après le '/'
                int indexOfBracket = key.IndexOf('[', indexOfSlash);

                if (key.Contains(gamepad) && indexOfSlash != -1 && indexOfBracket != -1)
                {
                    string gamepadButton = key.Substring(indexOfSlash + 1, indexOfBracket - indexOfSlash - 1);
                    keys.Add(gamepadButton);
                }
                else if (key.Contains(keyboard) && indexOfSlash != -1 && indexOfBracket != -1)
                {
                    string keyboardButton = key.Substring(indexOfSlash + 1, indexOfBracket - indexOfSlash - 1);
                    keys.Add(keyboardButton);
                }
            }
        }

        return keys;
    }
// public void ProcessKeysForAction(string actionName)
// {
//     List<string> keys = GetKeysForAction(actionName);

//     // Nouvelles variables pour stocker les résultats
//     List<string> extractedResults = new List<string>();

//     // Parcourir toutes les clés
//     foreach (var key in keys)
//     {
//         // Trouver l'index du caractère '/'
//         int startIndex = key.IndexOf('/') + 1;

//         // Trouver l'index du caractère '['
//         int endIndex = key.IndexOf('[');

//         // Extraire la partie entre '/' et '['
//         if (startIndex > 0 && endIndex > 0)
//         {
//             string extractedResult = key.Substring(startIndex, endIndex - startIndex);
//             extractedResults.Add(extractedResult);
//         }
//     }

//     // Utiliser les nouvelles variables comme nécessaire
//     foreach (var result in extractedResults)
//     {
//         Debug.Log("Extracted Result: " + result);
//     }
// }


}
