using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestGameResults : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kills;
    [SerializeField] TextMeshProUGUI waves;
    [SerializeField] TextMeshProUGUI duration;

    [SerializeField] TextMeshProUGUI kills2;
    [SerializeField] TextMeshProUGUI waves2;
    [SerializeField] TextMeshProUGUI duration2;

    [SerializeField] TextMeshProUGUI kills3;
    [SerializeField] TextMeshProUGUI waves3;
    [SerializeField] TextMeshProUGUI duration3;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update(){
        
        string jsonData = PlayerPrefs.GetString("HighScores", "{}");
        HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(jsonData);
        // Debug.Log("Après réinitialisation : " + JsonUtility.ToJson(highScoreData));
        // Debug.Log("kill game 1 : " + highScoreData.bestScore1.kills.ToString());

        // Affichez les informations dans le Text (ou où vous en avez besoin)
        if (highScoreData.bestScore1 != null)
        {
            kills.text = highScoreData.bestScore1.kills.ToString();
            // Debug.Log("test " + highScoreData.bestScore1.kills);
            // Debug.Log("test2 " + kills.text);
            waves.text = highScoreData.bestScore1.waves.ToString();
            duration.text = highScoreData.bestScore1.time;
        }
        if (highScoreData.bestScore2 != null)
        {
            kills2.text = highScoreData.bestScore2.kills.ToString();
            waves2.text = highScoreData.bestScore2.waves.ToString();
            duration2.text = highScoreData.bestScore2.time;
        }
        if (highScoreData.bestScore3 != null)
        {
            kills3.text = highScoreData.bestScore3.kills.ToString();
            waves3.text = highScoreData.bestScore3.waves.ToString();
            duration3.text = highScoreData.bestScore3.time;
        }
    }

}
