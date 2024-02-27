using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(UserInput.instance.CancelInput){
        //     GameManager.Instance.loadHome();
        // }
    }
    public void playSoundMenu(AudioClip clip){
        SoundManager.Instance.PlaySound(clip);
    }
    public void loadTheGame(){
        GameManager.Instance.loadGameScreen();
    }
    public void returnHome(){
        GameManager.Instance.loadHome();
    }
    public void openCredits(){
        GameManager.Instance.loadCredits();
    }
    public void openBestScore(){
        GameManager.Instance.loadScores();
    }
    public void openSettings(){
        GameManager.Instance.loadSettings();
    }
    public void openOnlineRankings(){
        GameManager.Instance.loadOnlineRankings();
    }
    public void quitGame(){
        GameManager.Instance.quit();
    }
    public void openJoucazWebsite(){
        GameManager.Instance.OpenWebsite("https://joudcazeaux.fr");
    }
    public void openJoucazTwitter(){
        GameManager.Instance.OpenWebsite("https://twitter.com/JoucazJC");
    }
    public void openJoucazInsta(){
        GameManager.Instance.OpenWebsite("https://www.instagram.com/joucazjc/");
    }
    public void openJoucazLinkedin(){
        GameManager.Instance.OpenWebsite("https://www.linkedin.com/in/joudcazeaux/");
    }
    public void openJoucazMail(){
        GameManager.Instance.OpenWebsite("mailto:joud.cazeaux@free.fr");
    }
}
