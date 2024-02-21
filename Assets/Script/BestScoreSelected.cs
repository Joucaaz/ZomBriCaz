using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BestScoreSelected : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(UserInput.instance.CancelInput && GameManager.Instance.isBestScoreMenu){
            GameManager.Instance.loadHome();
        }
        if(UserInput.instance.DetectAllUserInputs() == "gamepad" && !GameManager.Instance.gamepad){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManager.Instance.gamepad = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(GameManager.Instance.bestFirstSelected);
                }
                else if(UserInput.instance.DetectAllUserInputs() == "keyboard"){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    GameManager.Instance.gamepad = false;
                    EventSystem.current.SetSelectedGameObject(null);
                }
    }
}
