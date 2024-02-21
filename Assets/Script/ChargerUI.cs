using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChargerUI : MonoBehaviour
{
    public PlayerMovement2 playerMovementScript; // R�f�rence au script PlayerMovement
    public Animator characterAnimator;
    public TextMeshProUGUI nbBalles;
    public TextMeshProUGUI nbBallesTotal;
    public TextMeshProUGUI textReload;
    public Image weaponsImage;
    public Image bloodImage;

    public Sprite blood100;
    public Sprite blood50;
    public Sprite blood0;
    public Sprite noneBlood;

    public TextMeshProUGUI money;
    // public Image ammoPrefab; // Le pr�fab d'un carr� de munition.
    // float spacingX = -0.2f; // Espacement en X entre les carr�s de munition.
    [SerializeField] public TextMeshProUGUI timerText;
    public float elapsedTime;

    [SerializeField] public TextMeshProUGUI waveNumber;
    [SerializeField] public TextMeshProUGUI zombieLeft;

    [SerializeField] public ZombieSpawner zombieSpawner;
    public int zombieNumber;

    public int bullets;
    public int bulletsTotal;
    public Color32 colorRed = new Color32(229, 6, 6, 255);
    public Color32 colorGreen = new Color32(46, 169, 19, 255);
    public Color32 textColor = new Color32(242, 235, 235, 255);

    public Canvas canvaBlood;
    // public int nbKills = 0;
    public Image hitTouch;
    public Image hitDead;



    private void Start()
    {
        textReload.enabled = false;
        //UpdateUI();
        //bullets = playerMovementScript.bulletsInside;
        //GenerateAmmoSlots();
        // textReload.enabled = false;
        //zombieSpawner = gameObject.GetComponent<ZombieSpawner>();
        // zombieSpawner =
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime/60);
        int secondes = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, secondes);

        zombieLeft.text = zombieSpawner.zombiesList.Count.ToString();
        waveNumber.text = zombieSpawner.numberOfWave.ToString();

        money.text = playerMovementScript.money.ToString();
        weaponsImage.sprite = playerMovementScript.inventory.GetItem(playerMovementScript.primarySecondary).GetComponent<WeaponController>().weaponImage;
        
        characterAnimator = playerMovementScript.characterAnimator;
        bulletsTotal = playerMovementScript.bulletsTotal;
        bullets = playerMovementScript.bulletsInside;
        nbBalles.text = bullets.ToString();
        nbBallesTotal.text = bulletsTotal.ToString();
        if(bullets <= playerMovementScript.inventory.GetItem(playerMovementScript.primarySecondary).GetComponent<WeaponController>().reloadingConsidered)
        {
            
            nbBalles.color = colorRed;
            textReload.enabled = true;
            
            if (characterAnimator.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
            {
                textReload.enabled = false;
            }
            else
            {
                if(UserInput.instance.DetectAllUserInputs() == "keyboard")
                {
                    List<string> keys = UserInput.instance.GetKeysForAction("Reload");

                    if (keys != null && keys.Count > 0)
                    {
                        string firstKey = keys[0];
                        textReload.text = "Press " + firstKey + " to reload";
                    }
                }
                else if(UserInput.instance.DetectAllUserInputs() == "gamepad")
                {
                    List<string> keys = UserInput.instance.GetKeysForAction("Reload");

                    if (keys != null && keys.Count > 0)
                    {
                        string firstKey = keys[1];
                        textReload.text = "Press <sprite name=\"" + firstKey + "\"> to reload";
                    }
                }
                
            }
            /*
            if (Input.GetKey(KeyCode.R))
            {
                textReload.enabled = false;
            }*/
        }
        else if(bullets == playerMovementScript.inventory.GetItem(playerMovementScript.primarySecondary).GetComponent<WeaponController>().maxBulletsInOneMagazine)
        {
            nbBalles.color = colorGreen;
            textReload.enabled = false;

        }
        else
        {
            nbBalles.color = textColor;
            textReload.enabled = false;
        }
        if(playerMovementScript.healthPlayer == 100){
            canvaBlood.GetComponent<CanvasGroup>().alpha = 1f;
            bloodImage.sprite = blood100;
        }
        else if(playerMovementScript.healthPlayer == 50){
            canvaBlood.GetComponent<CanvasGroup>().alpha = 1f;
            bloodImage.sprite = blood50;
        }
        else if(playerMovementScript.healthPlayer <= 0){
            canvaBlood.GetComponent<CanvasGroup>().alpha = 1f;
            bloodImage.sprite = blood0;
        }
        else{
            canvaBlood.GetComponent<CanvasGroup>().alpha = 1f;
            bloodImage.sprite = noneBlood;
        }

    }
    /*
    private void UpdateUI()
    {
        for (int i = 0; i < bulletSlots.Length; i++)
        {
            bulletSlots[i].color = (i < playerMovementScript.bulletsInside) ? fullColor : emptyColor;
        }
    }*/
    // private void GenerateAmmoSlots()
    // {
    //     Image firstAmmoSlot = Instantiate(ammoPrefab, transform);
    //     Vector3 position = firstAmmoSlot.transform.position;
    //     //Debug.Log(position.x);

    //     for (int i = 1; i < bullets; i++)
    //     {
    //         position.x += spacingX * i;
    //         //Debug.Log(position.x);
    //         Image ammoSlot = Instantiate(ammoPrefab, position, Quaternion.identity, transform);
    //     }
    // }
}
