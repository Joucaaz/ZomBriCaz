using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using Unity.VisualScripting;
using System;
using TMPro;
using UnityEngine.Networking;

public class LootLockerManager : MonoBehaviour
{
    public Canvas canvaConnected;
    public TMP_InputField inputPseudo; 
    string playerName;
    void Start()
    {
        StartCoroutine(LoginRoutine());
        
    }

    void Update(){
        // Debug.Log(GetPlayerName());
    }
    IEnumerator LoginRoutine(){
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("successfully started LootLocker session");
                StartCoroutine(GetPlayerNameRoutine());
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                Debug.Log(response.player_id.ToString());
                done = true;
            }
            else{
                Debug.Log("error starting LootLocker session");
                done = true;
            }

        });
        
        yield return new WaitWhile(() => done == false);
    }

    IEnumerator GetPlayerNameRoutine(){

        yield return StartCoroutine(GetPlayerName());
        if (playerName == null)
        {
            ChooseUsername();
        }
        else
        {
            Debug.Log("Already Connected");
        }
        Debug.Log(playerName);
    }

    private void ChooseUsername()
    {
        GameManager.Instance.logMenu = true;
        canvaConnected.gameObject.SetActive(true);
    }
    public void SubmitUsername(){
        GameManager.Instance.logMenu = false;
        canvaConnected.gameObject.SetActive(false);
        LootLockerSDKManager.SetPlayerName(inputPseudo.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name" + response.name);
                PlayerPrefs.SetString("PlayerName", response.name);
                StartCoroutine(PostRequestUser("https://joudcazeaux.fr/ZomBriCaz/zombricazUserSave.php", PlayerPrefs.GetString("PlayerID"), response.name));
            } else
            {
                ChooseUsername();
                Debug.Log("Error setting player name");
            }
        });
    }

    IEnumerator PostRequestUser(string uri, string userId, string nameUser)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("nameUser", nameUser);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la requête POST : " + webRequest.error);
            }
            else
            {
                Debug.Log("Requête POST réussie : " + webRequest.downloadHandler.text);
            }
        }
    }

    

    IEnumerator GetPlayerName(){
        bool done = false;
        playerName = null;
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                if(response.name == ""){
                    playerName = null;
                }
                else{
                    playerName = response.name;
                }
                
                done = true;
            } else
            {
                Debug.Log("Error getting player name");
                playerName = null;
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

//     public string GetPlayerName(){
//         string playerName = null;
//         LootLockerSDKManager.GetPlayerName((response) =>
//         {
//             if (response.success)
//             {
//                 Debug.Log("Successfully retrieved player name: " + response.name);
//                 playerName = response.name;
//                 return response.name.ToString(); 
//             } else
//             {
//                 Debug.Log("Error getting player name");
//                 playerName = null;
//             }
//         });
//         return playerName;
//     }
}