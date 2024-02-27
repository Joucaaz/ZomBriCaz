using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class BuySkins : MonoBehaviour
{
    private PlayerInput defaultPlayerActions;
    public GameObject skinsFirstSelected;
    private Transform player;
    public PlayerMovement2 playerScript;
    public ArmeAchat armeAchat;
    public PauseManager pauseManager;
    public Canvas skinCanvas;
    public bool gamepad = true;
    private void Awake(){
        defaultPlayerActions = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<PlayerMovement2>();
    }
    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.CancelInput)// defaultPlayerActions.actions["BuyButton"].triggered
        {
            hideSkins();
        }
        if(pauseManager.boolShowSkins){
            if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !gamepad){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gamepad = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(skinsFirstSelected);
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

    public void hideSkins()
    {
        pauseManager.boolShowSkins = false;
        armeAchat.buyAmmo.enabled = true;
        gameObject.SetActive(false);
    }

    public void showSkins(){
        pauseManager.boolShowSkins = true;
        armeAchat.buyAmmo.enabled = false;
        gameObject.SetActive(true);
    }
}
