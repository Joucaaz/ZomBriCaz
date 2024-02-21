using UnityEngine;
using System.Collections.Generic;

public class CutCondition : MonoBehaviour
{
    // Référence au GameObject parent
    public GameObject parentObject;
    public GameObject player;

    // Liste des objets enfants
    private List<GameObject> enfants;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Initialisation de la liste des enfants
        enfants = new List<GameObject>();

        // Vérification que le parentObject n'est pas null
        if (parentObject != null)
        {
            // Récupérer tous les objets enfants du parentObject
            Transform parentTransform = parentObject.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                enfants.Add(child.gameObject);
            }

            
        }
    }

    void cutTouch()
    {
        foreach (GameObject enfant in enfants)
        {
            Zombie zombieComponent = enfant.GetComponent<Zombie>();

            if (zombieComponent != null)
            {
                // Obtenez la direction du joueur vers le zombie.
                Vector3 playerToZombie = enfant.transform.position - player.transform.position;

                // Obtenez la direction du joueur vers l'avant (ou la direction de regard).
                Vector3 playerForward = player.transform.forward;

                // Normalisez les vecteurs pour obtenir des vecteurs unitaires.
                playerToZombie.Normalize();
                playerForward.Normalize();

                // Calculez le produit scalaire entre les deux vecteurs.
                float dotProduct = Vector3.Dot(playerForward, playerToZombie);

                // Si le produit scalaire est positif, le joueur est face au zombie.
                if (dotProduct > 0.0f)
                {
                    float distance = Vector3.Distance(enfant.transform.position, player.transform.position);

                    if (distance <= 2.0f)
                    {
                        zombieComponent.zombieHealth -= zombieComponent.zombieHealth / 1;
                        if (zombieComponent.zombieHealth <= 0)
                        {
                            zombieComponent.zombieDie();
                        }
                    }
                }
            }
        }

    }

}
