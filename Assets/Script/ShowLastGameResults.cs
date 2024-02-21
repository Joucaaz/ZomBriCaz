using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowLastGameResults : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kills;
    [SerializeField] TextMeshProUGUI waves;
    [SerializeField] TextMeshProUGUI duration;
    // Start is called before the first frame update
    void Start()
    {
        
        string jsonData = PlayerPrefs.GetString("GameData");

        // Convertissez le JSON en objet GameData
        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

        // Affichez les informations dans le Text (ou où vous en avez besoin)
        if (gameData != null)
        {
            kills.text = gameData.kills.ToString();
            waves.text = gameData.waves.ToString();
            duration.text = gameData.time.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
