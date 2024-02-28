using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinCreator : MonoBehaviour
{
    public string titleString;
    public int price;
    public Sprite skin; 
    public TextMeshProUGUI title;
    public Material material;
    public Material material2;
    public Material material3;
    public Material material4;
    public GameObject weapon;
    public UnityEngine.UI.Image imageSkin;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI buttonText;
    public Button buttonBuy;
    private string buy = "Buy";
    private string owned = "Owned";
    private Transform player;
    public PlayerMovement2 playerScript;
    public GameObject chargerUI;
    public int prefabWeapon;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip wrongBuy;
    public BuySkins buySkins;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<PlayerMovement2>();
        title.text = titleString;
        imageSkin.sprite = skin;
        
        

    }
    void Update(){
        if(weapon != null && material != null){
            if (!AreTexturesEqual(getMaterial(weapon), material))
            {
                buttonText.text = price.ToString() + " $";

                // buttonBuy.enabled = false;
                // buttonBuy.interactable = true;

            }
            else{
                // buttonBuy.enabled = true;
                // buttonBuy.interactable = false;
                buttonText.text = owned;
            }
        }
        if(priceText != null){
            priceText.text = price.ToString() + " $";
        }
        
    }

    public Material getMaterial(GameObject weapon){
        Renderer rendererComponent;
        Material weaponMaterial;
        if (weapon != null)
        {
            rendererComponent = weapon.GetComponent<Renderer>();
            if (rendererComponent != null)
            {
                weaponMaterial = rendererComponent.material;
                return weaponMaterial;
                
            }
        }
        return null;
    }
    private bool AreTexturesEqual(Material material1, Material material2)
    {
        return material1.mainTexture == material2.mainTexture;
    }

    // public void changeMaterial(Material material){
    //     if(playerScript.money >= price && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon])
    //     {
    //         if (!AreTexturesEqual(getMaterial(weapon), material))
    //         {
    //             Debug.Log("texturepasegal");
    //             weapon.GetComponent<Renderer>().material = material;
    //             SoundManager.Instance.PlaySound(buySound);
    //             chargerUI.GetComponent<MoneyUI>().UpdateMoney(-price);
    //         }
    //         else{
    //             SoundManager.Instance.PlaySound(wrongBuy);
    //         }
    //     }
    //     else{
    //         SoundManager.Instance.PlaySound(wrongBuy);
    //     }
    // }
    public void changeMaterialWeapon(){
        if(playerScript.money >= price && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon])
        {
            if (!AreTexturesEqual(getMaterial(weapon), material))
            {
                weapon.GetComponent<Renderer>().material = material;
                SoundManager.Instance.PlaySound(buySound);
                chargerUI.GetComponent<MoneyUI>().UpdateMoney(-price);
                buySkins.hideSkins();
            }
            else{
                SoundManager.Instance.PlaySound(wrongBuy);
            }
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }

    public void changeMaterialDouble(){
        if(playerScript.money >= price && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon])
        {
            
            if (!AreTexturesEqual(getMaterial(weapon), material))
            {
                
                List<Material> materialsList = new List<Material> { material, material2 };
                
                weapon.GetComponent<Renderer>().SetMaterials(materialsList);
                SoundManager.Instance.PlaySound(buySound);
                chargerUI.GetComponent<MoneyUI>().UpdateMoney(-price);
                buySkins.hideSkins();
            }
            else{
                SoundManager.Instance.PlaySound(wrongBuy);
            }
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }
    public void changeMaterialQuadruple(){
        if(playerScript.money >= price && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon])
        {
            
            if (!AreTexturesEqual(getMaterial(weapon), material))
            {
                
                List<Material> materialsList = new List<Material> { material, material2, material3, material4 };
                Debug.Log(materialsList);
                weapon.GetComponent<Renderer>().SetMaterials(materialsList);
                SoundManager.Instance.PlaySound(buySound);
                chargerUI.GetComponent<MoneyUI>().UpdateMoney(-price);
                buySkins.hideSkins();
            }
            else{
                SoundManager.Instance.PlaySound(wrongBuy);
            }
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }

    public void AcheterMunitions()
    {
        if(playerScript.money >= price && playerScript.weaponController.bulletsTotal < playerScript.weaponController.bulletsTotalInitial && playerScript.inventory.GetItem(playerScript.primarySecondary) == playerScript.ListWeapons[prefabWeapon]) // 
        {
            playerScript.money = playerScript.money - price;
            playerScript.weaponController.bulletsTotal = playerScript.weaponController.bulletsTotalInitial;
            SoundManager.Instance.PlaySound(buySound);
            chargerUI.GetComponent<MoneyUI>().UpdateMoney(-price);
            buySkins.hideSkins();
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }
}
