using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject AllPlayer;
    [SerializeField] private AudioClip clipNewWave;

    public int initialZombies = 5;
    public int lastInitialZombies;
    //public int newInitialZombies;
    public int currentZombieInGame;
    public float spawningDelay = 0.5f;
/*
    [SerializeField] public GameObject[] spawnersPort;
    [SerializeField] public GameObject[] spawnersFoot;
    [SerializeField] public GameObject[] spawnersCars;*/

    public GameObject[] spawners;

    public List<Zombie> zombiesList = new List<Zombie>(); 

    public GameObject[] zombiePrefab; 
    public ChargerUI timer;
    [SerializeField] public GameObject zombiesParent;

    public int numberOfWave = 0;
    public int nbKills = 0;
    public GameObject chargerUI;

    void Start()
    {
        spawners = new GameObject[9];
        for(int i=0; i<spawners.Length; i++){
            spawners[i] = transform.GetChild(i).gameObject;
        }

        player = GameObject.FindGameObjectWithTag("PlayerCam");
        AllPlayer = GameObject.FindGameObjectWithTag("Player");
        lastInitialZombies = initialZombies;
        InvokeRepeating("startNextWave", 0f, 45f);
    }

    void Update()
    {
        // Debug.Log("nbkills : " + nbKills);

        List<Zombie> zombiesToRemove = new List<Zombie>();
        foreach(Zombie zombie in zombiesList){
            if(zombie.zombieHealth <= 0){
                zombiesToRemove.Add(zombie);
                nbKills +=1;
                AllPlayer.GetComponent<PlayerMovement2>().money += 30;
                chargerUI.GetComponent<MoneyUI>().UpdateMoney(30);
            }
        }
        foreach(Zombie zombie in zombiesToRemove){
            zombiesList.Remove(zombie);
        }
        zombiesToRemove.Clear();
    }

   int GetRandomNumberWithExclusions(int min, int max, List<int> exclusions)
    {
        int randomNumber;

        // Répétez jusqu'à obtenir un nombre qui n'est pas dans la liste d'exclusions
        do
        {
            randomNumber = Random.Range(min, max+1); // Utilisez Random.Range pour Unity
        } while (exclusions.Contains(randomNumber));

        return randomNumber;
    }
    private void startNextWave(){
        SoundManager.Instance.PlaySound(clipNewWave);
        numberOfWave ++;
        if(numberOfWave != 1){
            AllPlayer.GetComponent<PlayerMovement2>().money += 100;
            chargerUI.GetComponent<MoneyUI>().UpdateMoney(100);
        }        
        
        StartCoroutine(SpawnWave());
    }
    
    private IEnumerator SpawnWave()
    {

        for(int i = 0; i<lastInitialZombies; i++){
            Vector3 spawnPosition;
            // spawnPosition = spawners[0].transform.position;
            // var zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPosition, Quaternion.identity);
            // Zombie zombieScript = zombie.GetComponent<Zombie>();
            // zombiesList.Add(zombieScript);
            if(player.transform.position.z > 32){

                // Debug.Log("POS : port");
                List<int> exclusions = new List<int> { 0, 1, 2 };
                int randomNumber = GetRandomNumberWithExclusions(0, 8, exclusions);

                spawnPosition = spawners[randomNumber].transform.position;
                var zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPosition, Quaternion.identity);
                zombie.transform.parent = zombiesParent.transform;
                Zombie zombieScript = zombie.GetComponent<Zombie>();
                zombiesList.Add(zombieScript);
            
            }
            else if(player.transform.position.x>19){
                
                // Debug.Log("POS : cars");
                List<int> exclusions = new List<int> { 6, 7, 8 };
                int randomNumber = GetRandomNumberWithExclusions(0, 8, exclusions);

                spawnPosition = spawners[randomNumber].transform.position;
                var zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPosition, Quaternion.identity);
                zombie.transform.parent = zombiesParent.transform;
                Zombie zombieScript = zombie.GetComponent<Zombie>();
                zombiesList.Add(zombieScript);
            }
            else if(player.transform.position.x<-8.4){
                
                // Debug.Log("POS : Foot");
                List<int> exclusions = new List<int> { 3, 4, 5 };
                int randomNumber = GetRandomNumberWithExclusions(0, 8, exclusions);

                spawnPosition = spawners[randomNumber].transform.position;
                var zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPosition, Quaternion.identity);
                zombie.transform.parent = zombiesParent.transform;
                Zombie zombieScript = zombie.GetComponent<Zombie>();
                zombiesList.Add(zombieScript);  

            }
            else{

                // Debug.Log("POS : else");
                spawnPosition = spawners[Random.Range(0,spawners.Length)].transform.position;
                var zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPosition, Quaternion.identity);
                zombie.transform.parent = zombiesParent.transform;
                Zombie zombieScript = zombie.GetComponent<Zombie>();
                zombiesList.Add(zombieScript);

            }
            yield return new WaitForSeconds(spawningDelay); 
            
        }
        lastInitialZombies = lastInitialZombies+1;
    }
}
