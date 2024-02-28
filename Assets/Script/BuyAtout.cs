using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyAtout : MonoBehaviour
{
    private float distanceDetection = 5f; // Ajustez cette valeur selon vos besoins
    private Transform player;
    public PlayerMovement2 playerScript;
    public Camera camera;
    public Transform cursorUI;
    public TextMeshProUGUI buyAtout;
    public string nameAtout;
    public LayerMask excludeLayer;
    public GameObject collider;
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
                buyAtout.text = "Press " + firstKey + " to buy the " + nameAtout + " for :";
            }
        }
        else if(UserInput.instance.DetectAllUserInputs() == "gamepad")
        {
            List<string> keys = UserInput.instance.GetKeysForAction("BuyButton");
            if (keys != null && keys.Count > 0)
            {
                string firstKey = keys[1];
                buyAtout.text = "Press <sprite name=\"" + firstKey + "\"> to buy the " + nameAtout + " for :";
            }
        }
        if (Physics.Raycast(ray, out hit, 1000, excludeLayer))
        {
            if (hit.collider != null && hit.collider.gameObject == collider && distanceToPlayer < distanceDetection)
            {
                // AfficherIndicationAchat();
            }
            else{
                // CacherIndicationAchat();
            }
        }
    }
}
