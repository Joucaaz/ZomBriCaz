using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RankingOnline : MonoBehaviour
{
    private int numeroRank = 1;
    public Color32 colorGold;
    public Color32 colorSilver;
    public Color32 colorBronze;
    public Color32 colorElse;

    private Color32 currentColor;

    public GameObject prefabRankings;
    public Transform parentRankings;
    // Start is called before the first frame update
    void Start()
    {
        string url = "https://joudcazeaux.fr/ZomBriCaz/zombricazRankingsOnline.php";
        StartCoroutine(GetHighscoreData(url));
    }

    void Update()
    {
        
    }

    IEnumerator GetHighscoreData(string uri)
    {
        

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur de connexion : " + webRequest.error);
            }
            else
            {
                // Récupérer la chaîne de texte des données
                string dataText = webRequest.downloadHandler.text;
                Debug.Log("1 : " + dataText);

                // Diviser la chaîne de texte en lignes
                // string[] lines = dataText.Split('.');
                string[] lines = dataText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                
                // Traiter chaque ligne
                foreach (string line in lines)
                {

                    Debug.Log("line : " + line);
                    GameObject instance = Instantiate(prefabRankings, parentRankings);
                    Transform fondNumberRank = instance.transform.Find("NumberRowFond");

                    // Transform child0 = instance.transform.Find("text1");
                    // Transform child1 = instance.transform.Find("text1");
                    // Transform child2 = instance.transform.Find("text1");
                    // Transform child3 = instance.transform.Find("text1");

                    Transform[] childTransforms = new Transform[5]; // Assurez-vous d'avoir le bon nombre d'éléments

                    // Assigner les Transform des enfants à partir des noms
                    for (int i = 0; i <= 4; i++)
                    {
                        childTransforms[i] = instance.transform.Find("Text" + i.ToString());
                    }
                    switch (numeroRank)
                    {
                        case 1:
                            currentColor = colorGold;
                            break;
                        case 2:
                            currentColor = colorSilver;
                            break;
                        case 3:
                            currentColor = colorBronze;
                            break;
                        default:
                            currentColor = colorElse;
                            break;
                    }

                    childTransforms[0].gameObject.GetComponent<TextMeshProUGUI>().text = numeroRank.ToString();
                    fondNumberRank.gameObject.GetComponent<UnityEngine.UI.Image>().color = currentColor;

                    string[] values = line.Split(',');

                    for (int i = 1; i < childTransforms.Length; i++)
                    {
                        if (childTransforms[i] != null)
                        {
                            // Assigner la valeur de allValue à chaque enfant
                            childTransforms[i].gameObject.GetComponent<TextMeshProUGUI>().text = values[i-1];
                        }
                        else{
                            Debug.LogError("Enfant 'text" + i + "' non trouvé !");
                        }
                    }
                    numeroRank += 1;

                    // Afficher les valeurs (ajustez cela en fonction de votre logique de jeu)
                    foreach (string value in values)
                    {
                        Debug.Log(value); // Trim() pour supprimer les espaces autour des valeurs
                    }
                }
            }
        }
    }
}
