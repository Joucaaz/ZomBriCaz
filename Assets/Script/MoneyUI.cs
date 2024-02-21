using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] public GameObject moneyTextPrefab;
    public Transform moneyTextParent;
    private Animator moneyAnimator;
    public Color32 colorRed = new Color32(229, 6, 6, 255);
    public Color32 colorGreen = new Color32(46, 169, 19, 255);

    public void UpdateMoney(int amount)
    {
        GameObject moneyTextObject = Instantiate(moneyTextPrefab, moneyTextParent);

        // Récupérer le composant TextMeshProUGUI du nouvel objet
        TextMeshProUGUI moneyText = moneyTextObject.GetComponent<TextMeshProUGUI>();
        moneyAnimator = moneyTextObject.GetComponent<Animator>();

        // Vérifier si le composant TextMeshProUGUI est présent
        if (moneyText != null)
        {
            // Configurer le texte en fonction du montant
            string sign = amount >= 0 ? "+" : "-";
            moneyText.text = sign + Mathf.Abs(amount).ToString();

            // Changer la couleur du texte en fonction du signe du montant
            moneyText.color = amount >= 0 ? colorGreen : colorRed;

            // Lancer l'animation
            moneyAnimator.SetTrigger(amount >= 0 ? "GainMoney" : "LoseMoney");

            // Ajouter un léger décalage à la position du nouvel objet
            Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0f, 0f); // À ajuster selon vos besoins
            moneyTextObject.transform.localPosition += offset;
        }
        else
        {
            Debug.LogError("Le composant TextMeshProUGUI est manquant dans le préfabriqué.");
        }
    }

}
