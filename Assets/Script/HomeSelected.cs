using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeSelected : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(GameManager.Instance.isHomeMenu && !GameManager.Instance.logMenu){
        //     // Debug.Log("home");
        //     if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
        //         Cursor.lockState = CursorLockMode.Locked;
        //         Cursor.visible = false;
        //         GameManager.Instance.gamepad = true;
        //         EventSystem.current.SetSelectedGameObject(null);
        //         EventSystem.current.SetSelectedGameObject(GameManager.Instance.homeFirstSelected);
        //     }
        //     else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
        //         Cursor.lockState = CursorLockMode.None;
        //         Cursor.visible = true;
        //         GameManager.Instance.gamepad = false;
        //         EventSystem.current.SetSelectedGameObject(null);
        //     }
        // }
        // else if(GameManager.Instance.isCreditMenu){
        //         if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
        //             Cursor.lockState = CursorLockMode.Locked;
        //             Cursor.visible = false;
        //             GameManager.Instance.gamepad = true;
        //             EventSystem.current.SetSelectedGameObject(null);
        //             EventSystem.current.SetSelectedGameObject(GameManager.Instance.creditsFirstSelected);
        //         }
        //         else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
        //             Cursor.lockState = CursorLockMode.None;
        //             Cursor.visible = true;
        //             GameManager.Instance.gamepad = false;
        //             EventSystem.current.SetSelectedGameObject(null);
        //         }
        //     }
        // else if(GameManager.Instance.isBestScoreMenu){
        //         if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
        //             Cursor.lockState = CursorLockMode.Locked;
        //             Cursor.visible = false;
        //             GameManager.Instance.gamepad = true;
        //             EventSystem.current.SetSelectedGameObject(null);
        //             EventSystem.current.SetSelectedGameObject(GameManager.Instance.bestFirstSelected);
        //         }
        //         else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
        //             Cursor.lockState = CursorLockMode.None;
        //             Cursor.visible = true;
        //             GameManager.Instance.gamepad = false;
        //             EventSystem.current.SetSelectedGameObject(null);
        //         }
        //     }

        if(!GameManager.Instance.logMenu){
            // Debug.Log("home");
            if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameManager.Instance.gamepad = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(GameManager.Instance.homeFirstSelected);
            }
            else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.Instance.gamepad = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        
        }     
        
    }
}
