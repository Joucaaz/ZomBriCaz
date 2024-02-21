using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfoEnd : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nbKills;
    [SerializeField] public TextMeshProUGUI nbWaves;
    [SerializeField] public TextMeshProUGUI timeDuration;

    public ChargerUI chargerUI;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CC");
        int minutes = Mathf.FloorToInt(chargerUI.elapsedTime/60);
        int secondes = Mathf.FloorToInt(chargerUI.elapsedTime % 60);
        // nbKills.text = chargerUI.nbKills.ToString();
        nbWaves.text = chargerUI.waveNumber.text;
        timeDuration.text = string.Format("{0:00}:{1:00}", minutes, secondes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void goToHome(){
        SceneManager.LoadScene("HomeMenu");
    }
}
