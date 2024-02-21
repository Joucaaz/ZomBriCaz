using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmeAchat : MonoBehaviour
{
    private PlayerInput defaultPlayerActions;
    public int coutArme = 100; // Coût de l'arme
    public int coutAmmo = 20;
    public Sprite imageArmeNonAchete; // Image de l'arme non achetée
    public Sprite imageArmeAchete; // Image de l'arme achetée
    private float distanceDetection = 5f; // Ajustez cette valeur selon vos besoins
    public int prefabWeapon;

    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip wrongBuy;

    [SerializeField] bool armeAchete;
    public Canvas indicationCanvas;

    public TextMeshProUGUI buyWeapons;
    public TextMeshProUGUI buyAmmo;
    public TextMeshProUGUI price;
    private Transform player;
    public PlayerMovement2 playerScript;
    public Camera camera;
    public Transform cursorUI;
    public GameObject collider;
    public GameObject parent;
    public GameObject chargerUI;
    public string nameWeapons;
    public LayerMask excludeLayer;
    public BuySkins buySkins;

    
     
    private void Awake(){
        defaultPlayerActions = GetComponent<PlayerInput>();
        // defaultPlayerActions = new ActionZombieBriCazV2();
        // defaultPlayerActions.Enable();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<PlayerMovement2>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 screenPos = camera.WorldToScreenPoint(cursorUI.position);
        Ray ray = camera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        RaycastHit hit;
        if(UserInput.instance.DetectAllUserInputs() == "keyboard")
        {
            List<string> keys = UserInput.instance.GetKeysForAction("BuyButton");
            if (keys != null && keys.Count > 0)
            {
                string firstKey = keys[0];
                buyWeapons.text = "Press " + firstKey + " to buy the " + nameWeapons + " for :";
                buyAmmo.text = "Press " + firstKey + " to buy " + nameWeapons + " ammos & skins";
            }
        }
        else if(UserInput.instance.DetectAllUserInputs() == "gamepad")
        {
            List<string> keys = UserInput.instance.GetKeysForAction("BuyButton");
            if (keys != null && keys.Count > 0)
            {
                string firstKey = keys[1];
                buyWeapons.text = "Press <sprite name=\"" + firstKey + "\"> to buy the " + nameWeapons + " for :";
                buyAmmo.text = "Press <sprite name=\"" + firstKey + "\"> to buy " + nameWeapons + " ammos & skins";
            }
        }

        if (Physics.Raycast(ray, out hit, 1000, excludeLayer))
        {
            if (hit.collider != null && hit.collider.gameObject == collider && distanceToPlayer < distanceDetection)
            {
                AfficherIndicationAchat();
            }
            else{
                CacherIndicationAchat();
            }
        }
        if(armeAchete){
            GetComponent<SpriteRenderer>().sprite = imageArmeAchete;
        }
        else{
            GetComponent<SpriteRenderer>().sprite = imageArmeNonAchete;
        }
    }
    public void AfficherIndicationAchat()
    {
        indicationCanvas.gameObject.SetActive(true);
        if(armeAchete == false){
            
            
            buyWeapons.enabled = true;
            buyAmmo.enabled = false;
            price.enabled = true;
            price.text = coutArme.ToString();
            if (UserInput.instance.BuyWeaponInput)// defaultPlayerActions.actions["BuyButton"].triggered
            {
                AcheterArme();
            }
            // if (Input.GetButtonDown("BuyButton"))
            // {
            //     AcheterArme();
                
            // }
            // Debug.Log("Appuyez sur une touche pour acheter l'arme");
        }
        else{
            buyAmmo.enabled = true;
            buyWeapons.enabled = false;
            price.enabled = false;
            // price.text = coutAmmo.ToString();
            if (UserInput.instance.BuyWeaponInput && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon])
            {
                buySkins.showSkins();
                // AcheterMunitions();
            }
            // if (Input.GetButtonDown("BuyButton"))
            // {
            //     AcheterMunitions();
            // }
            // Debug.Log("Appuyez sur une touche pour acheter munitions");
        }
        
    }

    public void CacherIndicationAchat()
    {
        indicationCanvas.gameObject.SetActive(false);
    }

    private void AcheterArme()
    {
        if(playerScript.money >= coutArme)
        {
            playerScript.money = playerScript.money - coutArme;
            if(playerScript.inventory.GetItem(1) != null){
                parent.transform.Find(playerScript.inventory.GetItem(playerScript.primarySecondary).name).GetComponent<ArmeAchat>().armeAchete = false;
            }
            playerScript.inventory.AddItem(playerScript.ListWeapons[prefabWeapon], playerScript.primarySecondary);
            armeAchete = true;
            SoundManager.Instance.PlaySound(buySound);
            chargerUI.GetComponent<MoneyUI>().UpdateMoney(-coutArme);
            CacherIndicationAchat();
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }

    private void AcheterMunitions()
    {
        if(playerScript.money >= coutAmmo && playerScript.weaponController.bulletsTotal < playerScript.weaponController.bulletsTotalInitial && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon]) // 
        {
            playerScript.money = playerScript.money - coutAmmo;
            playerScript.weaponController.bulletsTotal = playerScript.weaponController.bulletsTotalInitial;
            SoundManager.Instance.PlaySound(buySound);
            chargerUI.GetComponent<MoneyUI>().UpdateMoney(-coutAmmo);
            CacherIndicationAchat();
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }
}
