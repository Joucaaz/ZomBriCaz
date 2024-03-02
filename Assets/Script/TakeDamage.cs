using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement2 playerMovement;
    // public SlowMotion slowMotion;
    public float damageDegats = 50;
    public Zombie zombieScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement2>();
        // slowMotion = player.GetComponent<SlowMotion>();
        //cameraShake = Camera.main.GetComponent<CameraShake>();

    }

    public void damage()
    {

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
        Debug.Log(distance);
        if (distance <= 2.0f) 
        {
            playerMovement.healthPlayer -= damageDegats;
            playerMovement.callBlood();
            playerMovement.reloadingEnCours = false;
            SoundManager.Instance.StopReloadSound();
            playerMovement.characterAnimator.Play("takeDamage");
            playerMovement.reloadingEnCours = false;
            if(playerMovement.healthPlayer <= 0 && playerMovement.characterAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1.0f){
                // slowMotion.StartSlowMotion();
                playerMovement.playerDie();    
            }
        }
        
    }

    public void changeZombieAttackBool(){
        zombieScript.zombieAttack = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void startZombie(){
        zombieScript.zombieStart = true;
    }

}