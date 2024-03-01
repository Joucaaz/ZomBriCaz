using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyAtout : MonoBehaviour
{
    private float distanceDetection = 5f; // Ajustez cette valeur selon vos besoins
    private Transform player;
    public PlayerMovement2 playerScript;
    public Camera camera;
    public Transform cursorUI;
    public TextMeshProUGUI buyAtout;
    public TextMeshProUGUI price;
    public int coutAtout; // Coût de l'arme
    public string nameAtout;
    public LayerMask excludeLayer;
    public GameObject collider;
    public Canvas indicationCanvas;
    [SerializeField] public bool atoutAchete;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip wrongBuy;
    public GameObject chargerUI;
    public GameObject parentAtout;
    public GameObject parentWeapons;
    public Sprite imageMastodonte;
    public Sprite imageSpeed;
    public Sprite imageMoney;
    public Sprite imageReload;
    public Sprite imageAmmo;
    public GameObject armAtout;
    public GameObject medoc;
    public Material material1;
    public Material material2;
    public string descriptionAtout;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI titleText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<PlayerMovement2>();
        titleText.text = nameAtout;
        descriptionText.text = descriptionAtout;
    }

    // Update is called once per frame
    void Update()
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
                if(atoutAchete){
                    buyAtout.text = "You already have this specialty.";
                }
                else{
                    buyAtout.text = "Press " + firstKey + " to buy the " + nameAtout + " for :";
                }
                
            }
        }
        else if(UserInput.instance.DetectAllUserInputs() == "gamepad")
        {
            List<string> keys = UserInput.instance.GetKeysForAction("BuyButton");
            if (keys != null && keys.Count > 0)
            {
                string firstKey = keys[1];
                if(atoutAchete){
                    buyAtout.text = "You already have this specialty.";
                }
                else{
                    buyAtout.text = "Press <sprite name=\"" + firstKey + "\"> to buy the " + nameAtout + " for :";
                }
                
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
    }

    public void AfficherIndicationAchat()
    {
        indicationCanvas.gameObject.SetActive(true);
        if(atoutAchete == false){
        
            price.enabled = true;
            price.text = coutAtout.ToString() + " $";
            if (UserInput.instance.BuyWeaponInput)// defaultPlayerActions.actions["BuyButton"].triggered
            {
                AcheterAtout();
            }
        }
        else{
            price.enabled = false;
        }
        
    }

    public void CacherIndicationAchat()
    {
        indicationCanvas.gameObject.SetActive(false);
    }

    private void AcheterAtout()
    {
        if(playerScript.money >= coutAtout)
        {
            playerScript.money = playerScript.money - coutAtout;

            switch (nameAtout)
            {
                case "Jugger-nog":
                    AddMastodonte();
                    animAtout(playerScript.inventory.GetItem(playerScript.primarySecondary), armAtout);
                    playerScript.countAtout += 1;
                    break;
                case "SonicSurge":
                    AddSpeed();
                    animAtout(playerScript.inventory.GetItem(playerScript.primarySecondary), armAtout);
                    playerScript.countAtout += 1;
                    break;
                case "DollarDiscount":
                    AddMoney();
                    animAtout(playerScript.inventory.GetItem(playerScript.primarySecondary), armAtout);
                    playerScript.countAtout += 1;
                    break;
                case "ReloadRush":
                    AddReload();
                    animAtout(playerScript.inventory.GetItem(playerScript.primarySecondary), armAtout);
                    playerScript.countAtout += 1;
                    break;
                case "BulletsFury":
                    AddAmmos();
                    animAtout(playerScript.inventory.GetItem(playerScript.primarySecondary), armAtout);
                    playerScript.countAtout += 1;
                    break;
                default:
                    Debug.Log("Atout inconnu");
                    break;
            }
            // if(playerScript.inventory.GetItem(1) != null){
            //     parent.transform.Find(playerScript.inventory.GetItem(playerScript.primarySecondary).name).GetComponent<ArmeAchat>().armeAchete = false;
            // }

            atoutAchete = true;
            SoundManager.Instance.PlaySound(buySound);
            chargerUI.GetComponent<MoneyUI>().UpdateMoney(-coutAtout);
            CacherIndicationAchat();
        }
        else{
            SoundManager.Instance.PlaySound(wrongBuy);
        }
    }

    public void animAtout(GameObject beforeWeapon, GameObject afterWeapon)
    {
        StartCoroutine(ChangeWeaponAtoutCoroutine(beforeWeapon, afterWeapon));
    }

    private IEnumerator ChangeWeaponAtoutCoroutine(GameObject beforeWeapon, GameObject armAtout)
    {

        // Jouez l'animation "DrawReverse"
        playerScript.characterAnimator.Play("DrawReverse");

        yield return new WaitForSeconds(beforeWeapon.GetComponent<WeaponController>().endDraw);

        List<Material> materialsList = new List<Material> { material1, material2 };
                
        medoc.GetComponent<Renderer>().SetMaterials(materialsList);

        beforeWeapon.SetActive(false);
        armAtout.SetActive(true);
        playerScript.characterAnimator = armAtout.GetComponent<Animator>();

        playerScript.characterAnimator.Play("EatmMedoc");

        yield return new WaitForSeconds(1.5f);

        armAtout.SetActive(false);
        beforeWeapon.SetActive(true);

        playerScript.characterAnimator = beforeWeapon.GetComponent<Animator>();

    }

    public void AddMastodonte(){
        playerScript.healthPlayer = 150;
        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageMastodonte;

    }
    public void AddSpeed(){
        
        playerScript.initialWalkSpeed = playerScript.initialWalkSpeed + 1;
        playerScript.walkSpeed = playerScript.initialWalkSpeed;
        playerScript.runSpeed = playerScript.runSpeed + 1;
        // playerScript.crouchPosition = playerScript.crouchPosition + 1;
        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageSpeed;
    }
    public void AddMoney(){
        GameObject[] moneyObjects = GameObject.FindGameObjectsWithTag("Money");

        foreach (GameObject moneyObject in moneyObjects)
        {
            ArmeAchat moneyScript = moneyObject.GetComponent<ArmeAchat>();
            BuyAtout moneyAtout = moneyObject.GetComponent<BuyAtout>();
            SkinCreator moneySkin = moneyObject.GetComponent<SkinCreator>();

            if (moneyScript != null)
            {
                moneyScript.coutArme = Mathf.RoundToInt(moneyScript.coutArme / 2f);
                moneyScript.coutAmmo = Mathf.RoundToInt(moneyScript.coutAmmo / 2f);
            }
            if(moneyAtout != null){
                moneyAtout.coutAtout = Mathf.RoundToInt(moneyAtout.coutAtout / 2f);

            }
            if(moneySkin != null){
                moneySkin.price = Mathf.RoundToInt(moneySkin.price / 2f);
            }     
            
        }
        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageMoney;
    }
    public void AddReload(){
        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageReload;
    }

    public void AddAmmos(){
        WeaponController[] reloadObjects = new WeaponController[parentWeapons.transform.childCount - 2];
        for (int i = 0; i < parentWeapons.transform.childCount - 2; i++)
        {
            reloadObjects[i] = parentWeapons.transform.GetChild(i).gameObject.GetComponent<WeaponController>();
            reloadObjects[i].numberofMagazine = reloadObjects[i].numberofMagazine + 3;
            reloadObjects[i].bulletsTotal = reloadObjects[i].numberofMagazine * reloadObjects[i].maxBulletsInOneMagazine;
        }
        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageAmmo;
    }

}
