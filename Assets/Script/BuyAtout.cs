using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<PlayerMovement2>();
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
                    buyAtout.text = "You already have this speciality.";
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
                    buyAtout.text = "You already have this speciality.";
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
                case "Mastodonte":
                    AddMastodonte();
                    playerScript.countAtout += 1;
                    break;
                case "Speed":
                    AddSpeed();
                    playerScript.countAtout += 1;
                    break;
                case "Sales":
                    AddMoney();
                    playerScript.countAtout += 1;
                    break;
                case "Reload":
                    AddReload();
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
        // playerScript.updateSpeedReload();
        // GameObject[] reloadObjects = new GameObject[parentWeapons.transform.childCount - 2];
        // for (int i = 0; i < parentWeapons.transform.childCount - 2; i++)
        // {
        //     reloadObjects[i] = parentWeapons.transform.GetChild(i).gameObject;
        // }

        // foreach (GameObject reloadObject in reloadObjects)
        // {
        //     Debug.Log(reloadObject);
        //     Animator animator = reloadObject.GetComponent<Animator>();
        //     Debug.Log(animator);           

        //     if (animator != null)
        //     {
        //         // AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(1);//could replace 0 by any other animation layer index
                
        //         // Debug.Log(animator["Reload"]);

        //         animator.SetFloat("speedReload", 5f);
        //     }
        // }

        parentAtout.transform.GetChild(playerScript.countAtout).GetComponent<UnityEngine.UI.Image>().sprite = imageReload;
    }

    public AnimationClip FindAnimation (Animator animator, string name) 
    {
    foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
    {
        // Debug.Log("clip : " + clip);
        if (clip.name.Contains(name))
        {
            return clip;
        }
    }

    return null;
    }
}
